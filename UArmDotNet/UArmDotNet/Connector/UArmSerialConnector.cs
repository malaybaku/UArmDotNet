using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Baku.UArmDotNet
{
    public class UArmConnector
    {
        public UArmConnector(SerialRobotConnector connector)
        {
            SerialConnector = connector;
            SerialConnector.Received += OnDataReceived;
            SerialConnector.Disconnected += OnDisconnected;
        }
        public UArmConnector() : this(new SerialRobotConnector())
        {
        }

        public SerialRobotConnector SerialConnector { get; }
        private readonly Dictionary<int, CancellationTokenSource> _pendingCommandIds = new Dictionary<int, CancellationTokenSource>();
        private readonly Dictionary<int, UArmResponse> _responses = new Dictionary<int, UArmResponse>();

        /// <summary>
        /// Get or set the minimum post interval [ms], default is 100.
        /// If set 0 or minus value, send post with no minimum interval (but not suited for uArm)
        /// </summary>
        public int PostMinIntervalMillisec { get; set; } = 100;

        public void Connect() => SerialConnector.Connect();
        public void Disconnect() => SerialConnector.Disconnect();
        public bool IsConnected => SerialConnector.IsConnected;        

        /// <summary>コマンドを投げて単一の返信を非同期で取得する</summary>
        /// <param name="command">コマンド文字列</param>
        /// <returns>非同期の処理結果</returns>
        public async Task<UArmResponse> Transact(string command)
        {
            int id = GenerateCommandId();
            var cts = new CancellationTokenSource();
            _pendingCommandIds[id] = cts;

            var t = Task.Delay(Timeout.InfiniteTimeSpan, cts.Token);

            PostImpl(id, command);

            await t;

            if (_responses.ContainsKey(id))
            {
                var result = _responses[id];
                _responses.Remove(id);
                return result;
            }
            else
            {
                throw new UArmException();
            }
        }

        /// <summary>コマンドを投げてそのまま結果は見ない</summary>
        /// <param name="command">コマンド文字列</param>
        public void Post(string command)
            => PostImpl(GenerateCommandId(), command);

        public event EventHandler<UArmResponseEventArgs> Received;

        private void PostImpl(int id, string command)
        {
            byte[] cmd = Encoding.ASCII.GetBytes($"#{id} {command}\n");
            SerialConnector.Post(cmd);
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string[] data = Encoding.ASCII
                .GetString(e.Data)
                .TrimEnd('\n')
                .Split(' ');

            int id = -1;
            if (data.Length > 1 && int.TryParse(data[0], out id))
            {
                //正常受信
                //返信待ちあったらマジメにやっていきます
                var args = new string[data.Length - 1];
                Array.Copy(data, 1, args, 0, args.Length);
                var response = new UArmResponse(id, args);

                //返信待ちしてたら待ち完了させる
                if (_pendingCommandIds.ContainsKey(id))
                {
                    _responses[id] = response;
                    _pendingCommandIds[id].Cancel();
                }

                Received?.Invoke(this, new UArmResponseEventArgs(response));
            }
            else
            {
                //受信段階で何かしらコケた
                throw new UArmException();
            }            
        }
        private void OnDisconnected(object sender, EventArgs e)
        {
            _responses.Clear();
            foreach (var cts in _pendingCommandIds.Values)
            {
                cts.Cancel();
            }
            _pendingCommandIds.Clear();
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
