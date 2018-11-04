using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using HalconDotNet;
using System.Windows.Data;

namespace BindingWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void HSmartWindowControlWPF_Loaded(object sender, RoutedEventArgs e)
        {
            HTuple Width, Height;

            WindowControl.HDisplayCurrentObject = new HImage("fabrik");

            ((HImage)WindowControl.HDisplayCurrentObject).GetImageSize(out Width, out Height);
            WindowControl.WindowSize = new System.Windows.Size(Width, Height);
            WindowControl.HImagePart = new System.Windows.Rect(0,0,Width-1,Height-1);
        }

        private void Button_Click_Fabrik(object sender, RoutedEventArgs e)
        {
            WindowControl.HDisplayCurrentObject = new HImage("fabrik");
        }

        private void Button_Click_Patras(object sender, RoutedEventArgs e)
        {
            WindowControl.HDisplayCurrentObject = new HImage("patras");
        }

        private void Button_Click_Atoms(object sender, RoutedEventArgs e)
        {
            WindowControl.HDisplayCurrentObject = new HImage("atoms");
        }

        private void Button_Click_PCBLayout(object sender, RoutedEventArgs e)
        {
            WindowControl.HDisplayCurrentObject = new HImage("pcb_layout");
            HRegion region = ((HImage)WindowControl.HDisplayCurrentObject).Threshold(0, 50.0);
            HRegion connected_region = region.Connection();
            WindowControl.HDisplayCurrentObject = connected_region;
        }
    }

    // Converter to reduce the decimal places provided by image part
    public class ImagePartDecimalPlaceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                string[] ImagePartArg = value.ToString().Split(';');

                string shortForm = ImagePartArg[0].Split(',').First() + ','
                                   + ImagePartArg[1].Split(',').First() + ','
                                   + ImagePartArg[2].Split(',').First() + ','
                                   + ImagePartArg[3].Split(',').First();
                return shortForm;
            }
            catch(Exception)
            {
                // In error case, just return the input value, do nothing
                return value.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // Nothing to do in set mode
            return value.ToString();
        }
    }
}

