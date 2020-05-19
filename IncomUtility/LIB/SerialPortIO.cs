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
        retryCount = 3
    };
    public class SerialPortIO
    {
        public static SerialPort serialPort = new SerialPort();
        public static Mutex mutex = new Mutex();
        private Quattro quattro = new Quattro();
        ERROR_LIST ret;
        private void writePacket(ref byte[] sendbuffer ,ref ERROR_LIST err)
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
            Thread.Sleep(100);
        }

        private byte[] readPacket(ref ERROR_LIST err)
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
                readBuffer = null;
            }

            //mutex.ReleaseMutex();
            err = ERROR_LIST.ERROR_NONE;

            return readBuffer;
        }
        private byte[] sendCommand( byte[] payload, ref ERROR_LIST error)
        {
            if (!serialPort.IsOpen)
            {
                error = ERROR_LIST.ERROR_PORT_NOT_OPEN;
                return null;
            }
            if (payload == null)
            {
                ret = ERROR_LIST.ERROR_INPUT_DATA_NONE;
                return null;
            }
            int retryCount = (int)Constants.retryCount;
            byte[] u8TXbuffer = null;
            byte[] u8RXbuffer = null;

            while (retryCount-- > 0)
            {
                flushIOBuffer(); 

                u8TXbuffer = quattro.buildCMDPacket((byte)PACKET_CONF.COMM_SYSTEM_MFG_PC,
                    (byte)PACKET_CONF.COMM_SYSTEM_INCOM, payload, ref ret);

                writePacket(ref u8TXbuffer, ref ret);
                if (ret != ERROR_LIST.ERROR_NONE)
                    break;

                u8RXbuffer = readPacket(ref ret);
                if (ret != ERROR_LIST.ERROR_NONE)
                    break;

                ret = quattro.validateRXPacket(ref u8RXbuffer);
                if (ret == ERROR_LIST.ERROR_NONE)
                {
                    mutex.ReleaseMutex();
                    break;
                }
                mutex.ReleaseMutex();
            }
            return u8RXbuffer;
        }
        public byte[] sendCommand(COMM_COMMAND_LIST CMD, byte[] payload, ref ERROR_LIST err)
        {
            if(payload == null)
            {
                err = ERROR_LIST.ERROR_INPUT_DATA_NONE;
                return null;
            }
            byte[]command = Quattro.commandToByteArray(CMD);
            command = Quattro.mergeByteArray(command, payload);
           
            return sendCommand(command, ref err);
        }
        public byte[] sendCommand(COMM_COMMAND_LIST CMD,ref ERROR_LIST err)
        {
            return sendCommand(Quattro.commandToByteArray(CMD), ref err);
        }
        public byte[] sendCommand(INNCOM_COMMAND_LIST CMD, byte[] payload, ref ERROR_LIST err)
        {
            if (payload == null)
            {
                err = ERROR_LIST.ERROR_INPUT_DATA_NONE;
                return null;
            }
            byte[] command = Quattro.commandToByteArray(CMD);
            command = Quattro.mergeByteArray(command, payload);

            return sendCommand(command, ref err);
        }
        public byte[] sendCommand(INNCOM_COMMAND_LIST CMD, ref ERROR_LIST err)
        {
            return sendCommand(Quattro.commandToByteArray(CMD), ref err);
        }

        private void flushIOBuffer()
        {
            if (!serialPort.IsOpen)
                return;

            if (serialPort.BytesToRead > 0)
                serialPort.ReadExisting();
        }
    }
}
