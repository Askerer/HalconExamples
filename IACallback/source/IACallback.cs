//
// HALCON/.NET (C#) image acquisition callback example
//
// © 2013-2018 MVTec Software GmbH
//
// IACallback.cs: Shows the usage of the image acquistion callbacks
//
// Example to demonstrate the usage of the HALCON user-specific callback
// functionality. The user may need to adapt the parameterization to open the
// required HALCON Image Acquisition interface with the corresponding device.
// Please see also the corresponding HALCON Image Acquisition Interface
// reference documentation, which lists the supported callback types.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

using HalconDotNet;

namespace IACallback
{

class Program
{
  // e.g. Path to the configuration file
  static readonly HTuple cCameraType = "default";

  // Handles to assure a correct thread synchronisation.
  static Thread gThreadHandle;

  static HCondition    gAcqCondition;
  static HMutex        gAcqMutex;
  static long          gCountCallbacks = 0;
  static long          gCountSignals = 0;
  static volatile bool gThreadRunning;
  static readonly int  cMaxGrabImages = 10;

  struct Payload {

    public readonly HTuple eventType;
    public readonly HFramegrabber acq;
    public Payload (HFramegrabber usrAcq, HTuple usrEventType)
    {
        acq = usrAcq;
        eventType = usrEventType; 
    }

  }

  

  // Import required functions to disable error dialog boxes on
  // interface detection.
  [DllImport("kernel32.dll")]
  static extern ErrorModes SetErrorMode(ErrorModes uMode);
  [DllImport("kernel32.dll")]
  static extern ErrorModes GetErrorMode();
  [Flags]
  public enum ErrorModes : uint
  {
    SYSTEM_DEFAULT = 0x0,
    SEM_FAILCRITICALERRORS = 0x0001,
    SEM_NOALIGNMENTFAULTEXCEPT = 0x0004,
    SEM_NOGPFAULTERRORBOX = 0x0002,
    SEM_NOOPENFILEERRORBOX = 0x8000
  }

  /**
   * Define a user-specific callback function.
   *
   * @param handle Acquisition handle.
   * @param context Pointer to interface-specific context data.
   * @param userContext Pointer to user-specific context data.
   */
  public static int  myCallbackFunction(IntPtr handle, IntPtr context,
                                        IntPtr userContext)
  {
    HTuple values, value, data, position;

    Payload helper =
        (Payload)Marshal.PtrToStructure(userContext, typeof(Payload));

    gAcqMutex.LockMutex();
    gCountCallbacks++;

    if (gThreadRunning)
    {
      Console.Write("callback_" + gCountCallbacks + "-> AcqHandle " + handle +
                    " called by UserContext " + helper.eventType.S.ToString());

      // Check if GenICam Event Data available and show it.
      try
      {
        values = helper.acq.GetFramegrabberParam("available_param_names");
        position = values.TupleStrstr(helper.eventType);
        if (position.Length > 0)
        {
            data = values.TupleSelectMask(position += 1);
            if (data.Length > 1)
            {
                Console.WriteLine(", with event data: ");
                for (int i = 0; i < data.Length; i++)
                {
                    // Exclude the event feature itself.
                    if (data.TupleSelect(i) != helper.eventType)
                    {
                        Console.Write("          " +
                                      data.TupleSelect(i).S.ToString() +
                                      " = ");
                        value =
                          helper.acq.GetFramegrabberParam(data.TupleSelect(i));
                        if (value.Length > 0)
                            Console.WriteLine(value.ToString());
                    }
                }
            }
            else
            {
                Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine();
        }
      }
      catch (HalconException exc)
      {
        Console.WriteLine(exc.GetErrorMessage());
      }
    }
    else
    {
      // If the worker thread finished, the callback type will be unregistered,
      // therefore just ignore the incoming callbacks in the meantime.
      Console.WriteLine("callback_" + gCountCallbacks + "-> Ignored");
    }

    gAcqCondition.SignalCondition();
    gAcqMutex.UnlockMutex();

    return 0;
  }

  /**
   * This reference must be kept alive until callback is no longer
   * used by the interface (or else crashes after garbage collection).
   */
  static HalconDotNet.HalconAPI.HFramegrabberCallback gDelegateCallback =
      myCallbackFunction;

  /**
   * Read the user selected option.
   *
   * @param options Total number of available options.
   * @return Selected option.
   */
  static int readUserSelection(int options)
  {
    int    selected = 0;
    string line;

    line = Console.ReadLine();

    while (!Int32.TryParse(line, out selected) || selected < 1 ||
           selected > options)
    {
      Console.WriteLine("Entry: " + line);
      Console.Write("Invalid input." + System.Environment.NewLine +
                    "Select a listed entry: ");
      line = Console.ReadLine();
    }

    return selected;
  }

  /**
   * Wait for user interaction.
   */
  static void waitUserInteraction()
  {
    Console.WriteLine(System.Environment.NewLine + System.Environment.NewLine +
                      "Type any key and press 'ENTER' to exit" +
                      System.Environment.NewLine + System.Environment.NewLine);
    Console.ReadLine();
  }

  /**
   * Display all available interfaces that have at least one device available.
   *
   * @return List of the detected interfaces.
   */
  static HTuple showAvailableInterfaces()
  {
    HTuple deviceInfo, deviceValue, interfacesAll, temp;
    HTuple interfaces = new HTuple();

    Console.WriteLine(System.Environment.NewLine +
                      "Detect and show all available interfaces..." +
                      System.Environment.NewLine);
    interfacesAll = HInfo.GetParamInfo("info_framegrabber", "Name", "values");

    if (interfacesAll.Length > 0)
    {
        Console.WriteLine("Available interfaces:");
    }
    else
    {
        Console.WriteLine("Found no interfaces.");
        return interfaces;
    }

    ErrorModes errorMode = GetErrorMode();
    // Disable error dialog boxes on interface detection.
    SetErrorMode(ErrorModes.SEM_FAILCRITICALERRORS |
                 ErrorModes.SEM_NOGPFAULTERRORBOX |
                 ErrorModes.SEM_NOOPENFILEERRORBOX);

    for (int i = 0; i < interfacesAll.Length; i++)
    {
      try
      {
        temp = interfacesAll.TupleSelect(i);
        deviceInfo = HInfo.InfoFramegrabber(temp.S.ToString(), "device",
                                            out deviceValue);
      }
      catch (HalconException)
      {
        // Interface not available.
        continue;
      }

      if (deviceValue.Length > 0)
      {
        interfaces.Append(interfacesAll.TupleSelect(i));
        Console.WriteLine(interfaces.Length + ")" +
                          interfacesAll.TupleSelect(i).S.ToString());
      }
    }

    // Restore previous error mode.
    SetErrorMode(errorMode);

    return interfaces;
  }

  /**
   * Display all the available devices of an acquisition interface.
   *
   * @param AcqInterface Acquisition interface.
   * @return List of the detected devices.
   */
  static HTuple showAvailableDevices(HTuple AcqInterface)
  {
    HTuple devices;

    Console.WriteLine(System.Environment.NewLine +
                      "Detect and show all available devices..." +
                      System.Environment.NewLine);
    HInfo.InfoFramegrabber(AcqInterface.S.ToString(), "device", out devices);

    if (devices.Length > 0)
    {
        Console.WriteLine("Available devices:");
    }
    else
    {
        Console.WriteLine("Found no devices.");
        return devices;
    }

    for (int i = 0; i < devices.Length; i++)
      Console.WriteLine((i + 1) + ")" + devices.TupleSelect(i).S.ToString());

    return devices;
  }

  /**
   * Display all the supported HALCON Image Acquisition callbacks.
   *
   * @param acq Reference to the initialized acquisition interface.
   * @return List of the available callbacks.
   */
  static HTuple showAvailableCallbackTypes(ref HFramegrabber acq)
  {
    HTuple callbacks;

    Console.WriteLine("Get and show the supported callback types using the " +
                      System.Environment.NewLine +
                      "parameter 'available_callback_types'..." +
                      System.Environment.NewLine);

    callbacks = acq.GetFramegrabberParam("available_callback_types");

    if (callbacks.Length > 0)
    {
        Console.WriteLine("Available callback types:" +
                          System.Environment.NewLine);
    }
    else
    {
        Console.WriteLine("No callback types supported by the selected device"
                          + System.Environment.NewLine);
        return callbacks;
    }

    for (int i = 0; i < callbacks.Length; i++)
      Console.WriteLine((i + 1) + ")" + callbacks.TupleSelect(i).S.ToString());

    return callbacks;
  }

  /**
   * Register the callback type selected by the user.
   *
   * @param acq Reference to an acquisition interface.
   * @param callbackType Selected callback to be registered.
   * @param userContext Object to be used at the other side of the callback.
   * @return If the registration was sucessfull.
   */
  static bool registerCallbackType(ref HFramegrabber acq, HTuple callbackType,
                                   object userContext)
  {
    IntPtr contextPtr, funcPtr;

    funcPtr = Marshal.GetFunctionPointerForDelegate(gDelegateCallback);
    contextPtr = Marshal.AllocHGlobal(Marshal.SizeOf(userContext));
    Marshal.StructureToPtr(userContext, contextPtr, false);

    try
    {
      // If the user context is not needed, IntPtr.Zero can be passed.
      acq.SetFramegrabberCallback(callbackType.S.ToString(), funcPtr,
                                  contextPtr);
    }
    catch (HalconException exc)
    {
      Console.WriteLine(System.Environment.NewLine + "Callback " +
                        callbackType.S.ToString() + " registration failed!");
      Console.WriteLine(exc.GetErrorMessage());
      return false;
    }

    return true;
  }

  /**
   * Work thread to manage the callbacks and grabbing images.
   *
   * @param context Worker thread context.
   */
  static void workerThread(object context)
  {
    Payload helper = (Payload)context;
    HImage  image;

    gAcqMutex.LockMutex();
    gThreadRunning = true;

    while (true)
    {
      // Enter the WaitCondition function with a locked mutex to avoid
      // problems with the image acquisition handle. WaitCondition atomically
      // unlocks the MutexHandle (UnlockMutex) and waits for the condition
      // variable ConditionHandle to be signaled. The WaitCondition function
      // will always exit with a locked mutex handle.
      gAcqCondition.WaitCondition(gAcqMutex);
      gCountSignals++;
      if (!gThreadRunning)
      {
        Console.WriteLine("Example aborted");
        break;
      }

      Console.WriteLine("workerTh_" + gCountSignals +
                        "-> Do the thread specific calculations");
      try
      {
        image = helper.acq.GrabImageAsync(-1);
      }
      catch (HalconException exc)
      {
        Console.WriteLine(exc.GetErrorMessage());
      }

      // cMaxGrabImages callbacks already processed?
      if (gCountSignals == cMaxGrabImages)
      {
        gThreadRunning = false;
        // Unlock the mutex to let the callback end, if not, it will not be
        // possible to unregister the callback.
        gAcqMutex.UnlockMutex();
        Console.WriteLine("End of the example!");
        // Unregister the callback here to avoid further incoming callbacks.
        try
        {
          helper.acq.SetFramegrabberCallback(helper.eventType.S.ToString(),
                                             IntPtr.Zero, IntPtr.Zero);
        }
        catch (HalconException exc)
        {
          Console.WriteLine(exc.GetErrorMessage());
        }

        Console.WriteLine(System.Environment.NewLine +
                          "Type any key and press 'ENTER' to exit" +
                          System.Environment.NewLine);
        break;
      }
    }
    gAcqMutex.UnlockMutex();
  }

  /**
   * Clean up an exit the program.
   *
   * @param acq Acquisition handle.
   * @param callbackTpye Registered callback.
   * @param waiting Activate waiting for user interaction.
   */
  static void exitProgram(ref HFramegrabber acq, string callbackType,
                          bool waiting)
  {
    if (waiting)
      waitUserInteraction();

    if (null == acq)
      System.Environment.Exit(1);

    // Unregister the callback.
    if (null != callbackType)
    {
      try
      {
        IntPtr currentCallback, context;
        currentCallback =
            acq.GetFramegrabberCallback(callbackType, out context);
        // Already unregistered?
        if (IntPtr.Zero != currentCallback)
          acq.SetFramegrabberCallback(callbackType, IntPtr.Zero, IntPtr.Zero);
      }
      catch (HalconException exc)
      {
        Console.WriteLine(exc.GetErrorMessage());
      }
    }

    // Abort the workerThread.
    if (gThreadRunning)
    {
      try
      {
        // abort the current grab.
        acq.SetFramegrabberParam("do_abort_grab", 1);
      }
      catch (HalconException exc)
      {
        Console.WriteLine(exc.GetErrorMessage());
      }
      gAcqMutex.LockMutex();
      gThreadRunning = false;
      gAcqCondition.SignalCondition();
      gAcqMutex.UnlockMutex();
      gThreadHandle.Join();
    }

    try
    {
      // Close the framegrabber.
      acq.Dispose();
      // Clean up.
      gAcqMutex.Dispose();
      gAcqCondition.Dispose();
    }
    catch (HalconException exc)
    {
      Console.WriteLine(exc.GetErrorMessage());
    }

    System.Environment.Exit(1);
  }

  static void Main(string[] args)
  {
    HTuple        interfaces;
    HTuple        devices;
    HTuple        callbackTypes;
    HTuple        openDefaults;
    long          callbackIndex;
    long          interfaceIndex;
    long          deviceIndex;
    HFramegrabber acq = null;
    Payload       userContext = default(Payload);
    bool          bWaitUserInteraction = true;

    try
    {
      gAcqCondition = new HCondition("", "");
      gAcqMutex = new HMutex("", "");

      // Get available interfaces.
      interfaces = showAvailableInterfaces();

      if (interfaces.Length < 1)
        exitProgram(ref acq, null, bWaitUserInteraction);

      Console.Write(System.Environment.NewLine +
                    "Enter the number of the interface you want to use: ");

      interfaceIndex = readUserSelection(interfaces.Length);

      // Show selected interface and revision.
      HTuple revision;
      HInfo.InfoFramegrabber(
          interfaces.TupleSelect(interfaceIndex - 1).S.ToString(), "revision",
          out revision);
      Console.WriteLine(
          System.Environment.NewLine + "Interface selected: " +
          interfaces.TupleSelect(interfaceIndex - 1).S.ToString() + " (Rev. " +
          revision.S.ToString() + ")");

      // Get available devices for the selected interface.
      devices =
          showAvailableDevices(interfaces.TupleSelect(interfaceIndex - 1));

      if (devices.Length < 1)
        exitProgram(ref acq, null, bWaitUserInteraction);

      Console.Write(System.Environment.NewLine +
                    "Enter the number of the device you want to connect to: ");

      deviceIndex = readUserSelection(devices.Length);

      // Get open_framegrabber default values and open the selected device.
      Console.WriteLine(
          System.Environment.NewLine +
          "Open the specified device by calling open_framegrabber()...");

      HInfo.InfoFramegrabber(
          interfaces.TupleSelect(interfaceIndex - 1).S.ToString(), "defaults",
          out openDefaults);

      int vRes = 0, hRes = 0, iWidth = 0, iHeight = 0, sRow = 0, sColumn = 0;
      try
      {
        hRes = Convert.ToInt32(openDefaults.TupleSelect(0).L);
        vRes = Convert.ToInt32(openDefaults.TupleSelect(1).L);
        iWidth = Convert.ToInt32(openDefaults.TupleSelect(2).L);
        iHeight = Convert.ToInt32(openDefaults.TupleSelect(3).L);
        sRow = Convert.ToInt32(openDefaults.TupleSelect(4).L);
        sColumn = Convert.ToInt32(openDefaults.TupleSelect(5).L);
      }
      catch (Exception)
      {
        // Failed to convert one of the parameters.
        Console.WriteLine("Failed to convert long to int");
        exitProgram(ref acq, null, bWaitUserInteraction);
      }
      acq = new HFramegrabber(
          interfaces.TupleSelect(interfaceIndex - 1).S.ToString(), hRes, vRes,
          iWidth, iHeight, sRow, sColumn,
          openDefaults.TupleSelect(6).S.ToString(),
          openDefaults.TupleSelect(7), openDefaults.TupleSelect(8),
          openDefaults.TupleSelect(9),
          openDefaults.TupleSelect(10).S.ToString(), cCameraType,
          devices.TupleSelect(deviceIndex - 1), openDefaults.TupleSelect(13),
          openDefaults.TupleSelect(14));

      // If the interface supports it, enable a helper that activates GenICam
      // events during SetFramegrabberCallback.
      try
      {
        acq.SetFramegrabberParam("event_notification_helper", "enable");
      }
      catch (Exception) {}

      // Show the available callback types and register a specific one.
      callbackTypes = showAvailableCallbackTypes(ref acq);

      if (callbackTypes.Length < 1)
        exitProgram(ref acq, null, bWaitUserInteraction);

      Console.Write(System.Environment.NewLine + "Enter the number of the " +
                    "callback type you like to register: ");

      callbackIndex = readUserSelection(callbackTypes.Length);

      // Use the userContext to pass your own class or structure with the
      // information that you need on the other side of the callback.
      // Since we are using Thread.Start(obj) save the variables we need 
      // in userContext, before passing it to the Thread.
      // Note that if you want to do changes to 'acq' on different threads 
      // the you should sincronize all thread an use a single
      // 'acq' reference. Additionally, if you change any of the 
      //  internal values of 'userContext' will not be reflected 
      //  on the other side.
      userContext = new Payload(acq, callbackTypes.TupleSelect(callbackIndex - 1));

      // Start worker thread to manage the incoming callbacks. With the 
      // provided userContext.
      gThreadRunning = false;
      gThreadHandle = new Thread(new ParameterizedThreadStart(workerThread));
      gThreadHandle.Start(userContext);

      gAcqMutex.LockMutex();

      // Register the callback after we start the thread. This is to avoid
      // cases where the events start comming immediately. If you don't do
      // it this way you might lose some events.
      if (!registerCallbackType(ref acq, userContext.eventType, userContext))
      {
        gAcqMutex.UnlockMutex();
        exitProgram(ref acq, userContext.eventType.S.ToString(),
                    bWaitUserInteraction);
      }

      // Please place your code here, to force the execution of a specific
      // callback type.

      Console.WriteLine(
          System.Environment.NewLine +
          "If the registered event is signaled, the previously registered" +
          System.Environment.NewLine +
          "user-specific callback function will be executed." +
          System.Environment.NewLine);

      // e.g. 'ExposureEnd'
      Console.WriteLine(
          "Start grabbing an image." + System.Environment.NewLine +
          "If e.g. an exposure_end was registered, the image grabbing will " +
          System.Environment.NewLine +
          "force the execution of the user-specific callback function." +
          System.Environment.NewLine);

      Console.WriteLine("This example is going to process " + cMaxGrabImages +
                        " incoming callbacks");

      try
      {
        acq.GrabImageStart(-1);
      }
      catch (HalconException exc)
      {
        Console.WriteLine(exc.GetErrorMessage());
      }
      gAcqMutex.UnlockMutex();

      // Wait for user interaction to kill the program.
      waitUserInteraction();
      bWaitUserInteraction = false;
      exitProgram(ref acq, userContext.eventType.S.ToString(),
                  bWaitUserInteraction);
    }
    catch (HOperatorException exc)
    {
      Console.WriteLine(exc.GetErrorMessage());

      if (exc.GetExtendedErrorCode() != 0 ||
          exc.GetExtendedErrorMessage().Length > 0)
      {
        Console.WriteLine("Extended error code: " +
                          exc.GetExtendedErrorCode());
        Console.WriteLine("Extended error message: " +
                          exc.GetExtendedErrorMessage());
      }

      if (userContext.eventType == null || userContext.eventType.Length == 0)
      {
          exitProgram(ref acq, null, bWaitUserInteraction);
      }
      else
      {
          exitProgram(ref acq, userContext.eventType.S.ToString(),
                      bWaitUserInteraction);
      }
    }
  }
}
}
