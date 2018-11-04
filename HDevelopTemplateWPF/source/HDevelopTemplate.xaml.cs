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
using System.Threading;

using HalconDotNet;

namespace HDevelopTemplateWPF
{
  /// <summary>
  /// Interaction logic for Window1.xaml
  /// </summary>
  public partial class HDevelopTemplate : Window
  {
    // The class HDevelopExport will be defined in the HALCON program
    // exported from HDevelop for 'C# - HALCON/.NET' and the Template
    // Window Export.
    private HDevelopExport HDevExp;

    public HDevelopTemplate()
    {
      InitializeComponent();

      HDevExp = new HDevelopExport();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      HDevExp.InitHalcon();
    }

    private void RunExport()
    {
      HTuple WindowID = hWindowControlWPF1.HalconID;
      HDevExp.RunHalcon(WindowID);

      this.Dispatcher.Invoke(new Action(() => {
        labelStatus.Content = "Finished.";
        buttonRun.IsEnabled = true;
      }));
    }

    private void buttonRun_Click(object sender, RoutedEventArgs e)
    {
      buttonRun.IsEnabled = false;
      labelStatus.Content = "Running...";
      labelStatus.UpdateLayout();

      Thread exportThread = new Thread(new ThreadStart(this.RunExport));
      exportThread.Start();
    }

  }
}
