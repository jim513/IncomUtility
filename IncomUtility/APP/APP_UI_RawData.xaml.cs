﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IncomUtility.APP
{
    /// <summary>
    /// APP_UI_RawData.xaml에 대한 상호 작용 논리
    /// </summary>
    public enum ANALOGUE_OUTPUT_TYPE
    {
        NOT_SPECIFIED = 0,
        SINK_MODE = 1,
        SOURCE_MODE = 2,
    }
    public partial class APP_UI_RawData : Window
    {
        ERROR_LIST err = ERROR_LIST.ERROR_NONE;
        DateTime now ;
        public APP_UI_RawData()
        {
            InitializeComponent();
        }

        private void tBtn_GetGasReading_Click(object sender, RoutedEventArgs e)
        {
            getGasReading();          
        }

        private void tBtn_ReadAnalogueOutput_Click(object sender, RoutedEventArgs e)
        {
            readAnalogueOutput();
        }

        private void tBtn_ReadCellVoltage_Click(object sender, RoutedEventArgs e)
        {
            readCellVoltage();           
        }

        private void tBtn_ReadRawADC_Click(object sender, RoutedEventArgs e)
        {
            readRawADC();           
        }

        private void tBtn_ReadRawCellSignal_Click(object sender, RoutedEventArgs e)
        {
            readRawCellSignal();         
        }

        private void tBtn_ReadTemperature_Click(object sender, RoutedEventArgs e)
        {
            readTemperature();          
        }

        private void tBtn_ReadVoltageLevel_Click(object sender, RoutedEventArgs e)
        {
            readVoltageLevel();         
        }

        private void tBtn_ReadVoltageOutput_Click(object sender, RoutedEventArgs e)
        {
            readVoltageOutput();         
        }

        private void tBtn_DebugGasReading_Click(object sender, RoutedEventArgs e)
        {
            debugGasReading();            
        }

        private void getGasReading()
        {
            /*
             * Get Gas Reading
             */
            byte[] channel = { 0x00 };
            byte[] result = QuattroProtocol.sendCommand(COMM_COMMAND_LIST.COMM_CMD_GET_GAS_READING, channel, ref err, 300);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - GET GAS READING");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            int offset = (int)PACKET_CONF.COMM_POS_PAYLOAD + (int)PACKET_CONF.COMM_RESPONSE_SZ;
            int recivedChannel = result[offset];
            float value = Utility.getF32FromByteA(result, offset + 1);
            int status = Utility.getU16FromByteA(result, offset + 5);

            now = DateTime.Now;
            tTxt_Logs.AppendText(now.ToLongTimeString() + " Channel : " + recivedChannel + ", Readings : " + value.ToString("F1") +
                ", Status : " + status);
            tTxt_Logs.AppendText(Environment.NewLine);
        }

        private void readAnalogueOutput()
        {
            /*
            * Read Analogue Output
            */
            byte[] result = QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_ANALOGUE_OUTPUT, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - READ ANLOGUE OUTPUT");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }

            int offset = (int)PACKET_CONF.COMM_POS_PAYLOAD + (int)PACKET_CONF.COMM_RESPONSE_SZ;
            byte mAOutputType = result[offset];
            float targetOutput = Utility.getF32FromByteA(result, offset + 1);
            float loopBack = Utility.getF32FromByteA(result, offset + 5);

            if (mAOutputType == (int)ANALOGUE_OUTPUT_TYPE.SINK_MODE)
            {
                tTxt_Logs.AppendText("Analogue Type : " + mAOutputType + " - sink mode");
            }
            else if (mAOutputType == (int)ANALOGUE_OUTPUT_TYPE.SOURCE_MODE)
            {
                tTxt_Logs.AppendText("Analogue Type : " + mAOutputType + " - source mode");
            }
            else
            {
                tTxt_Logs.AppendText("Analogue Type : " + mAOutputType + " - Not specified");
            }

            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_Logs.AppendText("Target output (mA) : " + targetOutput);
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_Logs.AppendText("Loopback (mA): " + loopBack);
            tTxt_Logs.AppendText(Environment.NewLine);
        }

        private void readCellVoltage()
        {
            /*
             * Read Cell Voltage /Current
             */
            byte[] result = QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_CELL_VOLTAGE_CURRENT, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - READ CELL VOLTAGE / CURRENT");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            int offset = (int)PACKET_CONF.COMM_POS_PAYLOAD + (int)PACKET_CONF.COMM_RESPONSE_SZ;
            float voltage = Utility.getF32FromByteA(result, offset);
            float current = Utility.getF32FromByteA(result, offset + 4);

            tTxt_Logs.AppendText("Cell Voltage : " + voltage);
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_Logs.AppendText("Cell Current : " + current);
            tTxt_Logs.AppendText(Environment.NewLine);
        }

        private void readRawADC()
        {
            /*
             * Read Raw ADC
             */
            byte[] result = QuattroProtocol.sendCommand(COMM_COMMAND_LIST.COMM_CMD_READ_RAW_ADC, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - READ RAW ADC");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            int offset = (int)PACKET_CONF.COMM_POS_PAYLOAD + (int)PACKET_CONF.COMM_RESPONSE_SZ;
            int channelNum = result[offset];
            tTxt_Logs.AppendText("Number of AD channel : " + channelNum);
            tTxt_Logs.AppendText(Environment.NewLine);
            for (int i = 0; i < channelNum; i++)
            {
                uint AD = Utility.getU32FromByteA(result, offset + 1 + 4 * i);
                tTxt_Logs.AppendText("AD channel : " + i + 1 + " : " + AD);
                tTxt_Logs.AppendText(Environment.NewLine);
            }
        }
        
        private void readRawCellSignal()
        {
            /*
             * Read Raw Cell Signal
             */
            byte[] result = QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_RAW_CELL_SIGNAL, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - READ RAW CELL SIGANL");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }

            int offset = (int)PACKET_CONF.COMM_POS_PAYLOAD + (int)PACKET_CONF.COMM_RESPONSE_SZ;
            float cellOutput = Utility.getF32FromByteA(result, offset);
            float cellOffset = Utility.getF32FromByteA(result, offset + 4);

            tTxt_Logs.AppendText("Cell Output : " + cellOutput);
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_Logs.AppendText("Cell Offset : " + cellOffset);
            tTxt_Logs.AppendText(Environment.NewLine);
        }

        private void readTemperature()
        {   
            /*
             * Read Internal Temperature
             */
            byte[] result = QuattroProtocol.sendCommand(COMM_COMMAND_LIST.COMM_CMD_INTERNAL_TEMP, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - READ TEMPERATURE");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }

            int offset = (int)PACKET_CONF.COMM_POS_PAYLOAD + (int)PACKET_CONF.COMM_RESPONSE_SZ;
            float temperature = Utility.getF32FromByteA(result, offset);

            tTxt_Logs.AppendText("Internal temp : " + temperature + "oC");
            tTxt_Logs.AppendText(Environment.NewLine);
        }

        private void readVoltageLevel()
        {
            /*
            * Read Voltage Level
            */
            byte[] result = QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_VOLTAGE_LEVELS, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - READ VOLTAGE LEVEL");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }

            int offset = (int)PACKET_CONF.COMM_POS_PAYLOAD + (int)PACKET_CONF.COMM_RESPONSE_SZ;
            float suppliedVoltage = Utility.getF32FromByteA(result, offset);
            float operationgVotage = Utility.getF32FromByteA(result, offset + 4);
            float referenceVotage = Utility.getF32FromByteA(result, offset + 8);

            tTxt_Logs.AppendText("24V : " + suppliedVoltage);
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_Logs.AppendText("3.3V : " + operationgVotage);
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_Logs.AppendText("5V : " + referenceVotage);
            tTxt_Logs.AppendText(Environment.NewLine);
        }

        private void readVoltageOutput()
        {
            /*
             * Read Voltage Output
             */
            byte[] result = QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_VOLTAGE_OUTPUT, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - READ VOLTAGE OUTPUT");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }

            int offset = (int)PACKET_CONF.COMM_POS_PAYLOAD + (int)PACKET_CONF.COMM_RESPONSE_SZ;
            float targetOutput = Utility.getF32FromByteA(result, offset);
            float loopBack = Utility.getF32FromByteA(result, offset + 4);

            tTxt_Logs.AppendText("Target output (V) : " + targetOutput);
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_Logs.AppendText("Loopback (V): " + loopBack);
            tTxt_Logs.AppendText(Environment.NewLine);
        }

        private void debugGasReading()
        {
            /*
             * Read Raw GasData
             */
            byte[] result = QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_RAW_GAS_DATA, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - READ RAW GAS DATA");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }

            int offset = (int)PACKET_CONF.COMM_POS_PAYLOAD + (int)PACKET_CONF.COMM_RESPONSE_SZ;
            int rawADC = Utility.getS32FromByteA(result, offset);
            float cellOutput = Utility.getF32FromByteA(result, offset + 4);
            float primaryConc = Utility.getF32FromByteA(result, offset + 8);
            float linearConc = Utility.getF32FromByteA(result, offset + 12);
            float deadband = Utility.getF32FromByteA(result, offset + 16);
            float displayConc = Utility.getF32FromByteA(result, offset + 20);
            int rawTemp = Utility.getS32FromByteA(result, offset + 24);
            int temp = Utility.getS32FromByteA(result, offset + 28);
            uint faultState = Utility.getU32FromByteA(result, offset + 32);
            uint warningState = Utility.getU32FromByteA(result, offset + 36);
            byte alarmState = result[offset + 40];

            tTxt_Logs.AppendText("Raw ADC: " + rawADC);
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_Logs.AppendText("Cell output: " + cellOutput.ToString("F3"));
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_Logs.AppendText("Primary Conc.: " + primaryConc.ToString("F3"));
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_Logs.AppendText("Linearized Conc.: " + linearConc.ToString("F3"));
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_Logs.AppendText("Deadbanded Conc.: " + deadband.ToString("F3"));
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_Logs.AppendText("Displayed Conc.: " + displayConc.ToString("F3"));
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_Logs.AppendText("NTC temp.: " + rawTemp);
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_Logs.AppendText("Compensated temp.: " + temp);
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_Logs.AppendText("Fault Status: " + faultState);
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_Logs.AppendText("Warning Status: " + warningState);
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_Logs.AppendText("Alarm Status: " + alarmState);
            tTxt_Logs.AppendText(Environment.NewLine);

        }
    }
}
