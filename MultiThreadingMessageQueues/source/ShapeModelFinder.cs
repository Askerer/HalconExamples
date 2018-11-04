using System;
using System.Collections.Generic;
using HALCONActors;
using HalconDotNet;
using System.Threading;

namespace HALCONDrawingObjects
{
  /// <summary>
  /// The ShapeModelFinder is responsible for searching the shape model in the
  /// image and passing on the results, if any model was found in the image.
  ///
  /// Notice that the corresponding thread is blocked waiting for new input.
  /// The input is provided by the callback delegate, i.e., the user interactions
  /// with the drawing object trigger the thread to perform the operation.
  /// </summary>
  public class ShapeModelFinder : HActor
  {
    private HShapeModel sbm;
    private object lockobj = new object();

    public ShapeModelFinder(IHActor next) : base(next,true)
    {
      sbm = null;
    }

    public ShapeModelFinder(HShapeModel model, IHActor next)
      : base(next, true)
    {
      if (model == null)
        throw new ArgumentNullException();
      sbm = model;
    }

    public HShapeModel ShapeModel
    {
      get
      {
        HShapeModel model;
        lock (lockobj)
          model = sbm;
        return model;
      }
      set
      {
        if (value == null)
          throw new ArgumentNullException();
        lock (lockobj)
          sbm = value;
      }
    }

    public override HMessage ActorTask(HMessage imsg)
    {
      HTuple row, column, angle, score;
      HImage img = new HImage(imsg.GetMessageObj("image"));
      if (sbm == null)
        return GenFinishMessage();
      HMessage msg = new HMessage();
      msg.SetMessageObj(img, "image");
      Monitor.Enter(lockobj);
      try
      {
        sbm.FindShapeModel(img, -Math.PI, 2*Math.PI, 0.5, 1, 0.5,
          "least_squares", 0, 0.9,out row, out column, out angle, out score);
        if (row.Length > 0)
        {
          HXLDCont contours = sbm.GetShapeModelContours(1);
          HHomMat2D homMat2D = new HHomMat2D();
          homMat2D.VectorAngleToRigid(0.0, 0.0, 0.0, row.D, column.D, angle.D);
          HXLDCont projcont = homMat2D.AffineTransContourXld(contours);
          msg.SetMessageObj(projcont, "contour");
        }
      }
      finally
      {
        Monitor.Exit(lockobj);
      }
      imsg.Dispose();
      return msg;
    }
  }
}
