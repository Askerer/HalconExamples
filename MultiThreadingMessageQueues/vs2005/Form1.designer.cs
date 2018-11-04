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
            this.setColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.yellowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.greenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setLineStyleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dashedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.continuousToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonRun = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.delayCheckBox = new System.Windows.Forms.CheckBox();
            this.halconWindow = new HalconDotNet.HSmartWindowControl();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setColorToolStripMenuItem,
            this.setLineStyleToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(140, 48);
            // 
            // setColorToolStripMenuItem
            // 
            this.setColorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.redToolStripMenuItem,
            this.yellowToolStripMenuItem,
            this.greenToolStripMenuItem,
            this.blueToolStripMenuItem});
            this.setColorToolStripMenuItem.Name = "setColorToolStripMenuItem";
            this.setColorToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
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
            this.setLineStyleToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
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
            // buttonRun
            // 
            this.buttonRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonRun.Font = new System.Drawing.Font("Candara", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRun.Location = new System.Drawing.Point(15, 562);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(113, 50);
            this.buttonRun.TabIndex = 2;
            this.buttonRun.Text = "Start";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonStop.Enabled = false;
            this.buttonStop.Font = new System.Drawing.Font("Candara", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStop.Location = new System.Drawing.Point(134, 562);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(113, 50);
            this.buttonStop.TabIndex = 3;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(284, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Adjust the Rectangular Model ROI";
            // 
            // delayCheckBox
            // 
            this.delayCheckBox.AutoSize = true;
            this.delayCheckBox.Font = new System.Drawing.Font("Candara", 12F);
            this.delayCheckBox.Location = new System.Drawing.Point(266, 581);
            this.delayCheckBox.Name = "delayCheckBox";
            this.delayCheckBox.Size = new System.Drawing.Size(113, 23);
            this.delayCheckBox.TabIndex = 5;
            this.delayCheckBox.Text = "Slow motion";
            this.delayCheckBox.UseVisualStyleBackColor = true;
            this.delayCheckBox.CheckedChanged += new System.EventHandler(this.delayCheckBox_CheckedChanged);
            // 
            // halconWindow
            // 
            this.halconWindow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.halconWindow.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.halconWindow.HMoveContent = true;
            this.halconWindow.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
            this.halconWindow.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.halconWindow.HImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.halconWindow.Location = new System.Drawing.Point(19, 35);
            this.halconWindow.Margin = new System.Windows.Forms.Padding(2);
            this.halconWindow.Name = "halconWindow";
            this.halconWindow.Size = new System.Drawing.Size(640, 512);
            this.halconWindow.TabIndex = 6;
            this.halconWindow.WindowSize = new System.Drawing.Size(640, 512);
            this.halconWindow.Load += new System.EventHandler(this.halconWindow_Load);
            // 
            // HALCONDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 632);
            this.Controls.Add(this.halconWindow);
            this.Controls.Add(this.delayCheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonRun);
            this.Name = "HALCONDialog";
            this.Text = "Performing Image Acquisition, Matching, and Display in Multiple Threads";
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.HALCONDialog_FormClosed);
            this.Resize += new System.EventHandler(this.HALCONDialog_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem setColorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem yellowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem greenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setLineStyleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dashedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem continuousToolStripMenuItem;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox delayCheckBox;
        private HalconDotNet.HSmartWindowControl halconWindow;
    }
}

