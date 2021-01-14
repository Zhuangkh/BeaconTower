using BeaconTower.Protocol;
using Grpc.Net.Client;
using System.Collections.Generic;
using System;

namespace BeaconTower.Client
{
    public sealed class MethodTracer : IDisposable
    {
        internal MethodTracer()
        {
        } 

        public void Dispose()
        {
            TimeStamp = DateTime.Now.Ticks;            
            this.AfterMethodInvokedAsync();            
        }
        internal long TraceID { get; init; }
        internal string NodeID { get; init; } = string.Empty;
        public long PreMethodID { get; init; }
        public long MethodID { get; init; }
        internal long TimeStamp { get; set; }
        public string MethodName { get; init; }
        public Dictionary<string, string> CustomData { get; } = new Dictionary<string, string>();

        private MethodActiveReuqest ConstructRequestData()
        {
            var request = new MethodActiveReuqest()
            {
                NodeID = NodeID,
                MethodID = MethodID,
                MethodName = MethodName,
                PreMethodID = PreMethodID,
                TimeStamp = TimeStamp,
                TraceID = TraceID
            };
            foreach (var item in CustomData)
            {
                request.CustomData.Add(item.Key, item.Value);
            }
            return request;
        }

        public async void BeforMethodInvokeAsync()
        {
            var targetServer = RpcServerManager.Instance.GetAvailableServer();
            if (targetServer == null)
            {
                return;
            }
            var client = new MethodTrace.MethodTraceClient(targetServer.Channel);
            await client.BeforMethodInvokeAsync(ConstructRequestData());
        }



        public async void AfterMethodInvokedAsync()
        {
            var targetServer = RpcServerManager.Instance.GetAvailableServer();
            if (targetServer == null)
            {
                return;
            }
            var client = new MethodTrace.MethodTraceClient(targetServer.Channel);
            await client.AfterMethodInvokedAsync(ConstructRequestData());
        }

    }
}
