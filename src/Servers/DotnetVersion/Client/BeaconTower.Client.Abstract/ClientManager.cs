using System;
using System.Collections.Generic;
using System.Linq;

namespace BeaconTower.Client.Abstract
{
    public class ClientManager
    {
        public static ClientManager Instance { get; } = new();
        public NodeType NodeType { get; internal set; } = NodeType.Unset;
        public string NodeID { get; internal set; } = "DefaultNode";
        private readonly List<AbsMessageServer> _servers = new List<AbsMessageServer>();
        private ClientManager()
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

        public NodeTracer CreateNodeTracer()
        {
            return new NodeTracer()
            {
                TraceID = LuanNiao.Core.IDGen.GetInstance().NextId(),
                NodeID = NodeID,
                Type = NodeType,
                TimeStamp = DateTime.Now.Ticks
            };
        }

        public List<AbsMessageServer> GetAvailableServer()
        {
            var res = _servers.Where(item => item.Available && item.Type == ServerType.Normal).ToList();
            if (res.Count == 0)
            {
                Console.WriteLine($"BeaconTower [{nameof(ClientManager)}]:There haven't any {Enum.GetName(typeof(ServerType), ServerType.Normal)} server available.");
                res = _servers.Where(item => item.Available && item.Type == ServerType.Demotion).ToList();
            }
            if (res.Count == 0)
            {
                Console.WriteLine($"BeaconTower [{nameof(ClientManager)}]:There haven't any server available.");
            }
            return res;
        }
    }
}
