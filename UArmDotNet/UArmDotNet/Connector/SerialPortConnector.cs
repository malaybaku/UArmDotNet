using System;
using System.IO.Ports;

namespace Baku.UArmDotNet
{
    public class SerialPortConnector : IRobotConnector
    {
        public SerialPortConnector()
        {
            _serial = new SerialPort();
            
        }

        private readonly SerialPort _serial;

        public event EventHandler<SerialDataReceivedEventArgs> Received;
        public event EventHandler Disconnected;

        public int TimeoutMillisec
        {
            get
            {
                return _serial.ReadTimeout;
            }

            set
            {
                _serial.ReadTimeout = value;
                _serial.WriteTimeout = value;
            }
        }

        public string PortName
        {
            get { return _serial.PortName; }
            set { _serial.PortName = value; }
        }

        public bool IsConnected
        {
            get { return _serial.IsOpen; }
        }

        public void Connect()
        {
            _serial.Open();
        }

        public void Disconnect()
        {
            if (IsConnected)
            {
                _serial.Close();
                if (!IsConnected)
                {

                }
            }
        }

        public void Post(byte[] command)
        {
            if (!IsConnected)
            {
                throw new UArmException();
            }
            //TODO: タイムアウトはUArmな例外でラップする方が無難じゃないですか
            _serial.Write(command, 0, command.Length);
        }

        public void Dispose()
        {
            Disconnect();
        }

        private void OnReceived(byte[] data)
        {
            byte[] bin = new byte[data.Length];
            Array.Copy(data, bin, data.Length);
            if(Received != null)
            {
                Received(this, new SerialDataReceivedEventArgs(bin));
            }
        }

        private void StartSerialReceive()
        {
            byte[] buf = new byte[SerialBlockSizeLimit];

            Action actReceive = null;
            actReceive = () =>
            {
                _serial.BaseStream.BeginRead(buf, 0, buf.Length, ar =>
                {
                    int actualLen = _serial.BaseStream.EndRead(ar);
                    if (actualLen > 0)
                    {
                        byte[] received = new byte[actualLen];
                        Array.Copy(buf, 0, received, 0, received.Length);
                        OnReceived(received);
                    }
                    actReceive();
                }, null);
            };

        }

        private const int SerialBlockSizeLimit = 1024;
    }
}
