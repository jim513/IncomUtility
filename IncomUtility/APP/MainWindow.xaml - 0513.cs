using IncomUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IncomUtility
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private double orginalWidth, originalHeight;
        private ScaleTransform scale;

        private APP_UI_MonitorDeviceStatus winMonitorDeviceStatus;
        private APP_UI_Communication winCommunication;
        private APP_UI_BLEInfo winBLEInfo;

        public static SerialPort serial = new SerialPort();
        public static Mutex _mutex = new Mutex();
        public MainWindow()
        {
            InitializeComponent();
            orginalWidth = Width;
            originalHeight = Height;

            scale = new ScaleTransform();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                ChangeSize(ActualWidth, ActualHeight);
            }
        }
        private void ChangeSize(double width, double height)
        {
            scale.ScaleX = width / orginalWidth;
            scale.ScaleY = height / originalHeight;

            FrameworkElement rootElement = Content as FrameworkElement;

            rootElement.LayoutTransform = scale;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ChangeSize(e.NewSize.Width, e.NewSize.Height);
        }
 
   
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (serial.IsOpen) serial.Close();
            for (int intCounter = App.Current.Windows.Count - 1; intCounter > 0; intCounter--)
            {
                App.Current.Windows[intCounter].Closing += Window_Event_Closing;
                App.Current.Windows[intCounter].Close();
            }
        }
        private void Window_Event_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
        }

        private void tMenu_MonitorDeviceStatus_Click(object sender, RoutedEventArgs e)
        {
            if (winMonitorDeviceStatus == null)
            {
                winMonitorDeviceStatus = new APP_UI_MonitorDeviceStatus();
                winMonitorDeviceStatus.Closing += Win_MonitorDeviceStatus_Closing;
                winMonitorDeviceStatus.Show();
            }
            else
            {
                winMonitorDeviceStatus.Show();
                winMonitorDeviceStatus.Activate();
            }
        }
        private void Win_MonitorDeviceStatus_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            winMonitorDeviceStatus.Hide();
        }
        private void tMenu_Communication_Click(object sender, RoutedEventArgs e)
        {
            if (winCommunication == null)
            {
                winCommunication = new APP_UI_Communication();
                winCommunication.Closing += Win_Communication_Closing;
                winCommunication.Show();
            }
            else
            {
                winCommunication.Show();
                winCommunication.Activate();
            }
        }
        private void Win_Communication_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            winCommunication.Hide();
        }


        private void tMenu_ViewBLEInfo_Click(object sender, RoutedEventArgs e)
        {
            if (winBLEInfo == null)
            {
                winBLEInfo = new APP_UI_BLEInfo();
                winBLEInfo.Closing += Win_BLEInfo_Closing;
                winBLEInfo.Show();
            }
            else
            {
                winBLEInfo.Show();
                winBLEInfo.Activate();
            }
        }
        private void Win_BLEInfo_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            winBLEInfo.Hide();
        }


        private void tMenu_ViewDeviceInfo_Click(object sender, RoutedEventArgs e)
        {
            if (winCommunication == null)
            {
                winCommunication = new APP_UI_Communication();
                winCommunication.Closing += Win_Communication_Closing;
                winCommunication.Show();
            }
            else
            {
                winCommunication.Show();
                winCommunication.Activate();
            }
        }
        private void Test<T> (T win) where T: new()
        {
            if(win == null)
            {
                win = new T();
            }

        }

    }
}
