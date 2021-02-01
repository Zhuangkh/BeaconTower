using System;
using System.Collections.Generic;
using System.Linq;

namespace BeaconTower.Client.Abstract
{
    /// <summary>
    /// use to manage all Server
    /// </summary>
    public class ServerManager
    {
        public static ServerManager Instance { get; } = new();
        public NodeType NodeType { get; internal set; } = NodeType.Unset;
        public string NodeID { get; internal set; } = "DefaultNode";
        private readonly List<AbsMessageServer> _servers = new List<AbsMessageServer>();
        private ServerManager()
        { }

        public void Init(BeaconTowerOptions options)
        {
            if (options == null)
            {
                throw new InvalidOperationException($"Parameter: {nameof(options)} was null.");
            }
            else if (!Enum.IsDefined(typeof(NodeType), options.NodeType))
            {
                throw new InvalidOperationException($"Parameter: {nameof(NodeType)}'s value was invalid.");
            }
            NodeType = options.NodeType;
            NodeID = options.NodeID;
        }


        public void RegistServer(AbsMessageServer server)
        {
            _servers.Add(server);
        }

        public NodeTracer CreateNodeTracer(long? traceID = null, string previewNodeID = null)
        {
            return new NodeTracer()
            {
                TraceID = traceID ?? LuanNiao.Core.IDGen.GetInstance().NextId(),
                NodeID = NodeID,
                Type = NodeType,
                PreviousNodeID = previewNodeID,
                TimeStamp = DateTime.Now.Ticks
            };
        }

        public List<AbsMessageServer> GetAvailableServer()
        {
            var res = _servers.Where(item => item.Available && item.Type == ServerType.Normal).ToList();
            if (res.Count == 0)
            {
                Console.WriteLine($"BeaconTower [{nameof(ServerManager)}]:There haven't any {Enum.GetName(typeof(ServerType), ServerType.Normal)} server available.");
                res = _servers.Where(item => item.Available && item.Type == ServerType.Demotion).ToList();
            }
            if (res.Count == 0)
            {
                Console.WriteLine($"BeaconTower [{nameof(ServerManager)}]:There haven't any server available.");
            }
            return res;
        }
    }
}
