//
// HALCON/.NET (C#) multithreading example
//
// © 2007-2018 MVTec Software GmbH
//
// Purpose:
// This example program shows how to perform image acquisition, image
// processing, and image display in parallel by using two threads (besides
// the main thread), one for each task. The first thread grabs images, the
// second one performs image processing tasks, and the main thread is in
// charge of the HALCON window - it displays the image processed last and
// its results.
//
// MultiThreadingForm.cs: Defines the behavior of the application's GUI.
//

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;

using HalconDotNet;

namespace MultiThreading
{
  /// <summary>
  /// Summary description for MultiThreadExpDlg.
  /// </summary>

  public class MultiThreadingForm : System.Windows.Forms.Form
  {
    private System.Windows.Forms.Button startButton;
    private System.Windows.Forms.Button stopButton;
    public  System.Windows.Forms.Label procTimeLabel;
    public  System.Windows.Forms.Label imageDataLabel;
    private System.ComponentModel.IContainer components;
    public  WorkerThread workerObject;

    public ManualResetEvent stopEventHandle;
    public Thread threadAcq, threadIP;
        private System.Windows.Forms.Label LabelPT;
    private System.Windows.Forms.Label CopyrightLabel;
        private HSmartWindowControl WindowControl;
    private System.Windows.Forms.Label LabelID;



    public MultiThreadingForm()
    {

      // Required for Windows Form Designer support
      InitializeComponent();

      // set up eventhandle and instance of WorkerThread class, which
      // contains thread functions
      stopEventHandle   = new ManualResetEvent(false);
      workerObject = new WorkerThread(this);
    }

    protected override void OnClosed(EventArgs e)
    {
      // if threads are still running
      // abort them
      if (threadAcq != null) threadAcq.Abort();
      if (threadIP != null) threadIP.Abort();
      base.OnClosed(e);
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
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.LabelPT = new System.Windows.Forms.Label();
            this.LabelID = new System.Windows.Forms.Label();
            this.procTimeLabel = new System.Windows.Forms.Label();
            this.imageDataLabel = new System.Windows.Forms.Label();
            this.CopyrightLabel = new System.Windows.Forms.Label();
            this.WindowControl = new HalconDotNet.HSmartWindowControl();
            this.SuspendLayout();
            //
            // startButton
            //
            this.startButton.Location = new System.Drawing.Point(376, 32);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(80, 40);
            this.startButton.TabIndex = 1;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            //
            // stopButton
            //
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(376, 96);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(80, 40);
            this.stopButton.TabIndex = 2;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            //
            // LabelPT
            //
            this.LabelPT.Location = new System.Drawing.Point(16, 272);
            this.LabelPT.Name = "LabelPT";
            this.LabelPT.Size = new System.Drawing.Size(112, 24);
            this.LabelPT.TabIndex = 3;
            this.LabelPT.Text = "Processing time:";
            this.LabelPT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // LabelID
            //
            this.LabelID.Location = new System.Drawing.Point(16, 304);
            this.LabelID.Name = "LabelID";
            this.LabelID.Size = new System.Drawing.Size(104, 24);
            this.LabelID.TabIndex = 4;
            this.LabelID.Text = "Data code content:";
            this.LabelID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // procTimeLabel
            //
            this.procTimeLabel.Location = new System.Drawing.Point(136, 272);
            this.procTimeLabel.Name = "procTimeLabel";
            this.procTimeLabel.Size = new System.Drawing.Size(72, 24);
            this.procTimeLabel.TabIndex = 5;
            this.procTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // imageDataLabel
            //
            this.imageDataLabel.Location = new System.Drawing.Point(136, 304);
            this.imageDataLabel.Name = "imageDataLabel";
            this.imageDataLabel.Size = new System.Drawing.Size(340, 24);
            this.imageDataLabel.TabIndex = 6;
            this.imageDataLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // CopyrightLabel
            //
            this.CopyrightLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CopyrightLabel.Location = new System.Drawing.Point(16, 336);
            this.CopyrightLabel.Name = "CopyrightLabel";
            this.CopyrightLabel.Size = new System.Drawing.Size(460, 16);
            this.CopyrightLabel.TabIndex = 30;
            this.CopyrightLabel.Text = "© 2007-2018 MVTec Software GmbH";
            this.CopyrightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // WindowControl
            //
            this.WindowControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.WindowControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.WindowControl.HMoveContent = true;
            this.WindowControl.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
            this.WindowControl.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.WindowControl.HImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.WindowControl.Location = new System.Drawing.Point(16, 16);
            this.WindowControl.Margin = new System.Windows.Forms.Padding(2);
            this.WindowControl.Name = "WindowControl";
            this.WindowControl.Size = new System.Drawing.Size(320, 240);
            this.WindowControl.TabIndex = 31;
            this.WindowControl.WindowSize = new System.Drawing.Size(320, 240);
            //
            // MultiThreadingForm
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(496, 357);
            this.Controls.Add(this.WindowControl);
            this.Controls.Add(this.CopyrightLabel);
            this.Controls.Add(this.imageDataLabel);
            this.Controls.Add(this.procTimeLabel);
            this.Controls.Add(this.LabelID);
            this.Controls.Add(this.LabelPT);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "MultiThreadingForm";
            this.Text = "Performing Image Acquisition, Processing, and Display in Multiple Threads";
            this.ResumeLayout(false);

    }
    #endregion

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.Run(new MultiThreadingForm());
    }


    ////////////////////////////////////////////////////////////////////////////
    // startButton_Click - Every click on the Start button will create new
    //                     instances of the Thread class - threadFG handles the
    //                     image acquisition, whereas threadIP performs the image
    //                     processing. Since Start and Stop button can be
    //                     clicked consecutively, all handles used to synchronize
    //                     the threads need to be set/reset to their initial
    //                     states. Handles that are members of the WorkerThread
    //                     class are reset by calling the init() method. Last but
    //                     not least we need to call Thread.Start() to "start"
    //                     the thread functions.
    ////////////////////////////////////////////////////////////////////////////
    private void startButton_Click(object sender, System.EventArgs e)
    {
      stopEventHandle.Reset();

      threadAcq = new Thread(new ThreadStart(workerObject.ImgAcqRun));
      threadIP = new Thread(new ThreadStart(workerObject.IPRun));

      startButton.Enabled = false;
      stopButton.Enabled = true;

      workerObject.Init();

      threadAcq.Start();
      threadIP.Start();
    }

    ////////////////////////////////////////////////////////////////////////////
    // stopButton_Click - Once the Stop button is clicked, the stopEventHandle is
    //                    "turned on" - to signal the two threads to terminate.
    ////////////////////////////////////////////////////////////////////////////
    private void stopButton_Click(object sender, System.EventArgs e)
    {
      stopEventHandle.Set();
    }

    ////////////////////////////////////////////////////////////////////////////
    public HWindow GetHalconWindow()
    {
      return WindowControl.HalconWindow;
    }

    ////////////////////////////////////////////////////////////////////////////
    // ResetControls - We used the means of delegates to prevent deadlocks
    //                 between the main thread and the IPthread. The delegate is
    //                 called to "clean" the display and the labels.
    ////////////////////////////////////////////////////////////////////////////
    public void ResetControls()
    {
      startButton.Enabled = true;
      stopButton.Enabled  = false;
      GetHalconWindow().ClearWindow();
      procTimeLabel.Text = " ";
      imageDataLabel.Text = " ";
      components = null;
    }


  } // end of class

} // end of using namespace



