using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Interop;
using System.Windows.Threading;

using HalconDotNet;

namespace MatchingWPF
{
  /// <summary>
  /// Interaction logic for MatchingWindow.xaml
  /// </summary>
  public partial class MatchingWindow : Window
  {
    HWindow Window = null;

    private HFramegrabber Framegrabber;
    private HImage Img;
    private int ImgWidth, ImgHeight;
    private HRegion Rectangle, ModelRegion;
    private double Row, Column;
    private double Rect1Row, Rect1Col, Rect2Row, Rect2Col;
    private double RectPhi, RectLength1, RectLength2;
    private HShapeModel ShapeModel;

    DispatcherTimer Timer;

    public MatchingWindow()
    {
      InitializeComponent();
    }

    private void hWindowControlWPF1_HInitWindow(object sender, EventArgs e)
    {
      // Get HALCON window from control. Due to restrictions of WPF
      // interoperating with native Windows, this is not yet available
      // in the Loaded event of the control or the parent window. The
      // best time to extract the HALCON window is in this event.
      Window = hWindowControlWPF1.HalconWindow;

      // Attention: Technically we are called by the control during its
      // initialization. Under now account should an exception be allowed
      // to propagate upwards from here (under Windows 7, not even the
      // Visual Studio debugger will function properly in this case).

      try
      {
        // Initialize enabled states
        CreateBtn.IsEnabled= true;
        StartBtn.IsEnabled = false;
        StopBtn.IsEnabled = false;

        // Create a timer for execution loop;
        Timer = new DispatcherTimer();
        Timer.Interval = new TimeSpan(10);
        Timer.Tick += new EventHandler(Timer_Tick);

        // Prepare image processing
        string ImgType;
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
      }
      catch (Exception ex)
      {
        // Catch all
        MessageBox.Show("Error in HInitWindow:" + ex.ToString());
      }
    }

    private void CreateBtn_Click(object sender, RoutedEventArgs e)
    {
      HImage ImgReduced;
      HRegion Rectangle1 = new HRegion();
      HRegion Rectangle2 = new HRegion();

      CreateBtn.IsEnabled = false;
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

      StopBtn.IsEnabled = false;
      StartBtn.IsEnabled = true;
    }

    private void StartBtn_Click(object sender, RoutedEventArgs e)
    {
      Timer.Start();
      CreateBtn.IsEnabled = false;
      StopBtn.IsEnabled = true;
      StartBtn.IsEnabled = false;
    }


    private void StopBtn_Click(object sender, RoutedEventArgs e)
    {
      Timer.Stop();
      CreateBtn.IsEnabled = false;
      StopBtn.IsEnabled = false;
      StartBtn.IsEnabled = true;
    }

    private void Timer_Tick(object sender, System.EventArgs e)
    {
      Action();
    }

    private void Action()
    {
      double        S1, S2;
      HTuple        RowCheck, ColumnCheck, AngleCheck, Score;
      HHomMat2D     Matrix = new HHomMat2D();
      HRegion       ModelRegionTrans;
      HTuple        Rect1RowCheck, Rect1ColCheck;
      HTuple        Rect2RowCheck, Rect2ColCheck;
      HRegion       Rectangle1 = new HRegion();
      HRegion       Rectangle2 = new HRegion();
      HMeasure      Measure1, Measure2;
      HTuple        RowEdgeFirst1, ColumnEdgeFirst1;
      HTuple        AmplitudeFirst1, RowEdgeSecond1;
      HTuple        ColumnEdgeSecond1, AmplitudeSecond1;
      HTuple        IntraDistance1, InterDistance1;
      HTuple        RowEdgeFirst2, ColumnEdgeFirst2;
      HTuple        AmplitudeFirst2, RowEdgeSecond2;
      HTuple        ColumnEdgeSecond2, AmplitudeSecond2;
      HTuple        IntraDistance2, InterDistance2;
      HTuple        MinDistance;
      int         NumLeads;

      Img.Dispose();
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
      MatchingTimeLabel.Content = "Time: " +
        String.Format("{0,4:F1}", (S2 - S1)*1000) + "ms";
      MatchingScoreLabel.Content = "Score: ";

      if (RowCheck.Length == 1)
      {
        MatchingScoreLabel.Content = "Score: " +
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
        Matrix.AffineTransPixel(Rect2Row, Rect2Col, out Rect2RowCheck,
                    out Rect2ColCheck);

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
        MeasureTimeLabel.Content = "Time: " +
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
        MeasureNumLabel.Content = "Number of leads: " +
                      String.Format("{0:D2}", NumLeads);
        MinDistance = InterDistance1.TupleConcat(InterDistance2).TupleMin();
        MeasureDistLabel.Content = "Minimum lead distance: " +
          String.Format("{0:F3}", MinDistance.D);
        Window.FlushBuffer();
        Measure1.Dispose();
        Measure2.Dispose();
      }
    }

  }
}
