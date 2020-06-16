using IncomUtility.APP;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Globalization;
using System.Resources;
using System.Reflection;
using System.Windows.Forms.DataVisualization.Charting;

namespace IncomUtility
{
    enum Constants
    {
        retryCount = 3,
        defaultSleep = 100
    }
    public enum ERROR_LIST
    {
        ERROR_NONE = 0,
        ERROR_PORT_NOT_OPEN = 1,
        ERROR_INPUT_DATA_NONE = 2,
        ERROR_INPUT_DATA_WRONG = 3,
        ERROR_RECIVE_DATA_NONE = 4,
        ERROR_RECIVE_DATA_WRONG = 5,
        ERROR_SOF_WRONG = 6,
        ERROR_EOF_WRONG = 7,
        ERROR_EXCEPTION = 8,
        ERROR_CRC_WRONG = 9,
        ERROR_NCK = 10

    }
    public static class SerialPortIO
    {
        public static ResourceManager rm = new ResourceManager("IncomUtility.StringsKor", Assembly.GetExecutingAssembly());
        //public static ResourceManager rm = new ResourceManager("IncomUtility.StringsEng", Assembly.GetExecutingAssembly());
        //public static ResourceManager rm = new ResourceManager("IncomUtility.StringsChn", Assembly.GetExecutingAssembly());

        public static SerialPort serialPort = new SerialPort();
        public static Mutex mutex = new Mutex();
        private static QuattroProtocol quattro = new QuattroProtocol();
        public static bool isMessageboxWortk = true;

        public static bool isPortOpen()
        {
            if (serialPort.IsOpen)
            {
                return true;
            }
            string[] names = SerialPort.GetPortNames();

            if (names.Length == 0)
            {
                return false;
            }

            serialPort.PortName = names[0];

            try
            {
                serialPort.Open();
            }
            catch
            {
                if (isMessageboxWortk)
                {
                   // MessageBox.Show("Connecting was failed");
                    MessageBox.Show(rm.GetString("NotConnected"));
                }
                return false;
            }
            if (serialPort.IsOpen)
            {
                return true;
            }

            return false;
        }
        private static void writePacket(ref byte[] sendbuffer, ref ERROR_LIST err)
        {

            if (!isPortOpen())
            {
                err = ERROR_LIST.ERROR_PORT_NOT_OPEN;
                return;
            }
            if (sendbuffer == null)
            {
                err = ERROR_LIST.ERROR_INPUT_DATA_NONE;
                return;
            }
            serialPort.Write(sendbuffer, 0, sendbuffer.Length);
            err = ERROR_LIST.ERROR_NONE;
        }

        private static byte[] readPacket(ref ERROR_LIST err)
        {
            if (!isPortOpen())
            {
                err = ERROR_LIST.ERROR_PORT_NOT_OPEN;
                return null;
            }

            int readBytes = serialPort.BytesToRead;
            byte[] readBuffer = new byte[readBytes];

            if (serialPort.BytesToRead > 0)
            {
                serialPort.Read(readBuffer, 0, readBytes);
            }
            else
            {
                err = ERROR_LIST.ERROR_RECIVE_DATA_NONE;
                return null;
            }

            err = ERROR_LIST.ERROR_NONE;

            return readBuffer;
        }
        public static byte[] sendCommand(byte[] payload, ref ERROR_LIST error, int sleepTime)
        {
            if (!isPortOpen())
            {
                error = ERROR_LIST.ERROR_PORT_NOT_OPEN;
                if (isMessageboxWortk)
                {
                //    MessageBox.Show("Incom is not connected");
                    MessageBox.Show(rm.GetString("NotConnected"));
                }
                return null;
            }
            if (payload == null)
            {
                error = ERROR_LIST.ERROR_INPUT_DATA_NONE;
                return null;
            }

            int retryCount = (int)Constants.retryCount;
            byte[] u8TXbuffer = null;
            byte[] u8RXbuffer = null;

            mutex.WaitOne();

            while (retryCount-- > 0)
            {
                flushIOBuffer();

                u8TXbuffer = quattro.buildCMDPacket((byte)PACKET_CONF.COMM_SYSTEM_MFG_PC,
                    (byte)PACKET_CONF.COMM_SYSTEM_INCOM, payload, ref error);
                if (error != ERROR_LIST.ERROR_NONE)
                {
                    continue;
                }

                writePacket(ref u8TXbuffer, ref error);
                if (error != ERROR_LIST.ERROR_NONE)
                {
                    continue;
                }
                Thread.Sleep(sleepTime);

                u8RXbuffer = readPacket(ref error);
                if (error != ERROR_LIST.ERROR_NONE)
                {
                    continue;
                }

                error = quattro.validateRXPacket(u8RXbuffer);
                if (error == ERROR_LIST.ERROR_NONE)
                {
                    break;
                }
            }
            if (APP_UI_CommLog.packetCheck == true)
            {
                MainWindow.winCommLog.setText(u8TXbuffer, u8RXbuffer);
            }

            flushIOBuffer();
            mutex.ReleaseMutex();

            return u8RXbuffer;
        }
        public static byte[] sendCommand(COMM_COMMAND_LIST CMD, byte[] payload, ref ERROR_LIST err, int sleepTime)
        {
            if (payload == null)
            {
                err = ERROR_LIST.ERROR_INPUT_DATA_NONE;
                return null;
            }
            byte[] command = QuattroProtocol.commandToByteArray(CMD);
            command = Utility.mergeByteArray(command, payload);

            return sendCommand(command, ref err, sleepTime);
        }
        public static byte[] sendCommand(COMM_COMMAND_LIST CMD, byte[] payload, ref ERROR_LIST err)
        {
            return sendCommand(CMD, payload, ref err, (int)Constants.defaultSleep);
        }
        public static byte[] sendCommand(COMM_COMMAND_LIST CMD, ref ERROR_LIST err, int sleepTime)
        {
            return sendCommand(QuattroProtocol.commandToByteArray(CMD), ref err, sleepTime);
        }
        public static byte[] sendCommand(COMM_COMMAND_LIST CMD, ref ERROR_LIST err)
        {
            return sendCommand(QuattroProtocol.commandToByteArray(CMD), ref err, (int)Constants.defaultSleep);
        }

        public static byte[] sendCommand(INNCOM_COMMAND_LIST CMD, byte[] payload, ref ERROR_LIST err, int sleepTime)
        {
            if (payload == null)
            {
                err = ERROR_LIST.ERROR_INPUT_DATA_NONE;
                return null;
            }
            byte[] command = QuattroProtocol.commandToByteArray(CMD);
            command = Utility.mergeByteArray(command, payload);

            return sendCommand(command, ref err, sleepTime);

        }
        public static byte[] sendCommand(INNCOM_COMMAND_LIST CMD, byte[] payload, ref ERROR_LIST err)
        {
            return sendCommand(CMD, payload, ref err, (int)Constants.defaultSleep);
        }
        public static byte[] sendCommand(INNCOM_COMMAND_LIST CMD, ref ERROR_LIST err, int sleepTime)
        {
            return sendCommand(QuattroProtocol.commandToByteArray(CMD), ref err, sleepTime);
        }
        public static byte[] sendCommand(INNCOM_COMMAND_LIST CMD, ref ERROR_LIST err)
        {
            return sendCommand(QuattroProtocol.commandToByteArray(CMD), ref err, (int)Constants.defaultSleep);
        }

        private static void flushIOBuffer()
        {
            if (!serialPort.IsOpen)
                return;

            if (serialPort.BytesToRead > 0)
                serialPort.ReadExisting();

            serialPort.DiscardInBuffer();
            serialPort.DiscardOutBuffer();
        }
    }
}
