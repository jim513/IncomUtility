using IncomUtility;
using IncomUtility.APP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private APP_UI_ViewLogs winViewLogs;
        private APP_UI_DeviceInfo winDeviceInfo;
        private APP_UI_RawData winRawData;
        private APP_UI_SensorData winSensorData;
        private APP_UI_InstrumentSetting winInstrumentSetting;
        private APP_UI_SecuritySetup winSecuritySetup;
        private APP_UI_GasCalibration winGasCalibration;
        private APP_UI_CalAnalogueOutput winCalAnalogueOutput;
        private APP_UI_CalVoltageOutput winCalVoltageOutput;
        private APP_UI_CalCellDrive winCalCellDrive;
        private APP_UI_HardwareTest winHardwareTest;
        private APP_UI_CommLog winCommLog;
        private APP_UI_Debug winDebug;

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

        private void Win_Open<T>(ref T win) where T : Window, new()
        {
            if (win == null)
            {
                win = new T();
                T win2 = win;

                /*
                 * Add Closing Event to hide
                 */             
                win.Closing += (sender, e) =>
                {
                    e.Cancel = true;
                    win2.Hide();
                };
                win.Show();
            }
            else
            {
                win.Show();
                win.Activate();
            }
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (SerialPortIO.serialPort.IsOpen)
                SerialPortIO.serialPort.Close();
            for (int intCounter = App.Current.Windows.Count - 1; intCounter > 0; intCounter--)
            {
                App.Current.Windows[intCounter].Closing += (send,f) =>
                {
                    f.Cancel = false;
                };
                App.Current.Windows[intCounter].Close();
            }
        }
        private void tMenu_MonitorDeviceStatus_Click(object sender, RoutedEventArgs e)
        {
            Win_Open<APP_UI_MonitorDeviceStatus>(ref winMonitorDeviceStatus);
        }
     
        private void tMenu_Communication_Click(object sender, RoutedEventArgs e)
        {
            Win_Open<APP_UI_Communication>(ref winCommunication);
        }

        private void tMenu_ViewBLEInfo_Click(object sender, RoutedEventArgs e)
        {
            Win_Open<APP_UI_BLEInfo>(ref winBLEInfo);
        }

        private void tMenu_ViewDeviceInfo_Click(object sender, RoutedEventArgs e)
        {
            Win_Open<APP_UI_DeviceInfo>(ref winDeviceInfo);
        }
        private void tMenu_ViewLogs_Click(object sender, RoutedEventArgs e)
        {
            Win_Open<APP_UI_ViewLogs>(ref winViewLogs);
        }
  
        private void tMenu_MonitorRawData_Click(object sender, RoutedEventArgs e)
        {
            Win_Open<APP_UI_RawData>(ref winRawData);
        }

        private void tMenu_EditSensorData_Click(object sender, RoutedEventArgs e)
        {
            Win_Open<APP_UI_SensorData>(ref winSensorData);
        }

        private void tMenu_EditInstrumentSettings_Click(object sender, RoutedEventArgs e)
        {
            Win_Open<APP_UI_InstrumentSetting>(ref winInstrumentSetting);
        }

        private void tMenu_SecuritySetup_Click(object sender, RoutedEventArgs e)
        {
            Win_Open<APP_UI_SecuritySetup>(ref winSecuritySetup);
        }

        private void tMenu_GasCalibration_Click(object sender, RoutedEventArgs e)
        {
            Win_Open<APP_UI_GasCalibration>(ref winGasCalibration);
        }

        private void tMenu_CalAnalougeOutput_Click(object sender, RoutedEventArgs e)
        {
            Win_Open<APP_UI_CalAnalogueOutput>(ref winCalAnalogueOutput);
        }

        private void tMenu_CalVoltageOutput_Click(object sender, RoutedEventArgs e)
        {
            Win_Open<APP_UI_CalVoltageOutput>(ref winCalVoltageOutput);
        }

        private void tMenuCalCellDrive_Click(object sender, RoutedEventArgs e)
        {
            Win_Open<APP_UI_CalCellDrive>(ref winCalCellDrive);
        }

        private void tMenu_HardwareTest_Click(object sender, RoutedEventArgs e)
        {
            Win_Open<APP_UI_HardwareTest>(ref winHardwareTest);
        }

        private void tMenu_CommLog_Click(object sender, RoutedEventArgs e)
        {
            Win_Open<APP_UI_CommLog>(ref winCommLog);
        }
        private void tMenu_Debug_Click(object sender, RoutedEventArgs e)
        {
            Win_Open<APP_UI_Debug>(ref winDebug);
        }

        private void tBtn_ResetAlarmFaults_Click(object sender, RoutedEventArgs e)
        {
            int t = App.Current.Windows.Count;
            tTxt_SensorDataVersion.Text = t.ToString();
        }

       
    }
}
