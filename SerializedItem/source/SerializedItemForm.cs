// HALCON/.NET (C#) serialize and deserialize example
//
// © 2012-2018 MVTec Software GmbH
//
// Purpose:
// This example program shows how to use the serialization of HALCON objects
// and tuples in c# interface.  First, in this example a shape based model
// is created, trained and write to a file.  Second, the shape based model
// is read from the file and other images are matched.

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using HalconDotNet;
using System.IO;

namespace SerializedItem
{
  /// <summary>
  /// Summary description for Form1.
  /// </summary>
  public class SerializedItemForm : System.Windows.Forms.Form
  {
    internal System.Windows.Forms.Label MatchingLabel;
    internal System.Windows.Forms.Button StartBtn;
    internal System.Windows.Forms.Button StopBtn;
    internal System.Windows.Forms.Button TrainBtn;
    internal System.Windows.Forms.Timer Timer;
    private System.Windows.Forms.Label CopyrightLabel;
    private System.ComponentModel.IContainer components;

    private HWindow Window;
    private HImage Img;
    private HShapeModel[] Models = new HShapeModel[4];
    private HTuple Names;
        private HSmartWindowControl WindowControl;
        private int ImgNum = 0;
    public SerializedItemForm()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();

      //
      // TODO: Add any constructor code after InitializeComponent call
      //
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (components != null)
        {
          components.Dispose();
        }
      }
      base.Dispose(disposing);
    }

    /// <summary>
    /// Translate mouse coordinates with respect to the top left corner of HSmartWindowControl.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void my_MouseWheel(object sender, MouseEventArgs e)
    {
        Point pt = WindowControl.Location;
        MouseEventArgs newe = new MouseEventArgs(e.Button, e.Clicks, e.X - pt.X, e.Y - pt.Y, e.Delta);
        WindowControl.HSmartWindowControl_MouseWheel(sender, newe);
    }

    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            this.MatchingLabel = new System.Windows.Forms.Label();
            this.StartBtn = new System.Windows.Forms.Button();
            this.StopBtn = new System.Windows.Forms.Button();
            this.TrainBtn = new System.Windows.Forms.Button();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.CopyrightLabel = new System.Windows.Forms.Label();
            this.WindowControl = new HalconDotNet.HSmartWindowControl();
            this.SuspendLayout();
            //
            // MatchingLabel
            //
            this.MatchingLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.MatchingLabel.Location = new System.Drawing.Point(16, 520);
            this.MatchingLabel.Name = "MatchingLabel";
            this.MatchingLabel.Size = new System.Drawing.Size(554, 16);
            this.MatchingLabel.TabIndex = 19;
            this.MatchingLabel.Text = "Ready to train shape model";
            //
            // StartBtn
            //
            this.StartBtn.Location = new System.Drawing.Point(684, 64);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(119, 32);
            this.StartBtn.TabIndex = 28;
            this.StartBtn.Text = "Start Matching";
            this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
            //
            // StopBtn
            //
            this.StopBtn.Location = new System.Drawing.Point(684, 112);
            this.StopBtn.Name = "StopBtn";
            this.StopBtn.Size = new System.Drawing.Size(119, 32);
            this.StopBtn.TabIndex = 27;
            this.StopBtn.Text = "Stop Matching";
            this.StopBtn.Click += new System.EventHandler(this.StopBtn_Click);
            //
            // TrainBtn
            //
            this.TrainBtn.Location = new System.Drawing.Point(684, 16);
            this.TrainBtn.Name = "TrainBtn";
            this.TrainBtn.Size = new System.Drawing.Size(119, 32);
            this.TrainBtn.TabIndex = 26;
            this.TrainBtn.Text = "Train Shape Model";
            this.TrainBtn.Click += new System.EventHandler(this.CreateBtn_Click);
            //
            // Timer
            //
            this.Timer.Interval = 1;
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            //
            // CopyrightLabel
            //
            this.CopyrightLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CopyrightLabel.Location = new System.Drawing.Point(16, 540);
            this.CopyrightLabel.Name = "CopyrightLabel";
            this.CopyrightLabel.Size = new System.Drawing.Size(787, 16);
            this.CopyrightLabel.TabIndex = 29;
            this.CopyrightLabel.Text = "© 2018 MVTec Software GmbH";
            this.CopyrightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // WindowControl
            //
            this.WindowControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.WindowControl.HMoveContent = true;
            this.WindowControl.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
            this.WindowControl.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.WindowControl.HImagePart = new System.Drawing.Rectangle(0, 0, 652, 494);
            this.WindowControl.Location = new System.Drawing.Point(12, 16);
            this.WindowControl.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.WindowControl.Name = "WindowControl";
            this.WindowControl.Size = new System.Drawing.Size(652, 494);
            this.WindowControl.TabIndex = 30;
            this.WindowControl.WindowSize = new System.Drawing.Size(652, 494);
            this.WindowControl.Load += new System.EventHandler(this.WindowControl_Load);
            //
            // SerializedItemForm
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(823, 562);
            this.Controls.Add(this.WindowControl);
            this.Controls.Add(this.CopyrightLabel);
            this.Controls.Add(this.StartBtn);
            this.Controls.Add(this.StopBtn);
            this.Controls.Add(this.TrainBtn);
            this.Controls.Add(this.MatchingLabel);
            this.Name = "SerializedItemForm";
            this.Text = "Serialize Item Demo";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

            this.MouseWheel += my_MouseWheel;
    }
    #endregion

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.Run(new SerializedItemForm());
    }

    private void Form1_Load(object sender, System.EventArgs e)
    {
      TrainBtn.Enabled = true;
      StartBtn.Enabled = false;
      StopBtn.Enabled = false;
      Timer.Enabled = false;
    }

        private void WindowControl_Load(object sender, EventArgs e)
        {
            Window = WindowControl.HalconWindow;

            Img = new HImage("coins/20cent_greek");
            Img.DispObj(Window);
        }

        // Training of the shape model and write the serialized item to a file
    // stream
    private void CreateBtn_Click(object sender, System.EventArgs e)
    {
      HTuple Names;
      HRegion Coin;
      HShapeModel Model;

      TrainBtn.Enabled = false;
      Window.SetColor("red");
      Window.SetDraw("margin");
      Window.SetLineWidth(1);

      Names = "german";
      Names.Append("italian");
      Names.Append("greek");
      Names.Append("spanish");

      //Write a serialized item of type tuple to a file
      Stream s = File.OpenWrite("serialized_shape_model");
      Names.Serialize(s);

      // Note: The binary layout of serialized HALCON objects is determined by
      // the native HALCON libary. You could also package this blob together with
      // other .NET objects into a formatted stream, e.g. by using
      //
      // BinaryFormatter f = new BinaryFormatter();
      // f.Serialize(s, Names);
      //
      // When using only HALCON objects without extra formatting, the streamed
      // data is interchangable with HDevelop or HALCON/C++.

      //Train shape models
      for (int i = 0; i < 4; i++)
      {
        MatchingLabel.Text = "Train coin " + (i + 1) + "/4 (" + Names[i] +
                   ") and serialize resulting shape model to a" +
                   " file...";
        MatchingLabel.Refresh();
        Img = new HImage("coins/20cent_" + Names[i]);

        Window.DispObj(Img);

        HRegion Region = Img.Threshold(70.0, 255.0);
        HRegion ConnectedRegions = Region.Connection();
        HRegion SelectedRegions = ConnectedRegions.SelectShapeStd("max_area", 0);
        HRegion RegionTrans = SelectedRegions.ShapeTrans("convex");
        Coin = new HRegion(RegionTrans.Row, RegionTrans.Column, 120);
        int Contrast = 20;
        HTuple HysteresisContrast;
        HysteresisContrast = (Contrast / 2.0);
        HysteresisContrast.Append(Contrast + 6.0);
        HysteresisContrast.Append(10.0);
        HImage ImgReduced = Img.ReduceDomain(Coin);
        //Called during the test phase to see if contrast is selected correctly
        Model = new HShapeModel(ImgReduced, new HTuple(0), 0.0,
          new HTuple(360.0).TupleRad(), new HTuple(0),
          new HTuple("no_pregeneration"), "ignore_local_polarity",
          HysteresisContrast, new HTuple(5));

        //Write a serialized item of type shape based model to a file
        Model.Serialize(s);

        // Destroy shape based model and objects
        Coin.Dispose();
        Model.Dispose();
        Img.Dispose();
        ImgReduced.Dispose();
        Region.Dispose();
        SelectedRegions.Dispose();
        RegionTrans.Dispose();
        ImgReduced.Dispose();

        HSystem.WaitSeconds(0.4);
      }

      s.Close();

      MatchingLabel.Text = "Ready to find coins";

      StopBtn.Enabled = false;
      StartBtn.Enabled = true;
    }

    private void StartBtn_Click(object sender, System.EventArgs e)
    {
      Timer.Enabled = true;
      TrainBtn.Enabled = false;
      StopBtn.Enabled = true;
      StartBtn.Enabled = false;
    }

    private void StopBtn_Click(object sender, System.EventArgs e)
    {
      Timer.Enabled = false;
      TrainBtn.Enabled = false;
      StopBtn.Enabled = false;
      StartBtn.Enabled = true;
    }

    private void Timer_Tick(object sender, System.EventArgs e)
    {
      Action();
      GC.Collect();
      GC.WaitForPendingFinalizers();
    }

    // Procedure to find the coins and to read the serialized item from a
    // file stream
    private void Action()
    {
      HRegion Coin, Circle;
      HTuple Row, Column, Angle, Score, ResultModel;

      if (ImgNum == 0)
      {
        MatchingLabel.Text = "Deserialize shape models...";
        MatchingLabel.Refresh();
        //Reading names
        Stream s = File.OpenRead("serialized_shape_model");
        Names = HTuple.Deserialize(s);
        //Reading shape models
        for (int i = 0; i < 4; i++)
          Models[i] = HShapeModel.Deserialize(s);
        s.Close();
        MatchingLabel.Text = "Deserialize shape models... OK";
        MatchingLabel.Refresh();
      }

      if (ImgNum == 13)
        ImgNum = 0;
      HTuple Zero;
      if (ImgNum + 1 < 10)
        Zero = "0";
      else
        Zero = "";

      //Find shape based model using the read models
      Img.ReadImage("coins/20cent_" + Zero + (ImgNum + 1).ToString() + ".png");
      ImgNum++;

      HRegion Region = Img.Threshold(70.0, 255.0);
      HRegion ConnectedRegions = Region.Connection();
      HRegion SelectedRegions = ConnectedRegions.SelectShapeStd("max_area", 0);
      HRegion RegionTrans = SelectedRegions.ShapeTrans("convex");
      Coin = new HRegion(RegionTrans.Row, RegionTrans.Column, 120);
      Circle = new HRegion(Coin.Row, Coin.Column, 35);
      HImage ImgReduced = Img.ReduceDomain(Circle);

      ImgReduced.FindShapeModels(Models, 0.0, new HTuple(360.0).TupleRad(),
                     0.6, 1, 0, "interpolation", 0, 0.9,
                     out Row, out Column, out Angle, out Score,
                     out ResultModel);

      HXLDCont ModelContours = Models[ResultModel.I].GetShapeModelContours(1);
      HHomMat2D HomMat2D = new HHomMat2D();

      Window.SetColor("green");

      HomMat2D.VectorAngleToRigid(0, 0, 0, Row, Column, Angle);
      HXLDCont ContoursAffineTrans = ModelContours.AffineTransContourXld(HomMat2D);

      HSystem.SetSystem("flush_graphic", "false");
      Window.DispObj(Img);
      HSystem.SetSystem("flush_graphic", "true");
      Window.DispObj(ContoursAffineTrans);

      MatchingLabel.Text = "#" + (ImgNum + 1) + ": Found: " +
                 Names[ResultModel.I] + " coin";
    }
  }
}
