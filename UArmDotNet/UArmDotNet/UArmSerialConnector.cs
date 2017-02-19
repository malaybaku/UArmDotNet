using System;
using System.Text;

namespace Baku.UArmDotNet
{
    public class UArmSerialConnector
    {
        public UArmSerialConnector(ISerialConnector serial)
        {
            _serial = serial;
            _serial.Received += OnDataReceived;


        }
        private readonly ISerialConnector _serial;

        public string Transact(string command)
        {
            throw new NotImplementedException();
        }

        public int Post(string command)
        {
            int id = GenerateCommandId();

            //TODO: Encodeエラー対策
            byte[] cmd = Encoding.ASCII.GetBytes(string.Format("#{0} {1}\n", id, command));
            _serial.Post(cmd);

            //TODO: キューかなんかにID放り込む必要ある？いやないか？
            return id;
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //TODO: エンコードエラー対策(こっちは不要かもしれないが)
            string data = Encoding.ASCII.GetString(e.Data);


        }


        private static int _commandId = 0;
        private static readonly object _generateIdLock = new object();
        private static int GenerateCommandId()
        {
            lock (_generateIdLock)
            {
                _commandId++;
                if (_commandId > 65535) _commandId = 1;
                return _commandId;
            }
        }

    }
}
