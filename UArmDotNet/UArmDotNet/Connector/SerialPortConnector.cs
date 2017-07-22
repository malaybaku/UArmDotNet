using System;
using System.IO.Ports;
using System.Linq;

namespace Baku.UArmDotNet
{
    /// <summary>Wrapper of the <see cref="System.IO.Ports.SerialPort"/> to connect safely</summary>
    public class SerialRobotConnector
    {
        private readonly SerialPort _serial = new SerialPort();

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

        public int BaudRate
        {
            get { return _serial.BaudRate; }
            set { _serial.BaudRate = value; }
        }

        public bool IsConnected => _serial.IsOpen; 

        public void Connect()
        {
            if (IsConnected) { return; }

            _serial.Open();
            if (IsConnected)
            {
                StartSerialReceive();
            }
        }

        public void Disconnect()
        {
            if (IsConnected)
            {
                _serial.Close();
                Disconnected?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Post(byte[] command)
        {
            if (!IsConnected)
            {
                throw new UArmException();
            }
            _serial.Write(command, 0, command.Length);
        }

        public void Dispose() => Disconnect();

        private void OnReceived(byte[] data)
        {
            byte[] bin = new byte[data.Length];
            Array.Copy(data, bin, data.Length);
            Received?.Invoke(this, new SerialDataReceivedEventArgs(bin));
        }

        private void StartSerialReceive()
        {
            Action actReceive = null;
            actReceive = async () =>
            {
                byte[] buf = new byte[SerialBlockSizeLimit];
                int totalDataLen = 0;
                while (true)
                {
                    int len = await _serial.BaseStream.ReadAsync(buf, totalDataLen, buf.Length - totalDataLen);
                    if (len > 0)
                    {
                        totalDataLen += len;
                    }
                    else
                    {
                        //接続断っぽいので打ち切り
                        break;
                    }
                    //改行文字で区切るというuArmのルールをここに入れる。設計上あんまりよくないが
                    if (buf.Contains((byte)'\n'))
                    {
                        break;
                    }
                }

                if (totalDataLen > 0)
                {
                    byte[] received = new byte[totalDataLen];
                    Array.Copy(buf, 0, received, 0, received.Length);
                    OnReceived(received);
                }

                //NOTE: 少なくとも接続切れた場合はもう諦める(リーク防止を重視)
                if (IsConnected)
                {
                    actReceive();
                }
            };
            actReceive();

        }

        private const int SerialBlockSizeLimit = 1024;
    }

    public class SerialDataReceivedEventArgs : EventArgs
    {
        public SerialDataReceivedEventArgs(byte[] data)
        {
            Data = new byte[data.Length];
            Array.Copy(data, Data, data.Length);
        }

        public byte[] Data { get; }
    }
}
