/* This is a template C# project for using HALCON programs exported
 * from HDevelop for 'C# - HALCON/.NET' and the Template Window Export. You
 * need to add the generated source code to this project before compiling.
 */

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using HalconDotNet;

namespace HDevelopTemplate
{
  /// <summary>
  /// Summary description for Form1.
  /// </summary>
  public class HDevelopTemplate : System.Windows.Forms.Form
  {
    // The class HDevelopExport will be defined in the HALCON program
    // exported from HDevelop for 'C# - HALCON/.NET' and the Template
    // Window Export.
    private HDevelopExport HDevExp;
    private System.Windows.Forms.Button Run;
    private System.Windows.Forms.Label Status;
    private HalconDotNet.HWindowControl HWinCtrl;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;

    public HDevelopTemplate()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();

      //
      // TODO: Add any constructor code after InitializeComponent call
      //
      HDevExp = new HDevelopExport();
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
      this.HWinCtrl = new HalconDotNet.HWindowControl();
      this.Run = new System.Windows.Forms.Button();
      this.Status = new System.Windows.Forms.Label();
      this.SuspendLayout();
      //
      // HWinCtrl
      //
      this.HWinCtrl.BackColor = System.Drawing.Color.Black;
      this.HWinCtrl.BorderColor = System.Drawing.Color.Black;
      this.HWinCtrl.ImagePart = new System.Drawing.Rectangle(0, 0, 512, 512);
      this.HWinCtrl.Location = new System.Drawing.Point(8, 8);
      this.HWinCtrl.Name = "HWinCtrl";
      this.HWinCtrl.Size = new System.Drawing.Size(512, 512);
      this.HWinCtrl.TabIndex = 0;
      this.HWinCtrl.WindowSize = new System.Drawing.Size(512, 512);
      //
      // Run
      //
      this.Run.Location = new System.Drawing.Point(536, 8);
      this.Run.Name = "Run";
      this.Run.Size = new System.Drawing.Size(64, 24);
      this.Run.TabIndex = 1;
      this.Run.Text = "Run";
      this.Run.Click += new System.EventHandler(this.Run_Click);
      //
      // Status
      //
      this.Status.Location = new System.Drawing.Point(0, 528);
      this.Status.Name = "Status";
      this.Status.Size = new System.Drawing.Size(608, 16);
      this.Status.TabIndex = 2;
      //
      // HDevelopTemplate
      //
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(616, 553);
      this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                    this.Status,
                                                                    this.Run,
                                                                    this.HWinCtrl});
      this.Name = "HDevelopTemplate";
      this.Text = "HDevelop Template";
      this.Load += new System.EventHandler(this.Form1_Load);
      this.ResumeLayout(false);
    }
  #endregion Windows Form Designer generated code

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.Run(new HDevelopTemplate());
    }

    private void Form1_Load(object sender, System.EventArgs e)
    {
      HDevExp.InitHalcon();
    }

    private void RunExport()
    {
      HTuple WindowID = HWinCtrl.HalconID;
      HDevExp.RunHalcon(WindowID);

      this.Invoke((MethodInvoker) delegate {
        SetFinished();
        Run.Enabled = true;
      });
    }

    private void SetFinished()
    {
      Status.Text = "Finished.";
    }

    private void Run_Click(object sender, System.EventArgs e)
    {
      Run.Enabled = false;
      Status.Text = "Running...";
      Status.Refresh();

      Thread exportThread = new Thread(new ThreadStart(this.RunExport));
      exportThread.Start();
    }
  }
}
