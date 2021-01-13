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

        public void InitSetting(NodeTypeEnum type, string nodeID)
        {
            NodeType = type;
            NodeID = nodeID;
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
