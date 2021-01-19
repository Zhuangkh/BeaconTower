using BeaconTower.Protocol;
using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace BeaconTower.Warehouse.Services
{
    public class NodeTraceService : NodeTrace.NodeTraceBase
    {
        public override Task<NullResponse> AfterNodeActived(NodeActiveRequest request, ServerCallContext context)
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"AfterNodeActived TraceID:{request.TraceID}");
                return new NullResponse();
            });
        }
        public override Task<NullResponse> BeforeNodeActive(NodeActiveRequest request, ServerCallContext context)
        {

            return Task.Run(() =>
            {
                Console.WriteLine($"BeforeNodeActive TraceID:{request.TraceID}");
                return new NullResponse();
            });
        }
    }
}
