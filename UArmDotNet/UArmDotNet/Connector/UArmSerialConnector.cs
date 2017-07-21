using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace Baku.UArmDotNet
{
    public class UArmConnector
    {
        public UArmConnector(IRobotConnector connector)
        {
            _connector = connector;
            _connector.Received += OnDataReceived;
        }
        private readonly IRobotConnector _connector;

        public void Connect()
        {
            _connector.Connect();
        }
        public void Disconnect()
        {
            _connector.Disconnect();
        }
        public bool IsConnected
        {
            get { return _connector.IsConnected; }
        }
        


        /// <summary>コマンドを投げて単一の返信を非同期で取得する</summary>
        /// <param name="command">コマンド文字列</param>
        /// <returns>非同期の処理結果</returns>
        public IObservable<UArmResponse> Transact(string command)
        {
            int id = GenerateCommandId();

            var result = new AsyncSubject<UArmResponse>();

            Observable.FromEventPattern<UArmResponseEventArgs>(this, nameof(Received))
                .Where(ep => ep.EventArgs.Data.Id == id)
                .Select(ep => ep.EventArgs.Data)
                .FirstAsync()
                .Subscribe(result);

            PostImpl(id, command);

            return result;
        }

        /// <summary>コマンドを投げてそのまま結果は見ない</summary>
        /// <param name="command">コマンド文字列</param>
        public void Post(string command)
        {
            PostImpl(GenerateCommandId(), command);
        }

        private void PostImpl(int id, string command)
        {
            byte[] cmd = Encoding.ASCII.GetBytes(string.Format("#{0} {1}\n", id, command));
            _connector.Post(cmd);
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = Encoding.ASCII.GetString(e.Data);

            //TODO: こっから先どうしましょうね

        }

        public event EventHandler<UArmResponseEventArgs> Received;

        private void RaiseOnReceived(UArmResponse e)
        {
            if (Received != null)
            {
                Received(this, new UArmResponseEventArgs(e));
            }
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

    public class UArmResponseEventArgs : EventArgs
    {
        public UArmResponseEventArgs(UArmResponse data)
        {
            Data = data;
        }

        public UArmResponse Data { get; }
    }

}
