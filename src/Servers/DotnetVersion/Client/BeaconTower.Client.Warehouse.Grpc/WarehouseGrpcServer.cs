using BeaconTower.Client.Abstract;
using BeaconTower.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeaconTower.Client.Warehouse.Grpc
{
    public sealed class WarehouseGrpcServer : AbsMessageServer
    {
        private readonly List<WarehouseGrpcChannel> _channels = new List<WarehouseGrpcChannel>();

        public NodeTypeEnum NodeType { get; internal set; }
        public string NodeID { get; internal set; }
        public override bool Available { get => GetAvailableServer() != null; }

        public WarehouseGrpcServer()
        {
            Alias = nameof(WarehouseGrpcServer);
        }


        private WarehouseGrpcChannel GetAvailableServer()
        {
            var targetChannel = _channels.FirstOrDefault(item => item.Connected);
            if (targetChannel == null)
            {
                return null;
            }
            return targetChannel;
        }

        public void RegistHost(string address)
        {
            _channels.Add(new WarehouseGrpcChannel(address));
        }


        private MethodActiveReuqest ConstructRequestData(MethodTracer item)
        {
            var request = new MethodActiveReuqest()
            {
                NodeID = item.NodeID,
                MethodID = item.MethodID,
                MethodName = item.MethodName,
                PreMethodID = item.PreMethodID,
                TimeStamp = item.TimeStamp,
                TraceID = item.TraceID
            };
            foreach (var data in item.CustomData)
            {
                request.CustomData.Add(data.Key, data.Value);
            }
            return request;
        }

        private NodeActiveRequest ConstructRequestData(NodeTracer item)
        {
            var request = new NodeActiveRequest()
            {
                NodeID = item.NodeID,
                NodeType = (NodeTypeEnum)item.Type,
                PreviousNodeID = item.PreviousNodeID,
                Path = item.Path,
                QueryString = item.QueryString,
                TimeStamp = item.TimeStamp,
                TraceID = item.TraceID
            };
            foreach (var data in item.CustomData)
            {
                request.CustomData.Add(data.Key, data.Value);
            }
            return request;
        }


        public override async Task BeforMethodInvokeAsync(MethodTracer info)
        {
            if (info == null)
            {
                return;
            }
            var targetServer = GetAvailableServer();
            if (targetServer == null)
            {
                return;
            }
            var client = new MethodTrace.MethodTraceClient(targetServer.Channel);
            await client.BeforMethodInvokeAsync(ConstructRequestData(info), deadline: DateTime.UtcNow.AddSeconds(1));
        }

        public override async Task AfterMethodInvokedAsync(MethodTracer info)
        {
            if (info == null)
            {
                return;
            }
            var targetServer = GetAvailableServer();
            if (targetServer == null)
            {
                return;
            }
            var client = new MethodTrace.MethodTraceClient(targetServer.Channel);
            await client.AfterMethodInvokedAsync(ConstructRequestData(info), deadline: DateTime.UtcNow.AddSeconds(1));
        }

        public override async Task BeforeNodeActiveAsync(NodeTracer info)
        {
            if (info == null)
            {
                return;
            }
            var targetServer = GetAvailableServer();
            if (targetServer == null)
            {
                return;
            }
            var client = new NodeTrace.NodeTraceClient(targetServer.Channel);
            await client.BeforeNodeActiveAsync(ConstructRequestData(info), deadline: DateTime.UtcNow.AddSeconds(1));
        }

        public override async Task AfterNodeActivedAsync(NodeTracer info)
        {
            if (info == null)
            {
                return;
            }
            var targetServer = GetAvailableServer();
            if (targetServer == null)
            {
                return;
            }
            var client = new NodeTrace.NodeTraceClient(targetServer.Channel);
            await client.AfterNodeActivedAsync(ConstructRequestData(info), deadline: DateTime.UtcNow.AddSeconds(1));
        }
    }
}
