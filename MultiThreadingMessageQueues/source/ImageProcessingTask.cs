using System;
using System.Collections.Generic;
using System.Threading;
using HalconDotNet;

namespace HALCONActors
{
  /// <summary>
  /// ImageProcessingTask is a basic class that provides a thread, which waits
  /// for new inputs, performs an operation on them, and passes the result on to
  /// the next thread.
  ///
  /// If a drawing object is specified, the drawing object is used to specify
  /// the region of interest.
  ///
  /// The typical case is when the threads receive images from a framegrabber,
  /// process them and finally send the result to a HALCON window.
  /// </summary>
  public class ImageProcessingTask : HActor
  {
    private object wlock = new object();
    protected HDrawingObject drawing_obj;

    public ImageProcessingTask(HDrawingObject dobj) : base(true)
    {
      drawing_obj = dobj;
    }

    public ImageProcessingTask(HDrawingObject dobj, IHActor next) : base(next, true)
    {
      drawing_obj = dobj;
    }

    protected HDrawingObject ActiveDrawingObject
    {
      get
      {
        HDrawingObject obj;
        lock (wlock)
          obj = drawing_obj;
        return obj;
      }

      set
      {
        if (value == null)
          throw new ArgumentNullException();
        lock (wlock)
          drawing_obj = value;
      }
    }

    protected HObject ActiveObject
    {
      get
      {
        HObject hobj = null;
        lock (wlock)
        {
          if (drawing_obj != null)
            hobj = drawing_obj.GetDrawingObjectIconic();
        }
        return hobj;
      }
    }

    public override HMessage ActorTask(HMessage imsg)
    {
      HImage img = new HImage(imsg.GetMessageObj("image"));
      HRegion roi = new HRegion(ActiveObject);
      HObject obj = new HObject();
      if (roi != null)
      {
        obj.ConcatObj(img);
        obj.ConcatObj(img.ReduceDomain(roi).SobelAmp("sum_abs", 3));
      }
      else
        obj.ConcatObj(img.SobelAmp("sum_abs", 3));
      HMessage msg = new HMessage();
      msg.SetMessageObj(obj,"filter_image");
      return msg;
    }

    ~ImageProcessingTask()
    {
      if (IsRunning)
        Stop();
    }
  }
}
