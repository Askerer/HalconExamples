using System;
using System.Collections.Generic;
using HALCONActors;
using HalconDotNet;
using System.Threading;

namespace HALCONDrawingObjects
{
  /// <summary>
  /// The ShapeModelDisplay is responsible for carrying out the heavy operations
  /// (creating a new shape model and displaying its contours) in a separate
  /// thread in order to ensure a smooth interaction with the user.
  ///
  /// Notice that the corresponding thread is blocked waiting for new input.
  /// The input is provided by the callback delegate, i.e., the user interactions
  /// with the drawing object trigger the thread to perform the operation.
  /// </summary>
  public class ShapeModelDisplay : ImageProcessingTask
  {
    private HFramegrabber imggrabber;
    private HShapeModel sbm = null;
    private object lockobj = new object();

    public ShapeModelDisplay(string path, HDrawingObject obj, IHActor window)
      : base(obj, window)
    {
      if (path == null)
        throw new ArgumentNullException();
      imggrabber = new HFramegrabber("File", 1, 1, 0, 0, 0, 0,
          "default", -1, "default", -1, "false", path, "", 1, 2);
    }

    public HShapeModel GetShapeModel
    {
      get
      {
        HShapeModel shape;
        lock (lockobj)
          shape = sbm;
        return shape;
      }
    }

    public override HMessage ActorTask(HMessage obj)
    {
      HTuple isempty = msg_queue.GetMessageQueueParam("is_empty");
      if (isempty.I == 0)
        msg_queue.SetMessageQueueParam("flush_queue", 1);
      if (obj != null)
        obj.Dispose();
      HTuple row, column, angle, score;
      HImage img;
      img = imggrabber.GrabImageAsync(-1);
      HRegion roi = new HRegion(ActiveObject);

      HMessage msg = new HMessage();
      msg.SetMessageObj(img, "image");
      Monitor.Enter(lockobj);
      try
      {
        if (sbm != null)
          sbm.Dispose();
        sbm = new HShapeModel(img.ReduceDomain(roi), "auto", -Math.PI,
          2*Math.PI, "auto", "auto", "use_polarity", "auto", "auto");

        sbm.FindShapeModel(img, 0.0, 0.0, 0.5, 1, 0.5, "least_squares", 0, 0.9,
          out row, out column, out angle, out score);
        if (row.Length > 0)
        {
          HXLDCont contours = sbm.GetShapeModelContours(1);
          HHomMat2D homMat2D = new HHomMat2D();
          homMat2D.VectorAngleToRigid(0.0, 0.0, 0.0, row.D, column.D, angle.D);
          HXLDCont projcont = homMat2D.AffineTransContourXld(contours);
          msg.SetMessageObj(projcont, "contour");
        }
        else
        {
          sbm.Dispose();
          sbm = null;
        }
      }
      finally
      {
        Monitor.Exit(lockobj);
      }
      return msg;
    }

    ~ShapeModelDisplay()
    {
      imggrabber.Dispose();
    }
  }
}
