using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace IncomUtility
{
    enum Constants
    {
        retryCount = 3,
        defaultSleep = 100
    };
     public static  class SerialPortIO
    {
         public static SerialPort serialPort = new SerialPort();
         public static Mutex mutex = new Mutex();
         private static Quattro quattro = new Quattro();

          public static bool isPortOpen()
        {
            if (serialPort.IsOpen)
                return true;

            string[] names= SerialPort.GetPortNames();
            
            if (names.Length == 0)
                return false;

            mutex.WaitOne();
            serialPort.PortName = names[0];

            try
            {
               serialPort.Open();
            }
            catch
            {
                mutex.ReleaseMutex();
                MessageBox.Show("Connecting was failed");
                return false;
            }

            mutex.ReleaseMutex();
            if (serialPort.IsOpen)
                return true;

            return false;
        }
         private static void writePacket(ref byte[] sendbuffer ,ref ERROR_LIST err)
        {
            
            if (!serialPort.IsOpen)
            {
                err = ERROR_LIST.ERROR_PORT_NOT_OPEN;
                return;
            }
            if (sendbuffer == null)
            {
                err = ERROR_LIST.ERROR_INPUT_DATA_NONE;
                return ;
            }
            mutex.WaitOne();
            serialPort.Write(sendbuffer, 0, sendbuffer.Length);
            err = ERROR_LIST.ERROR_NONE;
        }

         private static byte[] readPacket(ref ERROR_LIST err)
        {
            if (!serialPort.IsOpen)
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
                return  null;
            }

            //mutex.ReleaseMutex();
            err = ERROR_LIST.ERROR_NONE;

            return readBuffer;
        }
         private static byte[] sendCommand( byte[] payload, ref ERROR_LIST error ,int sleepTime)
        {
            if (!serialPort.IsOpen)
            {
                error = ERROR_LIST.ERROR_PORT_NOT_OPEN;
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

            while (retryCount-- > 0)
            {
                flushIOBuffer(); 

                u8TXbuffer = quattro.buildCMDPacket((byte)PACKET_CONF.COMM_SYSTEM_MFG_PC,
                    (byte)PACKET_CONF.COMM_SYSTEM_INCOM, payload, ref error);
                if (error != ERROR_LIST.ERROR_NONE)
                    break;

                writePacket(ref u8TXbuffer, ref error);
                if (error != ERROR_LIST.ERROR_NONE)
                    break;

                Thread.Sleep(sleepTime);

                u8RXbuffer = readPacket(ref error);
                if (error != ERROR_LIST.ERROR_NONE)
                    break;

                error = quattro.validateRXPacket(ref u8RXbuffer);
                if (error == ERROR_LIST.ERROR_NONE)
                {
                    mutex.ReleaseMutex();
                    break;
                }
                mutex.ReleaseMutex();
            }
            return u8RXbuffer;
        }
         public static byte[] sendCommand(COMM_COMMAND_LIST CMD, byte[] payload, ref ERROR_LIST err,int sleepTime)
        {
            if(payload == null)
            {
                err = ERROR_LIST.ERROR_INPUT_DATA_NONE;
                return null;
            }
            byte[]command = Quattro.commandToByteArray(CMD);
            command = Quattro.mergeByteArray(command, payload);
            
            return sendCommand(command, ref err, sleepTime);
        }
         public static byte[] sendCommand(COMM_COMMAND_LIST CMD, byte[] payload, ref ERROR_LIST err)
        {
            return sendCommand(CMD, payload, ref err, (int)Constants.defaultSleep);
        }
         public static byte[] sendCommand(COMM_COMMAND_LIST CMD,ref ERROR_LIST err, int sleepTime )
        {
            return sendCommand(Quattro.commandToByteArray(CMD), ref err, sleepTime);
        }
         public static byte[] sendCommand(COMM_COMMAND_LIST CMD, ref ERROR_LIST err)
        {
            return sendCommand(Quattro.commandToByteArray(CMD), ref err, (int)Constants.defaultSleep);
        }

         public static byte[] sendCommand(INNCOM_COMMAND_LIST CMD, byte[] payload, ref ERROR_LIST err, int sleepTime)
        {
            if (payload == null)
            {
                err = ERROR_LIST.ERROR_INPUT_DATA_NONE;
                return null;
            }
            byte[] command = Quattro.commandToByteArray(CMD);
            command = Quattro.mergeByteArray(command, payload);
            
            return sendCommand(command, ref err, sleepTime);

        }
         public static byte[] sendCommand(INNCOM_COMMAND_LIST CMD, byte[] payload, ref ERROR_LIST err)
        {
            return sendCommand(CMD,payload, ref err, (int)Constants.defaultSleep);
        }
         public static byte[] sendCommand(INNCOM_COMMAND_LIST CMD, ref ERROR_LIST err, int sleepTime)
        {
           return sendCommand(Quattro.commandToByteArray(CMD), ref err, sleepTime);
        }
         public static byte[] sendCommand(INNCOM_COMMAND_LIST CMD, ref ERROR_LIST err)
        {
            return sendCommand(Quattro.commandToByteArray(CMD), ref err, (int)Constants.defaultSleep);
        }

         private static void flushIOBuffer()
        {
            if (!serialPort.IsOpen)
                return;

            if (serialPort.BytesToRead > 0)
                serialPort.ReadExisting();
        }
    }
}
