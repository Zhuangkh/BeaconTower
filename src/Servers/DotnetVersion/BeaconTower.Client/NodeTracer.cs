using BeaconTower.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BeaconTower.Client
{
    public class NodeTracer
    {
        internal NodeTracer()
        {
        }

        public long TraceID { get; init; }
        public string NodeID { get; init; } = string.Empty;
        public NodeTypeEnum Type { get; init; }
        public long TimeStamp { get; set; }

        public string PreviousNodeID { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string QueryString { get; set; } = string.Empty;
        public Dictionary<string, string> CustomData { get; } = new Dictionary<string, string>();

        private readonly Stack<long> _methodStack = new Stack<long>();

        private NodeActiveRequest ConstructRequestData()
        {
            var request = new NodeActiveRequest()
            {
                NodeID = NodeID,
                NodeType = Type,
                PreviousNodeID = PreviousNodeID,
                Path = Path,
                QueryString = QueryString,
                TimeStamp = TimeStamp,
                TraceID = TraceID
            };
            foreach (var item in CustomData)
            {
                request.CustomData.Add(item.Key, item.Value);
            }
            return request;
        }

        public MethodTracer CreateMethodTrace(string methodName)
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                throw new InvalidOperationException($"Parameter:{nameof(methodName)} was null or empty");
            }
            var thisMethodID = LuanNiao.Core.IDGen.GetInstance().NextId();
            var res = new MethodTracer()
            {
                NodeID = NodeID,
                TimeStamp = DateTime.Now.Ticks,
                TraceID = this.TraceID,
                MethodID = thisMethodID,
                PreMethodID = _methodStack.Count==0?0:_methodStack.Peek(),
                MethodName = methodName
            };
            _methodStack.Push(thisMethodID);
            return res;
        }

        public async void BeforeNodeActiveAsync()
        {
            var targetServer = RpcServerManager.Instance.GetAvailableServer();
            if (targetServer == null)
            {
                return;
            }
            var client = new NodeTrace.NodeTraceClient(targetServer.Channel);
            await client.BeforeNodeActiveAsync(ConstructRequestData());
        }



        public async void AfterNodeActivedAsync()
        {
            var targetServer = RpcServerManager.Instance.GetAvailableServer();
            if (targetServer == null)
            {
                return;
            }
            var client = new NodeTrace.NodeTraceClient(targetServer.Channel);
            await client.BeforeNodeActiveAsync(ConstructRequestData());
        }
    }
}
