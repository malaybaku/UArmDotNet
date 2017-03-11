using System;

namespace Baku.UArmDotNet.Simulator
{
    public class SimulatorConnector : IRobotConnector
    {
        public SimulatorConnector(UArmCommandReceiver receiver)
        {
            _receiver = receiver;
        }
        private readonly UArmCommandReceiver _receiver;

        public bool IsConnected { get; private set; }

        public string PortName
        {
            get { return "SIMULATOR"; }
        }

        public int TimeoutMillisec { get; set; }

        public event EventHandler Disconnected;
        public event EventHandler<SerialDataReceivedEventArgs> Received;

        public void Connect()
        {
            IsConnected = true;
        }

        public void Disconnect()
        {
            if (IsConnected)
            {
                IsConnected = false;
                Disconnected?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Dispose()
        {
            Disconnect();
        }

        public void Post(byte[] command)
        {
            throw new NotImplementedException();
        }
    }
}
