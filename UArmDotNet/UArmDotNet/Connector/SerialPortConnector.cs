using System;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace Baku.UArmDotNet
{
    /// <summary>Wrapper of the <see cref="SerialPort"/> to connect safely</summary>
    public class SerialRobotConnector
    {
        private readonly SerialPort _serial = new SerialPort();

        public event EventHandler<SerialDataLineReceivedEventArgs> Received;
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

        private void OnReceived(string line)
            => Received?.Invoke(this, new SerialDataLineReceivedEventArgs(line));

        private void StartSerialReceive()
        {
            Action actReceive = null;
            StringBuilder stringBuf = new StringBuilder();
            actReceive = async () =>
            {
                byte[] buf = new byte[SerialBlockSizeLimit];
                try
                {
                    int len = await _serial.BaseStream.ReadAsync(buf, 0, buf.Length);
                    if (len == 0)
                    {
                        //接続断っぽいので打ち切る
                        return;
                    }

                    //とりあえず積み上げ
                    stringBuf.Append(Encoding.ASCII.GetString(buf, 0, len));
                }
                catch (IOException)
                {
                    //スレッドの停止などで終了した場合も打ち切り
                    return;
                }

                //バッファから改行で千切って投げる
                while(true)
                {
                    string current = stringBuf.ToString();
                    if (!current.Contains('\n'))
                    {
                        break;
                    }

                    string line = current.Split('\n')[0];
                    stringBuf.Remove(0, line.Length + 1);
                    OnReceived(line);
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

    public class SerialDataLineReceivedEventArgs : EventArgs
    {
        public SerialDataLineReceivedEventArgs(string line)
        {
            Line = line;
        }

        public string Line { get; }
    }
}
