using BeaconTower.Protocol;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace BeaconTower.Client
{
    public class RpcServerManager
    {
        public static RpcServerManager Instance { get; } = new();
        public NodeTypeEnum NodeType { get; internal set; }
        public string NodeID { get; internal set; }
        private readonly List<BeaconTowerGrpcChannel> _channels = new List<BeaconTowerGrpcChannel>();



        private RpcServerManager()
        { }

        public void Init(BeaconTowerOptions options)
        {
            if (options == null)
            {
                throw new InvalidOperationException($"Parameter: {nameof(options)} was null.");
            }
            else if (options.HostList.Count > 1)
            {
                throw new NotSupportedException("Not supported load balance in this version.");
            }
            NodeType = options.NodeType;
            NodeID = options.NodeID;
            foreach (var item in options.HostList)
            {
                RegistHost(item);
            }
        }


        public void RegistHost(string address)
        {
            _channels.Add(new BeaconTowerGrpcChannel(address));
        }

        /// <summary>
        /// 当没有可用服务器时返回空
        /// </summary>
        /// <returns></returns>
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

        public BeaconTowerGrpcChannel GetAvailableServer()
        {
            var targetChannel = _channels.FirstOrDefault(item => item.Connected);
            if (targetChannel == null)
            {
                return null;
            }
            return targetChannel;
        }
    }
}
