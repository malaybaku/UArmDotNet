using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly Dictionary<int, CancellationTokenSource> _pendingCommandIds = new Dictionary<int, CancellationTokenSource>();
        private readonly Dictionary<int, UArmResponse> _responses = new Dictionary<int, UArmResponse>();

        public SerialRobotConnector SerialConnector { get; }

        //RawData events are for debugging.
        public event EventHandler<UArmRawMessageEventArgs> ReceivedRawData;
        public event EventHandler<UArmRawMessageEventArgs> SendRawData;

        public event EventHandler<UArmResponseEventArgs> ReceivedResponse;
        public event EventHandler<UArmEventMessageEventArgs> ReceivedEvent;

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

            try
            {
                await t;
            }
            catch(TaskCanceledException)
            {
                //何もしない: キャンセルは正常系
            }

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

        public void PostWithoutId(string command)
        {
            SerialConnector.Post(Encoding.ASCII.GetBytes(command + "\n"));
        }

        private void PostImpl(int id, string command)
        {
            string commandWithId = $"#{id} {command}";
            byte[] cmd = Encoding.ASCII.GetBytes(commandWithId + "\n");
            SerialConnector.Post(cmd);
            SendRawData?.Invoke(this, new UArmRawMessageEventArgs(commandWithId));
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //エラー後の通信などで2つ分のレスポンスがまとめて飛んでくることがあるので、区切って処理する
            foreach (string line in Encoding.ASCII
                .GetString(e.Data)
                .Split('\n')
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => s.Trim('\r', '\n'))
                )
            {
                ReceivedRawData?.Invoke(this, new UArmRawMessageEventArgs(line));
                OnLineReceived(line);
            }
        }
        private void OnLineReceived(string line)
        {
            string[] data = line.Split(' ');

            //begin with "@": treat as event message
            int eventId = 0;
            if (data.Length > 0 &&
                data[0].Length > 1 &&
                data[0][0] == '@' &&
                int.TryParse(data[0].Substring(1), out eventId)
                )
            {
                var args = new string[data.Length - 1];
                Array.Copy(data, 1, args, 0, args.Length);
                var eventMessage = new UArmEventMessage(eventId, args);
                ReceivedEvent?.Invoke(this, new UArmEventMessageEventArgs(eventMessage));

                return;
            }

            //begin with "$": treas as response
            int id = -1;
            if (data.Length > 1 &&
                data[0].Length > 1 &&
                data[0][0] == '$' &&
                int.TryParse(data[0].Substring(1), out id))
            {
                var args = new string[data.Length - 1];
                Array.Copy(data, 1, args, 0, args.Length);
                var response = new UArmResponse(id, args);

                //Terminate waiting process if pending exists
                if (_pendingCommandIds.ContainsKey(id))
                {
                    _responses[id] = response;
                    _pendingCommandIds[id].Cancel();
                }

                ReceivedResponse?.Invoke(this, new UArmResponseEventArgs(response));

                return;
            }


            //Unknown data
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

    public class UArmRawMessageEventArgs : EventArgs
    {
        public UArmRawMessageEventArgs(string rawData)
        {
            RawData = rawData;
        }

        public string RawData { get; }
    }


    public class UArmResponseEventArgs : EventArgs
    {
        public UArmResponseEventArgs(UArmResponse response)
        {
            Response = response;
        }

        public UArmResponse Response { get; }
    }

    public class UArmEventMessageEventArgs : EventArgs
    {
        public UArmEventMessageEventArgs(UArmEventMessage eventMessage)
        {
            EventMessage = eventMessage;
        }
        public UArmEventMessage EventMessage { get; }
    }

}
