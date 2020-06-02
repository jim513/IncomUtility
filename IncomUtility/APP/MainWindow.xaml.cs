using IncomUtility;
using IncomUtility.APP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;


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
        public static APP_UI_CommLog winCommLog;
        private APP_UI_Debug winDebug;

        private Thread gas_monitor;
        private List<UIDataGrid> gridGasDataList;
        private LogToFile logFiles;
        private MonitorChart m_chart1;

        private int gridRowMax;
        private int monitorPeriod;
        ERROR_LIST err;
        public MainWindow()
        {
            InitializeComponent();

            orginalWidth = Width;
            originalHeight = Height;

            scale = new ScaleTransform();
            logFiles = new LogToFile("Incom_Gas_data", "csv");
            m_chart1 = new MonitorChart(this, chart_1);

            winCommLog = new APP_UI_CommLog();
            winCommLog.Closing += (sender, e) =>
            {
                e.Cancel = true;
                winCommLog.Hide();
            };
            
            gas_monitor_init();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                ChangeSize(ActualWidth, ActualHeight);
            }
            gas_chart_hide();
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
                App.Current.Windows[intCounter].Closing += (send, f) =>
                {
                    f.Cancel = false;
                };
                App.Current.Windows[intCounter].Close();
            }
            if (gas_monitor_running)
                gas_monitor.Abort();

            logFiles.Unchecked();
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

        private void tChb_CaputreDebugPacket_Checked(object sender, RoutedEventArgs e)
        {
            APP_UI_CommLog.packetCheck = (bool)tChb_CaputreDebugPacket.IsChecked;
        }
        private void tBtn_MainStart_Click(object sender, RoutedEventArgs e)
        {
            if (!SerialPortIO.isPortOpen())
            {
                MessageBox.Show("Incom is not connected");
                return;

            }
            string strHeader1 = "";
            /*
              *  Read Device SN
             */
            byte[] result = SerialPortIO.sendCommand(COMM_COMMAND_LIST.COMM_CMD_READ_DEVICE_SN, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                MessageBox.Show("ERROR - Read Device SN");
                return;
            }
            string DeviceSN = Encoding.Default.GetString(Quattro.getResponseValueData(result)).Trim('\0');
            strHeader1 += "DeviceSN : " + DeviceSN + ",";

            /*
          * Read SW Version
          */
            result = SerialPortIO.sendCommand(COMM_COMMAND_LIST.COMM_CMD_READ_SW_VERSION, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                MessageBox.Show("ERROR - Read SW Version");
                return;
            }
            int offset = (int)PACKET_CONF.COMM_POS_PAYLOAD + 3;
            int major = result[offset];
            int minor = result[offset + 1];
            int built = Utility.getU16FromByteA(result, offset + 2);
            tTxt_FirmVersion.Text = major.ToString() + "." + minor.ToString() + "." + built.ToString();
            strHeader1 += "SW Version : " + major.ToString() + "." + minor.ToString() + "." + built.ToString() + ",";

            /*
            *  Read EEPROM Version
            */
            result = SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_EEPROM_VER, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                MessageBox.Show("ERROR - Read EERPOM Version"); ;
                return;
            }
            byte E2PVer = result[(int)PACKET_CONF.COMM_POS_PAYLOAD + 3];
            tTxt_SensorDataVersion.Text = E2PVer.ToString();
            strHeader1 += "EEPROM : " + E2PVer.ToString() + ",";

            /*
            *  Read Output Type
            */
            result = SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_OUTPUT_DEVICE_TYPE, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                MessageBox.Show("ERROR - Read Board SN");
                return;
            }
            offset = (int)PACKET_CONF.COMM_POS_PAYLOAD + (int)PACKET_CONF.COMM_RESPONSE_SZ;
            int OutputType = result[offset];
            int Relay = result[offset + 1];
            int BLEModule = result[offset + 2];

            if (OutputType == 0)
                strHeader1 += "OutPut Type : mA Output,";
            else
                strHeader1 += "OutPut Type : Modbus,";
            if (Relay == 0)
                strHeader1 += "Relay : Not Fitted,";
            else
                strHeader1 += "Relay : Fitted,";
            if (BLEModule == 0)
                strHeader1 += "BLE : Not Fitted";
            else
                strHeader1 += "BLE : Not Fitted";

            /*
            *  Save Data to CSV file
            */
            string filePath = logFiles.Checked();
            string strHeader2 = "";
            if (filePath != null)
            {
                int columCount = tGrid_GasData.Columns.Count;
                for (int i = 0; i < columCount; i++)
                {
                    if (i == tGrid_GasData.Columns.Count - 1)
                    {
                        strHeader2 += tGrid_GasData.Columns[i].Header.ToString();
                        break;
                    }
                    strHeader2 += tGrid_GasData.Columns[i].Header.ToString() + ",";
                }
                logFiles.SetDataHeader(strHeader1, strHeader2);
            }

            /*
            *  Monitoring Start
            */
            gas_monitor_running = true;

            tBtn_MainStart.IsEnabled = false;
            tBtn_MainStop.IsEnabled = true;
            tUpdown_NumReadings.IsEnabled = false;
            tUpdown_LogInterval.IsEnabled = false;
            monitorPeriod = (int)tUpdown_LogInterval.Value * 1000 - 300;

            gas_chart_reset();
            gas_chart_make();
            gas_chart_show(1800);

            gas_monitor = new Thread(gas_monitor_run);
            gas_monitor.Start();
        }

        private void tBtn_MainStop_Click(object sender, RoutedEventArgs e)
        {
            logFiles.Unchecked();

            gas_monitor_stop();

            tBtn_MainStart.IsEnabled = true;
            tBtn_MainStop.IsEnabled = false;
            tUpdown_NumReadings.IsEnabled = true;
            tUpdown_LogInterval.IsEnabled = true;

        }
        private void tBtn_MainClear_Click(object sender, RoutedEventArgs e)
        {
            gridGasDataList.Clear();
            tGrid_GasData.Items.Refresh();

            gas_chart_reset();
            gas_chart_make();
        }

        private void tBtn_ResetAlarmFaults_Click(object sender, RoutedEventArgs e)
        {
            if (!SerialPortIO.isPortOpen())
            {
                MessageBox.Show("Incom is not connected");
                return;
            }
            SerialPortIO.sendCommand(COMM_COMMAND_LIST.COMM_CMD_RESET_ALARMS, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                MessageBox.Show("ERROR - Reset Alarms and Faults");
                return;
            }
            MessageBox.Show("Succesfully Reset Alarms and Faults");
        }

        #region GAS MONITORING
        private string gas_data_1 = "Raw ADC";
        private string gas_data_2 = "Cell Output";
        private string gas_data_3 = "Primary Conc.";
        private string gas_data_4 = "Lin. Conc.";
        private string gas_data_5 = "Deadband Conc.";
        private string gas_data_6 = "Display Conc.";
        private string gas_data_7 = "Raw NTC";
        private string gas_data_8 = "Comp'd Temp";
        private string gas_data_9 = "Fault Status";
        private string gas_data_10 = "Warning Status";
        private string gas_data_11 = "Alarm Status";
        private string gas_data_12 = "Voltage Out";
        private string gas_data_13 = "Voltage Loop";
        private string gas_data_14 = "mA Out";
        private string gas_data_15 = "mA Loop";

        private bool gas_monitor_running;

        private void gas_monitor_init()
        {
            gas_grid_make();

            gas_chart_make();
        }

        private void gas_monitor_run()
        {
            int offset = (int)PACKET_CONF.COMM_POS_PAYLOAD + (int)PACKET_CONF.COMM_RESPONSE_SZ;

            int raw_adc;
            float cell_ouput;
            float primary_conc;
            float linear_conc;
            float deadband_conc;
            float display_conc;
            int raw_temp;
            int temp;
            uint fault_state;
            uint warning_state;
            byte alarm_state;

            float target_analouge_output;
            float measured_lop_back_current;

            byte ma_output_type;
            float target_output;
            float loop_back;

            while (gas_monitor_running)
            {
                DateTime curr_Time = DateTime.Now;
                string date_time_str = curr_Time.ToString("yyyy/MM/dd HH:mm:ss");
                string time_str = curr_Time.ToString("HH:mm:ss");

                byte[] gas_raw_data_buffer = SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_RAW_GAS_DATA, ref err);
                if (err != ERROR_LIST.ERROR_NONE)
                    continue;
                byte[] gas_voltage_output_buffer = SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_VOLTAGE_OUTPUT, ref err);
                if (err != ERROR_LIST.ERROR_NONE)
                    continue;
                byte[] gas_analouge_output_buffer = SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_ANALOGUE_OUTPUT, ref err);
                if (err != ERROR_LIST.ERROR_NONE)
                    continue;

                raw_adc = Utility.getS32FromByteA(gas_raw_data_buffer, offset + 0);
                cell_ouput = Utility.getF32FromByteA(gas_raw_data_buffer, offset + 4);
                primary_conc = Utility.getF32FromByteA(gas_raw_data_buffer, offset + 8);
                linear_conc = Utility.getF32FromByteA(gas_raw_data_buffer, offset + 12);
                deadband_conc = Utility.getF32FromByteA(gas_raw_data_buffer, offset + 16);
                display_conc = Utility.getF32FromByteA(gas_raw_data_buffer, offset + 20);
                raw_temp = Utility.getS32FromByteA(gas_raw_data_buffer, offset + 24);
                temp = Utility.getS32FromByteA(gas_raw_data_buffer, offset + 28);
                fault_state = Utility.getU32FromByteA(gas_raw_data_buffer, offset + 32);
                warning_state = Utility.getU32FromByteA(gas_raw_data_buffer, offset + 36);
                alarm_state = gas_raw_data_buffer[offset + 40];


                target_analouge_output = Utility.getF32FromByteA(gas_voltage_output_buffer, offset);
                measured_lop_back_current = Utility.getF32FromByteA(gas_voltage_output_buffer, offset + 4);

                ma_output_type = gas_analouge_output_buffer[offset];
                target_output = Utility.getF32FromByteA(gas_analouge_output_buffer, offset + 1);
                loop_back = Utility.getF32FromByteA(gas_analouge_output_buffer, offset + 5);

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    gas_data_update(tGrid_GasData, gridGasDataList, m_chart1, time_str, raw_adc, cell_ouput, primary_conc, linear_conc, deadband_conc,
     display_conc, raw_temp, temp, fault_state, warning_state, alarm_state, target_analouge_output, measured_lop_back_current, target_output, loop_back);
                }
                ));
                string gas_data = date_time_str + string.Format(",{0},{1:0.000},{2:0.000},{3:0.000}," +
                                                  "{4:0.000},{5:0.000},{6},{7}," +
                                                  "{8},{9},{10},{11:0.000}," +
                                                  "{12:0.000},{13:0.000},{14:0.000}", raw_adc, cell_ouput, primary_conc, linear_conc, deadband_conc,
                                                  display_conc, raw_temp, temp, fault_state, warning_state, alarm_state, target_analouge_output,
                                                  measured_lop_back_current, target_output, loop_back
                                             );
                logFiles.Write(gas_data);
                Thread.Sleep(monitorPeriod);
            }
        }
        private void gas_monitor_stop()
        {
            if (gas_monitor != null)
            {
                gas_monitor_running = false;

                gas_monitor.Join();
            }
        }
        private void gas_grid_make()
        {
            gridGasDataList = new List<UIDataGrid>();

            tGrid_GasData.ItemsSource = gridGasDataList;

            gas_grid_make_header(tGrid_GasData,
                         gas_data_1, gas_data_2, gas_data_3, gas_data_4,
                         gas_data_5, gas_data_6, gas_data_7, gas_data_8,
                         gas_data_9, gas_data_10, gas_data_11, gas_data_12,
                         gas_data_13, gas_data_14, gas_data_15);
        }

        private void gas_grid_make_header(DataGrid grid,
                                string data1_name_1, string data1_name_2, string data1_name_3, string data1_name_4,
                                string data1_name_5, string data1_name_6, string data1_name_7, string data1_name_8,
                                string data1_name_9, string data1_name_10, string data1_name_11, string data1_name_12,
                                string data1_name_13, string data1_name_14, string data1_name_15)
        {
            grid.Columns[0].Header = "Time/Date";
            grid.Columns[1].Header = data1_name_1;
            grid.Columns[2].Header = data1_name_2;
            grid.Columns[3].Header = data1_name_3;
            grid.Columns[4].Header = data1_name_4;
            grid.Columns[5].Header = data1_name_5;
            grid.Columns[6].Header = data1_name_6;
            grid.Columns[7].Header = data1_name_7;
            grid.Columns[8].Header = data1_name_8;
            grid.Columns[9].Header = data1_name_9;
            grid.Columns[10].Header = data1_name_10;
            grid.Columns[11].Header = data1_name_11;
            grid.Columns[12].Header = data1_name_12;
            grid.Columns[13].Header = data1_name_13;
            grid.Columns[14].Header = data1_name_14;
            grid.Columns[15].Header = data1_name_15;
        }


        private void gas_data_update(DataGrid grid, List<UIDataGrid> list, MonitorChart chart1,
                               string time, int data1, float data2, float data3, float data4, float data5, float data6, int data7, float data8,
                               uint data9, uint data10, byte data11, float data12, float data13, float data14, float data15)
        {
            gridRowMax = (int)tUpdown_NumReadings.Value;

            if (grid.Items.Count >= gridRowMax)
            {
                list.RemoveAt(0);
            }

            list.Add(new UIDataGrid(time, string.Format("{0}", data1), string.Format("{0:f4}", data2),
                                        string.Format("{0:f4}", data3), string.Format("{0:f4}", data4),
                                        string.Format("{0:f4}", data5), string.Format("{0:f1}", data6),
                                        string.Format("{0}", data7), string.Format("{0}", data8),
                                        string.Format("{0}", data9), string.Format("{0}", data10),
                                        string.Format("{0}", data11), string.Format("{0}", data12),
                                        string.Format("{0}", data13), string.Format("{0}", data14),
                                        string.Format("{0:f4}", data15)
                                        ));

            chart1.AddData(0, time, data1);
            chart1.AddData(1, time, data2);
            chart1.AddData(2, time, data3);
            chart1.AddData(3, time, data4);

            grid.Items.Refresh();

            if (grid.Items.Count > 0)
            {
                grid.ScrollIntoView(grid.Items[grid.Items.Count - 1]);
            }

        }
        private void gas_chart_make()
        {
            System.Drawing.Color ch1_color = System.Drawing.Color.Red;
            System.Drawing.Color ch2_color = System.Drawing.Color.DarkOrange;
            System.Drawing.Color ch3_color = System.Drawing.Color.Blue;
            System.Drawing.Color ch4_color = System.Drawing.Color.Green;

            m_chart1.AddSeries(gas_data_1, chart_1_legend_1, ch1_color);
            m_chart1.AddSeries(gas_data_2, chart_1_legend_2, ch2_color);
            m_chart1.AddSeries(gas_data_3, chart_1_legend_3, ch3_color);
            m_chart1.AddSeries(gas_data_4, chart_1_legend_4, ch4_color);

        }
        private void gas_chart_show(int max_window)
        {
            m_chart1.WindowSize = max_window;

            m_chart1.ShowLegend();
        }

        private void gas_chart_hide()
        {
            m_chart1.HideLegend();
        }

        private void gas_chart_reset()
        {
            m_chart1.Reset();
        }
        #endregion


        #region Chart control
        private void chart_1_legend_1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            m_chart1.ToggleLegend(0);
        }

        private void chart_1_legend_2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            m_chart1.ToggleLegend(1);
        }

        private void chart_1_legend_3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            m_chart1.ToggleLegend(2);
        }

        private void chart_1_legend_4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            m_chart1.ToggleLegend(3);

        }
        #endregion
    }
}