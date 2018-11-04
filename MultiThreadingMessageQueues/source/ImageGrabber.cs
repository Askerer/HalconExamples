using System;
using System.Collections.Generic;
using HalconDotNet;
using System.Threading;

namespace HALCONActors
{
  /// <summary>
  /// ImageGrabber is the first element in the chain of responsibility pattern.
  /// Its purpose is to actively grab images and pass them on to an image processing
  /// thread.
  /// </summary>
  public class ImageGrabber : HActor
  {
    protected HFramegrabber grabber;

    public override HMessage ActorTask(HMessage obj)
    {
      if (obj != null)
        obj.Dispose();
      HMessage msg = new HMessage();
      HImage img = grabber.GrabImageAsync(-1);
      msg.SetMessageObj(img, "image");
      if(SlowMotion == true)
        HOperatorSet.WaitSeconds(0.2);
      return msg;
    }

    public ImageGrabber(string path, IHActor destination) : 
      base(destination,false)
    {
      SlowMotion = true;
      if (path == null)
        throw new ArgumentNullException();
      grabber = new HFramegrabber("File", 1, 1, 0, 0, 0, 0,
          "default", -1, "default", -1, "false", path, "", 1, 2);
    }

    ~ImageGrabber()
    {
      if (IsRunning)
      {
        if (Next != null)
          Next.AddMessage(GenFinishMessage());
        Stop();
      }
      grabber.Dispose();
    }

    public bool SlowMotion
    {
      get;
      set;
    }
  }
}
