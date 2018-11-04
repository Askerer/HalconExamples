namespace HALCONDrawingObjects
{
  partial class HALCONDialog
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.addNewObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.rectangle1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.rectangle2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.circleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.ellipseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.setColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.redToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.yellowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.greenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.blueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.setLineStyleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.dashedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.continuousToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.clearAllObjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.label1 = new System.Windows.Forms.Label();
      this.hsmartControl = new HalconDotNet.HSmartWindowControl();
      this.contextMenuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // contextMenuStrip1
      // 
      this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
      this.addNewObjectToolStripMenuItem,
      this.setColorToolStripMenuItem,
      this.setLineStyleToolStripMenuItem,
      this.clearAllObjectsToolStripMenuItem});
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new System.Drawing.Size(158, 92);
      // 
      // addNewObjectToolStripMenuItem
      // 
      this.addNewObjectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
      this.rectangle1ToolStripMenuItem,
      this.rectangle2ToolStripMenuItem,
      this.circleToolStripMenuItem,
      this.ellipseToolStripMenuItem});
      this.addNewObjectToolStripMenuItem.Name = "addNewObjectToolStripMenuItem";
      this.addNewObjectToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
      this.addNewObjectToolStripMenuItem.Text = "Add new object";
      // 
      // rectangle1ToolStripMenuItem
      // 
      this.rectangle1ToolStripMenuItem.Name = "rectangle1ToolStripMenuItem";
      this.rectangle1ToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
      this.rectangle1ToolStripMenuItem.Text = "Rectangle1";
      this.rectangle1ToolStripMenuItem.Click += new System.EventHandler(this.rectangle1ToolStripMenuItem_Click);
      // 
      // rectangle2ToolStripMenuItem
      // 
      this.rectangle2ToolStripMenuItem.Name = "rectangle2ToolStripMenuItem";
      this.rectangle2ToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
      this.rectangle2ToolStripMenuItem.Text = "Rectangle2";
      this.rectangle2ToolStripMenuItem.Click += new System.EventHandler(this.rectangle2ToolStripMenuItem_Click);
      // 
      // circleToolStripMenuItem
      // 
      this.circleToolStripMenuItem.Name = "circleToolStripMenuItem";
      this.circleToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
      this.circleToolStripMenuItem.Text = "Circle";
      this.circleToolStripMenuItem.Click += new System.EventHandler(this.circleToolStripMenuItem_Click);
      // 
      // ellipseToolStripMenuItem
      // 
      this.ellipseToolStripMenuItem.Name = "ellipseToolStripMenuItem";
      this.ellipseToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
      this.ellipseToolStripMenuItem.Text = "Ellipse";
      this.ellipseToolStripMenuItem.Click += new System.EventHandler(this.ellipseToolStripMenuItem_Click);
      // 
      // setColorToolStripMenuItem
      // 
      this.setColorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
      this.redToolStripMenuItem,
      this.yellowToolStripMenuItem,
      this.greenToolStripMenuItem,
      this.blueToolStripMenuItem});
      this.setColorToolStripMenuItem.Name = "setColorToolStripMenuItem";
      this.setColorToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
      this.setColorToolStripMenuItem.Text = "Set color";
      // 
      // redToolStripMenuItem
      // 
      this.redToolStripMenuItem.Name = "redToolStripMenuItem";
      this.redToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
      this.redToolStripMenuItem.Text = "Red";
      this.redToolStripMenuItem.Click += new System.EventHandler(this.redToolStripMenuItem_Click);
      // 
      // yellowToolStripMenuItem
      // 
      this.yellowToolStripMenuItem.Name = "yellowToolStripMenuItem";
      this.yellowToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
      this.yellowToolStripMenuItem.Text = "Yellow";
      this.yellowToolStripMenuItem.Click += new System.EventHandler(this.yellowToolStripMenuItem_Click);
      // 
      // greenToolStripMenuItem
      // 
      this.greenToolStripMenuItem.Name = "greenToolStripMenuItem";
      this.greenToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
      this.greenToolStripMenuItem.Text = "Green";
      this.greenToolStripMenuItem.Click += new System.EventHandler(this.greenToolStripMenuItem_Click);
      // 
      // blueToolStripMenuItem
      // 
      this.blueToolStripMenuItem.Name = "blueToolStripMenuItem";
      this.blueToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
      this.blueToolStripMenuItem.Text = "Blue";
      this.blueToolStripMenuItem.Click += new System.EventHandler(this.blueToolStripMenuItem_Click);
      // 
      // setLineStyleToolStripMenuItem
      // 
      this.setLineStyleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
      this.dashedToolStripMenuItem,
      this.continuousToolStripMenuItem});
      this.setLineStyleToolStripMenuItem.Name = "setLineStyleToolStripMenuItem";
      this.setLineStyleToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
      this.setLineStyleToolStripMenuItem.Text = "Set line style";
      // 
      // dashedToolStripMenuItem
      // 
      this.dashedToolStripMenuItem.Name = "dashedToolStripMenuItem";
      this.dashedToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
      this.dashedToolStripMenuItem.Text = "Dashed";
      this.dashedToolStripMenuItem.Click += new System.EventHandler(this.dashedToolStripMenuItem_Click);
      // 
      // continuousToolStripMenuItem
      // 
      this.continuousToolStripMenuItem.Name = "continuousToolStripMenuItem";
      this.continuousToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
      this.continuousToolStripMenuItem.Text = "Continuous";
      this.continuousToolStripMenuItem.Click += new System.EventHandler(this.continuousToolStripMenuItem_Click);
      // 
      // clearAllObjectsToolStripMenuItem
      // 
      this.clearAllObjectsToolStripMenuItem.Name = "clearAllObjectsToolStripMenuItem";
      this.clearAllObjectsToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
      this.clearAllObjectsToolStripMenuItem.Text = "Clear all objects";
      this.clearAllObjectsToolStripMenuItem.Click += new System.EventHandler(this.clearAllObjectsToolStripMenuItem_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(397, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Instructions: Add and modify drawing objects via context menu (right mouse button" +
        ")";
      // 
      // hsmartControl
      // 
      this.hsmartControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.hsmartControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
      this.hsmartControl.HMoveContent = true;
      this.hsmartControl.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
      this.hsmartControl.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
      this.hsmartControl.HImagePart = new System.Drawing.Rectangle(0, 0, 512, 512);
      this.hsmartControl.Location = new System.Drawing.Point(14, 25);
      this.hsmartControl.Margin = new System.Windows.Forms.Padding(15, 16, 15, 16);
      this.hsmartControl.Name = "hsmartControl";
      this.hsmartControl.Size = new System.Drawing.Size(512, 512);
      this.hsmartControl.TabIndex = 2;
      this.hsmartControl.WindowSize = new System.Drawing.Size(512, 512);
      this.hsmartControl.Load += new System.EventHandler(this.halconWindow_Load);
      // 
      // HALCONDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(538, 558);
      this.ContextMenuStrip = this.contextMenuStrip1;
      this.Controls.Add(this.hsmartControl);
      this.Controls.Add(this.label1);
      this.Name = "HALCONDialog";
      this.Text = "Drawing Objects in C#";
      this.Load += new System.EventHandler(this.HALCONDialog_Load);
      this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
      this.SizeChanged += new System.EventHandler(this.HALCONDialog_SizeChanged);
      this.contextMenuStrip1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    private System.Windows.Forms.ToolStripMenuItem addNewObjectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem rectangle1ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem rectangle2ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem circleToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem ellipseToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem setColorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem redToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem yellowToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem greenToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem setLineStyleToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem blueToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem dashedToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem continuousToolStripMenuItem;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ToolStripMenuItem clearAllObjectsToolStripMenuItem;
    private HalconDotNet.HSmartWindowControl hsmartControl;
  }
}

