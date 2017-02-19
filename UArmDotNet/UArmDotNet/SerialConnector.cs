using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baku.UArmDotNet
{
    public class SerialConnector : ISerialConnector
    {
        public bool IsConnected => false;

        public string PortName => "";

        public int TimeoutMillisec { get; set; }

        public void Connect()
        {
        }

        public void Disconnect()
        {
        }

        public void Dispose()
        {
            Disconnect();
        }

        public void Post(byte[] command)
        {
        }

        public event EventHandler<SerialDataReceivedEventArgs> Received;

    }
}
