using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace Baku.UArmDotNet
{
    public class UArmConnector
    {
        public UArmConnector(ISerialConnector serial)
        {
            _serial = serial;
            _serial.Received += OnDataReceived;
            

        }
        private readonly ISerialConnector _serial;

        public void Connect()
        {
            _serial.Connect();
        }
        public void Disconnect()
        {
            _serial.Disconnect();
        }
        public bool IsConnected
        {
            get { return _serial.IsConnected; }
        }
        


        /// <summary>コマンドを投げて単一の返信を非同期で取得する</summary>
        /// <param name="command">コマンド文字列</param>
        /// <returns>非同期の処理結果</returns>
        public IObservable<UArmResponse> Transact(string command)
        {
            int id = GenerateCommandId();

            var result = new AsyncSubject<UArmResponse>();

            Observable.FromEventPattern<UArmSerialDataReceivedEventArgs>(this, nameof(Received))
                .Where(ep => ep.EventArgs.Data.Id == id)
                .Select(ep => ep.EventArgs.Data)
                .FirstAsync()
                .Subscribe(result);

            PostToSerial(id, command);

            return result;
        }

        /// <summary>コマンドを投げてそのまま結果は見ない</summary>
        /// <param name="command">コマンド文字列</param>
        public void Post(string command)
        {
            PostToSerial(GenerateCommandId(), command);
        }

        private void PostToSerial(int id, string command)
        {
            byte[] cmd = Encoding.ASCII.GetBytes(string.Format("#{0} {1}\n", id, command));
            _serial.Post(cmd);
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //TODO: エンコードエラー対策(こっちは不要かもしれないが)
            string data = Encoding.ASCII.GetString(e.Data);


        }

        public event EventHandler<UArmSerialDataReceivedEventArgs> Received;

        private void RaiseOnReceived(UArmResponse e)
            => Received?.Invoke(this, new UArmSerialDataReceivedEventArgs(e));

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

    public class UArmSerialDataReceivedEventArgs : EventArgs
    {
        public UArmSerialDataReceivedEventArgs(UArmResponse data)
        {
            Data = data;
        }

        public UArmResponse Data { get; }
    }

}
