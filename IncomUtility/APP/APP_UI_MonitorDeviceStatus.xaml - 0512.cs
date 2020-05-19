using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace IncomUtility
{
    /// <summary>
    /// APP_UI_MonitorDeviceStatus.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class APP_UI_MonitorDeviceStatus : Window
    {
        private List<APP_UI_DataGrid> instrument_status_grid_data_list;
        private List<APP_UI_DataGrid> fault_status_grid_data_list;
        private List<APP_UI_DataGrid> warning_status_grid_data_list;
        public APP_UI_MonitorDeviceStatus()
        {
            InitializeComponent();

            instrument_status_grid_data_list = new List<APP_UI_DataGrid>();
            fault_status_grid_data_list = new List<APP_UI_DataGrid>();
            warning_status_grid_data_list = new List<APP_UI_DataGrid>();

            updateDataGrid(instrument_status_grid_data,instrument_status_grid_data_list, InstrumentStatus);
            updateDataGrid(fault_status_grid_data, fault_status_grid_data_list, FaultStatus);
            updateDataGrid(warning_status_grid_data,warning_status_grid_data_list, WarningStatus);
        }
     
        private void TimeSpanEdit_ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Get old and new value
            var newValue = e.NewValue;
            var oldValue = e.OldValue;
        }

        private string[] InstrumentStatus ={"Fault","Warning","Gas Alarm 1", "Gas Alarm 2", "Inhibit" , "Over-range", "BLE Status"};
        private string[] FaultStatus = {"Internal communication", "Cell Failure (No cell / PWM)","Cell is producing a negative",
            "EEPROM corruption", "uP Op voltage error", "RAM falut","Flash memory fault", "Code memory fault","mA output fault", 
            "Supplied voltage Fault","Internal HW Fault", "Internal SW Fault", "Calibration Overdue"};
        private string[] WarningStatus = { "Calibration Overdue", " Temperature limit", "BLE failure (BLE version", "Time/date not set (RTC",
            "Log memory corrupted", "Certificate is corrupted", "Over-range warning", "Under-range warning" };

        private void updateDataGrid(DataGrid grid, List<APP_UI_DataGrid> list, string[] text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                list.Add( new APP_UI_DataGrid(string.Format("Bit" + i + " - " + text[i]), ""));
            }
            for (int i = text.Length; i < 16; i++)
            {
                list.Add( new APP_UI_DataGrid("Bit" + i + " - Reserved", ""));

            }
            grid.Items.Refresh();

            if (grid.Items.Count > 0)
            {
                grid.ScrollIntoView(grid.Items[grid.Items.Count - 1]);
            }
        }
        Quattro quattro;  
        byte[] CMD_READ_DEVICE_STATUS = { 0x70, 0x01 };
           
        private void tBtn_ReadDeviceStatus_Click(object sender, RoutedEventArgs e)
        {
            if (!MainWindow.serial.IsOpen)
                return;
            if (quattro == null)
                quattro = new Quattro();

            byte[] u8TXbuffer = quattro.buildCMDPacket((byte)Quattro.PACKET_CONF.COMM_SYSTEM_MFG_PC, (byte)Quattro.PACKET_CONF.COMM_SYSTEM_INCOM,
             CMD_READ_DEVICE_STATUS);
            
            //tTxtMemo1.AppendText(BitConverter.ToString(u8TXbuffer) + "\n");

            quattro.writePacket(MainWindow.serial, ref u8TXbuffer);
            byte[] u8RXbuffer = quattro.readPacket(MainWindow.serial);

            //tTxtMemo1.AppendText(BitConverter.ToString(u8RXbuffer) +"\n");

            //int startPos = BitConverter.ToInt32(u8RXbuffer, u8RXbuffer.Length-5);
            //int startPos = BitConverter.ToInt32(u8RXbuffer, 0); // EE BB 00 3C
            for(int i = 0; i < 16; i++)
            {
                byte currentByte= u8RXbuffer[u8RXbuffer.Length - 4];
                byte pos = 1;
                if( i == 8 )
                {
                    currentByte = u8RXbuffer[u8RXbuffer.Length - 5];
                    pos = 1;
                }
                if ((currentByte & pos) == pos)
                {
                    //instrument_status_grid_data.Columns[1];
                  
                }
                else
                {

                }
                pos <<= 1;
            }

        }
    }
}
