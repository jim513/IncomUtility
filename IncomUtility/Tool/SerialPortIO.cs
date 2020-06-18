using IncomUtility.APP;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.DataVisualization.Charting;

namespace IncomUtility
{
    enum Constants
    {
        retryCount = 3,
        defaultSleep = 100,
        secondTomilisecond = 1000
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
    
        public static SerialPort serialPort = new SerialPort();
        public static Mutex mutex = new Mutex();

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
                return false;
            }
            if (serialPort.IsOpen)
            {
                return true;
            }

            return false;
        }
        public static void writePacket(ref byte[] sendbuffer, ref ERROR_LIST err)
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

        public static byte[] readPacket(ref ERROR_LIST err)
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
    
        public static void flushIOBuffer()
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
