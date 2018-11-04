using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HalconDotNet;

namespace HALCONDrawingObjects
{
    public partial class HALCONDialog : Form
    {
        private List<HDrawingObject> drawing_objects = new List<HDrawingObject>();
        private UserActions user_actions;
        private HDrawingObject selected_drawing_object = new HDrawingObject(250,250,100);
        private HImage background_image = null;
        private Size prevsize = new Size();

        public HALCONDialog()
        {
            InitializeComponent();
        }

        public HImage BackgroundImage { get { return background_image; } }

        private void OnSelectDrawingObject(HDrawingObject dobj, HWindow hwin, string type)
        {
            selected_drawing_object = dobj;
            user_actions.SobelFilter(dobj, hwin, type);
        }

        private void AttachDrawObj(HDrawingObject obj)
        {
            drawing_objects.Add(obj);
            // The HALCON/C# interface offers convenience methods that
            // encapsulate the set_drawing_object_callback operator.
            obj.OnDrag(user_actions.SobelFilter);
            obj.OnAttach(user_actions.SobelFilter);
            obj.OnResize(user_actions.SobelFilter);
            obj.OnSelect(OnSelectDrawingObject);
            obj.OnAttach(user_actions.SobelFilter);
            if (selected_drawing_object == null)
                selected_drawing_object = obj;
            hsmartControl.HalconWindow.AttachDrawingObjectToWindow(obj);
        }

        private void dashedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selected_drawing_object != null)
                selected_drawing_object.SetDrawingObjectParams(new HTuple("line_style"), new HTuple(20, 5));
        }

        private void continuousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selected_drawing_object != null)
                selected_drawing_object.SetDrawingObjectParams(new HTuple("line_style"), new HTuple());
        }

        private void rectangle1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HDrawingObject rect1 = HDrawingObject.CreateDrawingObject(
              HDrawingObject.HDrawingObjectType.RECTANGLE1, 100, 100, 210, 210);
            rect1.SetDrawingObjectParams("color", "green");
            AttachDrawObj(rect1);
        }

        private void rectangle2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HDrawingObject rect2 = HDrawingObject.CreateDrawingObject(
              HDrawingObject.HDrawingObjectType.RECTANGLE2, 100, 100, 0, 100, 50);
            rect2.SetDrawingObjectParams("color", "yellow");
            AttachDrawObj(rect2);
        }

        private void circleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HDrawingObject circle = HDrawingObject.CreateDrawingObject(
              HDrawingObject.HDrawingObjectType.CIRCLE, 200, 200, 70);
            circle.SetDrawingObjectParams("color", "magenta");
            AttachDrawObj(circle);
        }

        private void ellipseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HDrawingObject ellipse = HDrawingObject.CreateDrawingObject(
              HDrawingObject.HDrawingObjectType.ELLIPSE, 50, 50, 0, 100, 50);
            ellipse.SetDrawingObjectParams("color", "blue");
            AttachDrawObj(ellipse);
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                contextMenuStrip1.Show(this, new Point(e.X, e.Y));
        }

        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selected_drawing_object != null)
                selected_drawing_object.SetDrawingObjectParams("color", "red");
        }

        private void yellowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selected_drawing_object != null)
                selected_drawing_object.SetDrawingObjectParams("color", "yellow");
        }

        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selected_drawing_object != null)
                selected_drawing_object.SetDrawingObjectParams("color", "green");
        }

        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selected_drawing_object != null)
                selected_drawing_object.SetDrawingObjectParams("color", "blue");
        }

        private void halconWindow_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                contextMenuStrip1.Show(this, new Point(e.X, e.Y));
        }

        private void clearAllObjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
          foreach (HDrawingObject dobj in drawing_objects)
            dobj.Dispose();
          drawing_objects.Clear();
          selected_drawing_object = null;
        }

      private void HALCONDialog_Load(object sender, EventArgs e)
      {
        HTuple width, height;
        background_image = new HImage("fabrik");
        background_image.GetImageSize(out width, out height);
        hsmartControl.HalconWindow.SetPart(0, 0, height.I - 1, width.I - 1);
        hsmartControl.HalconWindow.AttachBackgroundToWindow(background_image);
        user_actions = new UserActions(this);
        this.AttachDrawObj(selected_drawing_object);
      }

      private void halconWindow_Load(object sender, EventArgs e)
      {
        this.MouseWheel += hsmartControl.HSmartWindowControl_MouseWheel;
        prevsize.Width = Width;
        prevsize.Height = Height;
      }

      private void HALCONDialog_SizeChanged(object sender, EventArgs e)
      {
        Size delta = new Size(Width - prevsize.Width, Height - prevsize.Height);
        hsmartControl.Size += delta;
        prevsize.Width = Width;
        prevsize.Height = Height;
      }
    }
}
