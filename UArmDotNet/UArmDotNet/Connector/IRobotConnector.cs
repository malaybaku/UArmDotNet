using System;

namespace Baku.UArmDotNet
{
    /// <summary>Define the interface to communicate with the robot</summary>
    public interface IRobotConnector : IDisposable
    {
        int TimeoutMillisec { get; set; }

        //TODO: 例外を決めてね

        /// <summary>Connect to the robot.</summary>
        void Connect();
        /// <summary>Disconnect from the robot.</summary>
        void Disconnect();

        /// <summary>Get whether the connection is established or not</summary>
        bool IsConnected { get; }

        /// <summary>
        /// Post binary data (basically ASCII string is expected) to the robot.
        /// </summary>
        /// <param name="command">data to send</param>
        void Post(byte[] command);
        /// <summary>
        /// Fires when received binary data from the connected robot.
        /// </summary>
        event EventHandler<SerialDataReceivedEventArgs> Received;
        /// <summary>
        /// Fires when <see cref="IsConnected"/> property changed from <see cref="true"/> to <see cref="false"/>.
        /// </summary>
        event EventHandler Disconnected;
    }

    /// <summary>
    /// Event data including the received binary message from the robot
    /// </summary>
    public class SerialDataReceivedEventArgs : EventArgs
    {
        public SerialDataReceivedEventArgs(byte[] data)
        {
            Data = data;
        }
        public byte[] Data { get; private set; }
    }

}
