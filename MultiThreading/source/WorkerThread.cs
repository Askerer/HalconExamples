//
// HALCON/.NET (C#) multithreading example
//
// © 2007-2018 MVTec Software GmbH
//
// WorkerThread.cs: Defines the behavior of the worker threads.
//

using System;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections;

using HalconDotNet;

/////////////////////////////////////////////////////////////////////////////
// Detailed information:
// When using multiple threads you have to ensure that the data shared
// is valid at any time of the execution. For this, mutexes are used to
// guarantee mutual access to shared data objects. Besides, event handles
// are used to synchronize the threads with each other.
//
// The GUI depicts the main thread of the application, which is also in charge
// of displaying the results.
//
// When you press the Start button, the two (secondary) thread handles
// (threadAcq and threadIP) are 'triggered', which then start the (global) thread
// functions ImgAcqRun (image acquisition) and IPRun (image processing), respectively.
// Since these processing tasks are encapsulated units, the necessary handles
// and variables are initialized and closed within the thread functions.
//
// When you press the Stop button, the StopEvent is sent, which causes all
// threads to finish their current procedure, close all handles opened
// initially, and leave the coresponding thread function.
//
// The threads share the following data:
//
//   threadAcq & threadIP:   image    (ArrayList  imgList)
//
//   threadIP & main thread: results  (struct ResultContainer  resultData)
//
// The two variables are protected by the following mutexes:
//
//   imgList     => newImageMutex
//
//   resultData  => resultDataMutex
//
// Events exchanged among threads are as follows:
//
//   threadAcq -> threadIP:    newImageEvent
//
//   threadIP  -> main thread: newResultEvent
//
//   threadIP  <- main thread: containerIsFreeEvent
//
// All thread handles also listen for the StopEvent triggered by
// the Stop button.
//
/////////////////////////////////////////////////////////////////////////////

namespace MultiThreading
{
  delegate void FuncDelegate();

  public class WorkerThread
  {

    // shared data and mutexes
    ResultContainer   resultData;
    ArrayList         imgList;
    Mutex             newImgMutex;
    Mutex             resultDataMutex;

    // event handles to synchronize threads
    AutoResetEvent    newImgEvent;
    AutoResetEvent    newResultEvent;
    ManualResetEvent  containerIsFreeEvent;
    ManualResetEvent  delegatedStopEvent;

    // access to instances of GUI
    MultiThreadingForm mainForm;
    HWindow window = null;

    // auxiliary variables
    FuncDelegate delegateDisplay;
    FuncDelegate delegateControlReset;
    const int MAX_BUFFERS = 10;


    // constructor: set up class members
    public WorkerThread(MultiThreadingForm form)
    {
      newImgEvent          = new AutoResetEvent(false);
      newResultEvent       = new AutoResetEvent(false);
      containerIsFreeEvent = new ManualResetEvent(true);

      resultData      = new ResultContainer();
      newImgMutex     = new Mutex();
      resultDataMutex = new Mutex();

      mainForm = form;
      delegatedStopEvent = form.stopEventHandle;

      delegateDisplay = new FuncDelegate(DisplayResults);
      delegateControlReset = new FuncDelegate(mainForm.ResetControls);
      imgList = new ArrayList();
    }

    //////////////////////////////////////////////////////////////////////////////
    //  Init() - The event handles used to synchronize the threads must be
    //           reset before a new thread.Start() can be used.
    //           If the imageList buffer wasn't processed completely (during
    //           the last run), the list needs to be emptied before it is
    //           used for the next run.
    //////////////////////////////////////////////////////////////////////////////
    public void Init()
    {
      newImgEvent.Reset();
      newResultEvent.Reset();
      containerIsFreeEvent.Set();

      int length = imgList.Count;
      for (int  i=0; i<length; i++)
      {
        HImage image = (HImage) imgList[0];
        imgList.Remove(image);
        image.Dispose();
      }

      window = mainForm.GetHalconWindow();
    }

    //////////////////////////////////////////////////////////////////////////////
    // DisplayResults() - This method is used in/as a delegate. It is invoked
    //                    from the main GUI thread
    //////////////////////////////////////////////////////////////////////////////
    public void DisplayResults()
    {
      int i;

      resultDataMutex.WaitOne();                              // CriticalSect
      HTuple time               = resultData.timeNeeded;      // CriticalSect
      HTuple decodedDataStrings = resultData.decodedData;     // CriticalSect
      HImage image              = resultData.resultImg;       // CriticalSect
      HTuple resultHandle       = resultData.resultHandle;    // CriticalSect
      HXLD   symbolXLDs         = resultData.symbolData;      // CriticalSect
      containerIsFreeEvent.Set();                             // CriticalSect
      resultDataMutex.ReleaseMutex();                         // CriticalSect

      window.DispObj(image);
      window.DispObj(symbolXLDs);

      mainForm.procTimeLabel.Text = time.TupleString(".1f") + "  ms";
      mainForm.procTimeLabel.Refresh();

      for (i = 0; i < resultHandle.Length; i++)
      {
        mainForm.imageDataLabel.Text = decodedDataStrings[i].S;
        mainForm.imageDataLabel.Refresh();
      }

      image.Dispose();
      symbolXLDs.Dispose();
    }

    //////////////////////////////////////////////////////////////////////////////
    // FGRun() - The thread functionFGRun is in charge of the asynchronous
    //           grabbing. To pass the images to the  other threads, we use
    //           a list. In case  the list exceeds a certain length, because
    //           the processing thread is slower then the grabbing thread,
    //           we omit new images until the list decreases again.
    //           To prevent data races, weuse a mutex to assure mutual
    //           access to the image list.
    //////////////////////////////////////////////////////////////////////////////
    public void ImgAcqRun()
    {
      // -------------------  INIT ----------------

      string sequenceName = "datacode/ecc200/ecc200.seq";

      HFramegrabber acquisition = new HFramegrabber("File",1,1,0,0,0,0,
        "default",-1,"default",-1,"default",sequenceName,"default",-1,-1);

      // -----------  WAIT FOR EVENTS  ---------------

      while (true)
      {
        HImage grabbedImage = acquisition.GrabImageAsync(-1);

        newImgMutex.WaitOne();                // CriticalSect
        if (imgList.Count < MAX_BUFFERS)      // CriticalSect
        {                                     // CriticalSect
          imgList.Add(grabbedImage);
        }
        else
        {
          grabbedImage.Dispose();
        }                                     // CriticalSect
        newImgMutex.ReleaseMutex();           // CriticalSect

        newImgEvent.Set();

        if (delegatedStopEvent.WaitOne(0, true))  break;
      }

      // --------  RESET/CLOSE ALL HANDLES  ---------

      acquisition.Dispose();
      newImgEvent.Reset();

      return;
    }


    //////////////////////////////////////////////////////////////////////////////
    //  IPRun() - The thread function IPRun performs the image processing.
    //            It waits for the grabbing thread to indicate a new image in the
    //            image list. After calling the operator FindDataCode2D, the
    //            result values are stored in the ResultContainer instance
    //            resultData, which can be entered only after the previous result
    //            values were displayed (containerIsFree-event).
    //////////////////////////////////////////////////////////////////////////////
    public void IPRun()
    {
      // -------------------  INIT ----------------

      HDataCode2D reader = new HDataCode2D("Data Matrix ECC 200",
        new HTuple(), new HTuple());

      reader.SetDataCode2dParam("default_parameters", "enhanced_recognition");

      // -----------  WAIT FOR EVENTS  ---------------

      while (newImgEvent.WaitOne())
      {
        newImgMutex.WaitOne();              // CriticalSect
        HImage image = (HImage)imgList[0];  // CriticalSect
        imgList.Remove(image);              // CriticalSect
        newImgMutex.ReleaseMutex();         // CriticalSect

        HTuple t1 = HSystem.CountSeconds();

        HTuple decodedDataStrings, resultHandle;

        HXLD symbolXLDs = reader.FindDataCode2d(image, new  HTuple(),
          new HTuple(), out resultHandle, out decodedDataStrings);

        HTuple t2 = HSystem.CountSeconds();

        containerIsFreeEvent.WaitOne();
        resultDataMutex.WaitOne();                      // CriticalSect
        resultData.timeNeeded   = (1000*(t2-t1));       // CriticalSect
        resultData.decodedData  = decodedDataStrings;   // CriticalSect
        resultData.resultImg    = image;                // CriticalSect
        resultData.resultHandle = resultHandle;         // CriticalSect
        resultData.symbolData   = symbolXLDs;           // CriticalSect
        containerIsFreeEvent.Reset();                   // CriticalSect
        resultDataMutex.ReleaseMutex();                 // CriticalSect
        newResultEvent.Set();

        mainForm.Invoke(delegateDisplay);

        if (delegatedStopEvent.WaitOne(0, true)) break;

      }
      // --------  RESET/CLOSE ALL HANDLES  ---------

      mainForm.threadAcq.Join();
      mainForm.Invoke(delegateControlReset);

      reader.Dispose();

      newResultEvent.Reset();

      return;
    }
  } //end of  class

  ////////////////////////////////////////////////////////////////////////////////
  // class ResultContainer - This data structure is in charge of passing the result
  //                         values (obtained in the IPthread) to the main thread
  //                         for display.
  ////////////////////////////////////////////////////////////////////////////////
  public class ResultContainer
  {
    public HImage resultImg;
    public HXLD   symbolData;
    public HTuple resultHandle;
    public HTuple decodedData;
    public HTuple timeNeeded;

    public ResultContainer()
    {
    }
  } //end of  class

} //end of  namespace

