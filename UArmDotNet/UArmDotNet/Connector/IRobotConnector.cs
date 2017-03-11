using System;

namespace Baku.UArmDotNet
{
    public interface IRobotConnector : IDisposable
    {
        int TimeoutMillisec { get; set; }


        void Connect();
        void Disconnect();

        string PortName { get; }
        bool IsConnected { get; }

        void Post(byte[] command);
        event EventHandler<SerialDataReceivedEventArgs> Received;
        event EventHandler Disconnected;
    }

    public class SerialDataReceivedEventArgs : EventArgs
    {
        public SerialDataReceivedEventArgs(byte[] data)
        {
            Data = data;
        }
        public byte[] Data { get; private set; }
    }

    public class SerialConnectionErrorEventArgs : EventArgs
    {
        public SerialConnectionErrorEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }
    


}
