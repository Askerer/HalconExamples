// HALCON/.NET (C#) pattern matching and measure example
//
// ?2000-2018 MVTec Software GmbH
//
// Purpose:
// This example program shows the use of pattern matching with shape models
// to locate an object.  Furthermore, it shows how to use the detected position
// and rotation of the object to construct search spaces for inspection tasks.
// In this particular example, the print on an IC is used to find the IC.  From
// the found position and rotation, two measurement rectangles are constructed
// to measure the spacing between the leads of the IC.  Because of the lighting
// used in this example, the leads have the saturated gray value of 255 at
// several positions and rotations, which enlarges the apparent width of the
// leads, and hence seems to reduce the spacing between the leads, although the
// same board is used in all images.

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using HalconDotNet;

namespace Matching
{
  /// <summary>
  /// Summary description for Form1.
  /// </summary>
  public class MatchingForm : System.Windows.Forms.Form
  {
    internal System.Windows.Forms.Label MeasureDistLabel;
    internal System.Windows.Forms.Label MeasureNumLabel;
    internal System.Windows.Forms.Label MeasureTimeLabel;
    internal System.Windows.Forms.Label MeasureLabel;
    internal System.Windows.Forms.Label MatchingScoreLabel;
    internal System.Windows.Forms.Label MatchingTimeLabel;
    internal System.Windows.Forms.Label MatchingLabel;
    internal System.Windows.Forms.Button StartBtn;
    internal System.Windows.Forms.Button StopBtn;
    internal System.Windows.Forms.Button CreateBtn;
    internal System.Windows.Forms.Timer  Timer;
    private System.Windows.Forms.Label CopyrightLabel;
    private System.ComponentModel.IContainer components;

    private HWindow                       Window;
    private HFramegrabber                 Framegrabber;
    private HImage                        Img;
    private int                           ImgWidth, ImgHeight;
    private HRegion                       Rectangle, ModelRegion;
    private double                        Row, Column;
    private double                        Rect1Row, Rect1Col, Rect2Row, Rect2Col;
    private double                        RectPhi, RectLength1, RectLength2;
    private HSmartWindowControl           WindowControl;
    private HShapeModel                   ShapeModel;

    public MatchingForm()
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
    protected override void Dispose( bool disposing )
    {
      if( disposing )
      {
        if (components != null)
        {
          components.Dispose();
        }
      }
      base.Dispose( disposing );
    }

  #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.MeasureDistLabel = new System.Windows.Forms.Label();
      this.MeasureNumLabel = new System.Windows.Forms.Label();
      this.MeasureTimeLabel = new System.Windows.Forms.Label();
      this.MeasureLabel = new System.Windows.Forms.Label();
      this.MatchingScoreLabel = new System.Windows.Forms.Label();
      this.MatchingTimeLabel = new System.Windows.Forms.Label();
      this.MatchingLabel = new System.Windows.Forms.Label();
      this.StartBtn = new System.Windows.Forms.Button();
      this.StopBtn = new System.Windows.Forms.Button();
      this.CreateBtn = new System.Windows.Forms.Button();
      this.Timer = new System.Windows.Forms.Timer(this.components);
      this.CopyrightLabel = new System.Windows.Forms.Label();
      this.WindowControl = new HalconDotNet.HSmartWindowControl();
      this.SuspendLayout();
      //
      // MeasureDistLabel
      //
      this.MeasureDistLabel.Location = new System.Drawing.Point(272, 544);
      this.MeasureDistLabel.Name = "MeasureDistLabel";
      this.MeasureDistLabel.Size = new System.Drawing.Size(176, 16);
      this.MeasureDistLabel.TabIndex = 25;
      this.MeasureDistLabel.Text = "Minimum lead distance:";
      this.MeasureDistLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      //
      // MeasureNumLabel
      //
      this.MeasureNumLabel.Location = new System.Drawing.Point(152, 544);
      this.MeasureNumLabel.Name = "MeasureNumLabel";
      this.MeasureNumLabel.Size = new System.Drawing.Size(120, 16);
      this.MeasureNumLabel.TabIndex = 24;
      this.MeasureNumLabel.Text = "Number of leads:";
      this.MeasureNumLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      //
      // MeasureTimeLabel
      //
      this.MeasureTimeLabel.Location = new System.Drawing.Point(72, 544);
      this.MeasureTimeLabel.Name = "MeasureTimeLabel";
      this.MeasureTimeLabel.Size = new System.Drawing.Size(80, 16);
      this.MeasureTimeLabel.TabIndex = 23;
      this.MeasureTimeLabel.Text = "Time:";
      this.MeasureTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      //
      // MeasureLabel
      //
      this.MeasureLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.MeasureLabel.Location = new System.Drawing.Point(16, 544);
      this.MeasureLabel.Name = "MeasureLabel";
      this.MeasureLabel.Size = new System.Drawing.Size(56, 16);
      this.MeasureLabel.TabIndex = 22;
      this.MeasureLabel.Text = "Measure:";
      //
      // MatchingScoreLabel
      //
      this.MatchingScoreLabel.Location = new System.Drawing.Point(152, 520);
      this.MatchingScoreLabel.Name = "MatchingScoreLabel";
      this.MatchingScoreLabel.Size = new System.Drawing.Size(96, 16);
      this.MatchingScoreLabel.TabIndex = 21;
      this.MatchingScoreLabel.Text = "Score:";
      this.MatchingScoreLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      //
      // MatchingTimeLabel
      //
      this.MatchingTimeLabel.Location = new System.Drawing.Point(72, 520);
      this.MatchingTimeLabel.Name = "MatchingTimeLabel";
      this.MatchingTimeLabel.Size = new System.Drawing.Size(80, 16);
      this.MatchingTimeLabel.TabIndex = 20;
      this.MatchingTimeLabel.Text = "Time:";
      this.MatchingTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      //
      // MatchingLabel
      //
      this.MatchingLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.MatchingLabel.Location = new System.Drawing.Point(16, 520);
      this.MatchingLabel.Name = "MatchingLabel";
      this.MatchingLabel.Size = new System.Drawing.Size(56, 16);
      this.MatchingLabel.TabIndex = 19;
      this.MatchingLabel.Text = "Matching:";
      //
      // StartBtn
      //
      this.StartBtn.Location = new System.Drawing.Point(680, 64);
      this.StartBtn.Name = "StartBtn";
      this.StartBtn.Size = new System.Drawing.Size(80, 32);
      this.StartBtn.TabIndex = 28;
      this.StartBtn.Text = "Start";
      this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
      //
      // StopBtn
      //
      this.StopBtn.Location = new System.Drawing.Point(680, 112);
      this.StopBtn.Name = "StopBtn";
      this.StopBtn.Size = new System.Drawing.Size(80, 32);
      this.StopBtn.TabIndex = 27;
      this.StopBtn.Text = "Stop";
      this.StopBtn.Click += new System.EventHandler(this.StopBtn_Click);
      //
      // CreateBtn
      //
      this.CreateBtn.Location = new System.Drawing.Point(680, 16);
      this.CreateBtn.Name = "CreateBtn";
      this.CreateBtn.Size = new System.Drawing.Size(80, 32);
      this.CreateBtn.TabIndex = 26;
      this.CreateBtn.Text = "Create Model";
      this.CreateBtn.Click += new System.EventHandler(this.CreateBtn_Click);
      //
      // Timer
      //
      this.Timer.Interval = 1;
      this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
      //
      // CopyrightLabel
      //
      this.CopyrightLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.CopyrightLabel.Location = new System.Drawing.Point(16, 576);
      this.CopyrightLabel.Name = "CopyrightLabel";
      this.CopyrightLabel.Size = new System.Drawing.Size(744, 16);
      this.CopyrightLabel.TabIndex = 29;
      this.CopyrightLabel.Text = "?2000-2018 MVTec Software GmbH";
      this.CopyrightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      //
      // WindowControl
      //
      this.WindowControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.WindowControl.HMoveContent = true;
      this.WindowControl.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
      this.WindowControl.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
      this.WindowControl.Location = new System.Drawing.Point(16, 16);
      this.WindowControl.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.WindowControl.Name = "WindowControl";
      this.WindowControl.Size = new System.Drawing.Size(646, 492);
      this.WindowControl.TabIndex = 30;
      this.WindowControl.Load += new System.EventHandler(this.WindowControl_Load);
      //
      // MatchingForm
      //
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(778, 597);
      this.Controls.Add(this.WindowControl);
      this.Controls.Add(this.CopyrightLabel);
      this.Controls.Add(this.StartBtn);
      this.Controls.Add(this.StopBtn);
      this.Controls.Add(this.CreateBtn);
      this.Controls.Add(this.MeasureDistLabel);
      this.Controls.Add(this.MeasureNumLabel);
      this.Controls.Add(this.MeasureTimeLabel);
      this.Controls.Add(this.MeasureLabel);
      this.Controls.Add(this.MatchingScoreLabel);
      this.Controls.Add(this.MatchingTimeLabel);
      this.Controls.Add(this.MatchingLabel);
      this.Name = "MatchingForm";
      this.Text = "Matching and Measurement Demo";
      this.Load += new System.EventHandler(this.Form1_Load);
      this.ResumeLayout(false);

    }
  #endregion

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.Run(new MatchingForm());
    }

    private void Form1_Load(object sender, System.EventArgs e)
    {
      CreateBtn.Enabled = true;
      StartBtn.Enabled = false;
      StopBtn.Enabled = false;
      Timer.Enabled = false;
    }

    private void WindowControl_Load(object sender, EventArgs e)
    {
      string ImgType;

      Window = WindowControl.HalconWindow;
      Framegrabber = new HFramegrabber("File", 1, 1, 0, 0, 0, 0, "default",
        -1, "default", -1, "default",
        "board/board.seq", "default", 1, -1);
      Img = Framegrabber.GrabImage();
      Img.GetImagePointer1(out ImgType, out ImgWidth, out ImgHeight);
      Window.SetPart(0, 0, ImgHeight - 1, ImgWidth - 1);
      Img.DispObj(Window);
      Window.SetDraw("margin");
      Window.SetLineWidth(3);
      Rectangle = new HRegion(188.0, 182, 298, 412);
      Rectangle.AreaCenter(out Row, out Column);
      Rect1Row = Row - 102;
      Rect1Col = Column + 5;
      Rect2Row = Row + 107;
      Rect2Col = Column + 5;
      RectPhi = 0;
      RectLength1 = 170;
      RectLength2 = 5;
      this.MouseWheel += my_MouseWheel;
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

    private void CreateBtn_Click(object sender, System.EventArgs e)
    {
      HImage  ImgReduced;
      HRegion Rectangle1 = new HRegion();
      HRegion Rectangle2 = new HRegion();

      CreateBtn.Enabled = false;
      Window.SetColor("red");
      Window.SetDraw("margin");
      Window.SetLineWidth(3);

      ImgReduced = Img.ReduceDomain(Rectangle);
      ImgReduced.InspectShapeModel(out ModelRegion, 1, 30);
      Rectangle1.GenRectangle2(Rect1Row, Rect1Col, RectPhi, RectLength1, RectLength2);
      Rectangle2.GenRectangle2(Rect2Row, Rect2Col, RectPhi, RectLength1, RectLength2);
      ShapeModel = new HShapeModel(ImgReduced, 4, 0, new HTuple(360.0).TupleRad().D,
        new HTuple(1.0).TupleRad().D, "none", "use_polarity", 30, 10);

      Window.SetColor("green");
      Window.SetDraw("fill");
      ModelRegion.DispObj(Window);
      Window.SetColor("blue");
      Window.SetDraw("margin");
      Rectangle1.DispObj(Window);
      Rectangle2.DispObj(Window);

      StopBtn.Enabled = false;
      StartBtn.Enabled = true;
    }

    private void StartBtn_Click(object sender, System.EventArgs e)
    {
      Timer.Enabled = true;
      CreateBtn.Enabled = false;
      StopBtn.Enabled = true;
      StartBtn.Enabled = false;
    }

    private void StopBtn_Click(object sender, System.EventArgs e)
    {
      Timer.Enabled = false;
      CreateBtn.Enabled = false;
      StopBtn.Enabled = false;
      StartBtn.Enabled = true;
    }

    private void Timer_Tick(object sender, System.EventArgs e)
    {
      Action();
      GC.Collect();
      GC.WaitForPendingFinalizers();
    }

    private void Action()
    {
      double            S1, S2;
      HTuple            RowCheck, ColumnCheck, AngleCheck, Score;
      HHomMat2D         Matrix = new HHomMat2D();
      HRegion           ModelRegionTrans;
      HTuple            Rect1RowCheck, Rect1ColCheck;
      HTuple            Rect2RowCheck, Rect2ColCheck;
      HRegion           Rectangle1 = new HRegion();
      HRegion           Rectangle2 = new HRegion();
      HMeasure          Measure1, Measure2;
      HTuple            RowEdgeFirst1, ColumnEdgeFirst1;
      HTuple            AmplitudeFirst1, RowEdgeSecond1;
      HTuple            ColumnEdgeSecond1, AmplitudeSecond1;
      HTuple            IntraDistance1, InterDistance1;
      HTuple            RowEdgeFirst2, ColumnEdgeFirst2;
      HTuple            AmplitudeFirst2, RowEdgeSecond2;
      HTuple            ColumnEdgeSecond2, AmplitudeSecond2;
      HTuple            IntraDistance2, InterDistance2;
      HTuple            MinDistance;
      int               NumLeads;

      HSystem.SetSystem("flush_graphic", "false");
      Img.GrabImage(Framegrabber);
      Img.DispObj(Window);

      // Find the IC in the current image.
      S1 = HSystem.CountSeconds();
      ShapeModel.FindShapeModel(Img, 0,
        new HTuple(360).TupleRad().D,
        0.7, 1, 0.5, "least_squares",
        4, 0.9, out RowCheck, out ColumnCheck,
        out AngleCheck, out Score);
      S2 = HSystem.CountSeconds();
      MatchingTimeLabel.Text = "Time: " +
        String.Format("{0,4:F1}", (S2 - S1)*1000) + "ms";
      MatchingScoreLabel.Text = "Score: ";

      if (RowCheck.Length == 1)
      {
        MatchingScoreLabel.Text = "Score: " +
          String.Format("{0:F5}", Score.D);
        // Rotate the model for visualization purposes.
        Matrix.VectorAngleToRigid(new HTuple(Row), new HTuple(Column), new HTuple(0.0),
          RowCheck, ColumnCheck, AngleCheck);

        ModelRegionTrans = ModelRegion.AffineTransRegion(Matrix, "false");
        Window.SetColor("green");
        Window.SetDraw("fill");
        ModelRegionTrans.DispObj(Window);
        // Compute the parameters of the measurement rectangles.
        Matrix.AffineTransPixel(Rect1Row, Rect1Col,
          out Rect1RowCheck, out Rect1ColCheck);
        Matrix.AffineTransPixel(Rect2Row, Rect2Col,
          out Rect2RowCheck, out Rect2ColCheck);

        // For visualization purposes, generate the two rectangles as
        // regions and display them.
        Rectangle1.GenRectangle2(Rect1RowCheck.D, Rect1ColCheck.D,
          RectPhi + AngleCheck.D,
          RectLength1, RectLength2);
        Rectangle2.GenRectangle2(Rect2RowCheck.D, Rect2ColCheck.D,
          RectPhi + AngleCheck.D,
          RectLength1, RectLength2);
        Window.SetColor("blue");
        Window.SetDraw("margin");
        Rectangle1.DispObj(Window);
        Rectangle2.DispObj(Window);
        // Do the actual measurements.
        S1 = HSystem.CountSeconds();
        Measure1 = new HMeasure(Rect1RowCheck.D, Rect1ColCheck.D,
          RectPhi + AngleCheck.D,
          RectLength1, RectLength2,
          ImgWidth, ImgHeight, "bilinear");
        Measure2 = new HMeasure(Rect2RowCheck.D, Rect2ColCheck.D,
          RectPhi + AngleCheck.D,
          RectLength1, RectLength2,
          ImgWidth, ImgHeight, "bilinear");
        Measure1.MeasurePairs(Img, 2, 90,
          "positive", "all",
          out RowEdgeFirst1,
          out ColumnEdgeFirst1,
          out AmplitudeFirst1,
          out RowEdgeSecond1,
          out ColumnEdgeSecond1,
          out AmplitudeSecond1,
          out IntraDistance1,
          out InterDistance1);
        Measure2.MeasurePairs(Img, 2, 90,
          "positive", "all",
          out RowEdgeFirst2,
          out ColumnEdgeFirst2,
          out AmplitudeFirst2,
          out RowEdgeSecond2,
          out ColumnEdgeSecond2,
          out AmplitudeSecond2,
          out IntraDistance2,
          out InterDistance2);
        S2 = HSystem.CountSeconds();
        MeasureTimeLabel.Text = "Time: " +
          String.Format("{0,5:F1}", (S2 - S1)*1000) + "ms";
        Window.SetColor("red");
        Window.DispLine(RowEdgeFirst1 - RectLength2*Math.Cos(AngleCheck),
          ColumnEdgeFirst1 - RectLength2*Math.Sin(AngleCheck),
          RowEdgeFirst1 + RectLength2*Math.Cos(AngleCheck),
          ColumnEdgeFirst1 + RectLength2*Math.Sin(AngleCheck));
        Window.DispLine(RowEdgeSecond1 - RectLength2*Math.Cos(AngleCheck),
          ColumnEdgeSecond1 - RectLength2*Math.Sin(AngleCheck),
          RowEdgeSecond1 + RectLength2*Math.Cos(AngleCheck),
          ColumnEdgeSecond1 + RectLength2*Math.Sin(AngleCheck));
        Window.DispLine(RowEdgeFirst2 - RectLength2*Math.Cos(AngleCheck),
          ColumnEdgeFirst2 - RectLength2*Math.Sin(AngleCheck),
          RowEdgeFirst2 + RectLength2*Math.Cos(AngleCheck),
          ColumnEdgeFirst2 + RectLength2*Math.Sin(AngleCheck));
        Window.DispLine(RowEdgeSecond2 - RectLength2*Math.Cos(AngleCheck),
          ColumnEdgeSecond2 - RectLength2*Math.Sin(AngleCheck),
          RowEdgeSecond2 + RectLength2*Math.Cos(AngleCheck),
          ColumnEdgeSecond2 + RectLength2*Math.Sin(AngleCheck));
        NumLeads = IntraDistance1.Length + IntraDistance2.Length;
        MeasureNumLabel.Text = "Number of leads: " +
          String.Format("{0:D2}", NumLeads);
        MinDistance = InterDistance1.TupleConcat(InterDistance2).TupleMin();
        MeasureDistLabel.Text = "Minimum lead distance: " +
          String.Format("{0:F3}", MinDistance.D);
        HSystem.SetSystem("flush_graphic", "true");
        // Force the graphics window update by displaying an offscreen pixel
        Window.DispLine(-1.0, -1.0, -1.0, -1.0);
      }
    }
  }
}
