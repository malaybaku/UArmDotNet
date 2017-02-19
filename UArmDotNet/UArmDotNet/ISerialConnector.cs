using System;

namespace Baku.UArmDotNet
{
    public interface ISerialConnector : IDisposable
    {
        int TimeoutMillisec { get; set; }


        void Connect();
        void Disconnect();

        string PortName { get; }
        bool IsConnected { get; }

        void Post(byte[] command);
        event EventHandler<SerialDataReceivedEventArgs> Received;
        /// <summary>NOTE: 明示的にDisconnectした場合も発生してほしいが、
        /// クライアント側はDisconnect関数呼んでもこれは飛んでこない可能性を想定すること</summary>
        event EventHandler Disconnected;

        //event EventHandler< Received:
        //string WaitCommandResponse(int id); 

    }

    public class SerialDataReceivedEventArgs : EventArgs
    {
        public SerialDataReceivedEventArgs(byte[] data)
        {
            Data = data;
        }
        public byte[] Data { get; private set; }
    }

    public interface ISerialResponse
    {
        int Id { get; }
        string[] Params { get; }
    }

}
