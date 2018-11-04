using System;
using System.Collections.Generic;
using HalconDotNet;
using System.Windows.Forms;
using HALCONDrawingObjects;

namespace HALCONDrawingObjects
{
  /// <summary>
  /// This is the place where the user adds his/her code called from
  /// set_drawing_object_callback.
  /// </summary>
  class UserActions
  {
    private HALCONDialog halcon_dialog;

    public UserActions(HALCONDialog hd)
    {
      halcon_dialog = hd;
    }

    public void SobelFilter(HDrawingObject dobj, HWindow hwin,string type)
    {
      try
      {
        HImage image = halcon_dialog.BackgroundImage;
        HRegion region = new HRegion(dobj.GetDrawingObjectIconic());
        hwin.SetWindowParam("flush", "false");
        hwin.ClearWindow();
        hwin.DispObj(image.ReduceDomain(region).SobelAmp("sum_abs", 11));
        hwin.SetWindowParam("flush", "true");
        hwin.FlushBuffer();
      }
      catch (HalconException hex)
      {
        MessageBox.Show(hex.GetErrorMessage(), "HALCON error", MessageBoxButtons.OK);
      }
    }
  }
}
