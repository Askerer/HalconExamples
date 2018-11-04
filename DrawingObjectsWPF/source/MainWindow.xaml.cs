using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;
using HalconDotNet;

/*
 * This example not only demonstrates how to use the HSmartWindowControlWPF
 * within a WPF dialog, but also how to combine HDevelop and Visual Studio,
 * so that changes in your HDevelop script are automatically carried over
 * to the Visual Studio Project. Please refer to the "Build Events" property
 * tab.
 */

namespace DrawingObjectsWPF
{
  public delegate void DisplayResultsDelegate();

  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    HDevelopExport hdev_export;
    List<HTuple> drawing_objects;
    DisplayResultsDelegate display_results_delegate;
    HDrawingObject.HDrawingObjectCallback cb;
    HObject ho_EdgeAmplitude;
    HObject background_image = null;
    object image_lock = new object();
    private HImage image = new HImage("fabrik");

    public MainWindow()
    {
      InitializeComponent();
      InitializeComponent();
      hdev_export = new HDevelopExport();
      drawing_objects = new List<HTuple>();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      hSmartWindowControlWPF1.HalconWindow.DispObj(image);
    }

    private void OnRectangle1_Click(object sender, RoutedEventArgs e)
    {
      // Execute context menu command:
      // Add new rectangle1 drawing object
      HTuple draw_id;
      hdev_export.add_new_drawing_object("rectangle1", hSmartWindowControlWPF1.HalconID, out draw_id);
      SetCallbacks(draw_id);
    }

    private void OnRectangle2_Click(object sender, RoutedEventArgs e)
    {
      // Execute context menu command:
      // Add new rectangle2 drawing object
      HTuple draw_id;
      hdev_export.add_new_drawing_object("rectangle2", hSmartWindowControlWPF1.HalconID, out draw_id);
      SetCallbacks(draw_id);
    }

    private void OnCircle_Click(object sender, RoutedEventArgs e)
    {
      // Execute context menu command:
      // Add new circle drawing object
      HTuple draw_id;
      hdev_export.add_new_drawing_object("circle", hSmartWindowControlWPF1.HalconID, out draw_id);
      SetCallbacks(draw_id);
    }

    private void OnEllipse_Click(object sender, RoutedEventArgs e)
    {
      // Execute context menu command:
      // Add new ellipse drawing object
      HTuple draw_id;
      hdev_export.add_new_drawing_object("ellipse", hSmartWindowControlWPF1.HalconID, out draw_id);
      SetCallbacks(draw_id);
    }

    private void OnClearAllObjects_Click(object sender, RoutedEventArgs e)
    {
      lock (image_lock)
      {
        foreach (HTuple dobj in drawing_objects)
        {
          HOperatorSet.ClearDrawingObject(dobj);
        }
        drawing_objects.Clear();
      }
      hSmartWindowControlWPF1.HalconWindow.ClearWindow();
    }

    private void SetCallbacks(HTuple draw_id)
    {
      // Set callbacks for all relevant interactions
      drawing_objects.Add(draw_id);
      IntPtr ptr = Marshal.GetFunctionPointerForDelegate(cb);
      HOperatorSet.SetDrawingObjectCallback(draw_id, "on_resize", ptr);
      HOperatorSet.SetDrawingObjectCallback(draw_id, "on_drag", ptr);
      HOperatorSet.SetDrawingObjectCallback(draw_id, "on_attach", ptr);
      HOperatorSet.SetDrawingObjectCallback(draw_id, "on_select", ptr);
      lock (image_lock)
      {
        HOperatorSet.AttachDrawingObjectToWindow(hSmartWindowControlWPF1.HalconID, draw_id);
      }
    }

    private void hSmartWindowControlWPF1_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
      ContextMenu cm = this.FindResource("cmButton") as ContextMenu;
      cm.PlacementTarget = sender as Button;
      cm.IsOpen = true;
    }

    protected void DisplayCallback(IntPtr draw_id, IntPtr window_handle, string type)
    {
      // On callback, process and display image
      lock (image_lock)
      {
        hdev_export.process_image(background_image, out ho_EdgeAmplitude, hSmartWindowControlWPF1.HalconID, draw_id);
      }
      // You need to switch to the UI thread to display the results
      Dispatcher.BeginInvoke(display_results_delegate);
    }

    private void hSmartWindowControlWPF1_HInitWindow(object sender, EventArgs e)
    {
      // Initialize window
      // - Read and attach background image
      // - Initialize display results delegate function
      // - Initialize callbacks
      HTuple width, height;
      hdev_export.hv_ExpDefaultWinHandle = hSmartWindowControlWPF1.HalconID;
      HOperatorSet.ReadImage(out background_image, "fabrik");
      HOperatorSet.GetImageSize(background_image, out width, out height);
      hSmartWindowControlWPF1.HalconWindow.SetPart(0.0, 0.0, height - 1, width - 1);
      hSmartWindowControlWPF1.HalconWindow.AttachBackgroundToWindow(new HImage(background_image));
      display_results_delegate = new DisplayResultsDelegate(() =>
      {
        lock (image_lock)
        {
          if (ho_EdgeAmplitude != null)
            hdev_export.display_results(ho_EdgeAmplitude);
        }
      });
      cb = new HDrawingObject.HDrawingObjectCallback(DisplayCallback);
    }
  }
}
