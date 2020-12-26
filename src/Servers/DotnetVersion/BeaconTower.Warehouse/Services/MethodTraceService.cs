using System.Threading.Tasks;
using BeaconTower.Protocol;
using Grpc.Core;

namespace BeaconTower.Warehouse.Services
{
    public class NodeTraceService : NodeTrace.NodeTraceBase
    {
        public override Task<NullResponse> AfterNodeActived(AfterNodeActivedRequest request, ServerCallContext context)
        {
            return base.AfterNodeActived(request, context);
        }

        public override Task<NullResponse> BeforeNodeActive(BeforeNodeActiveRequest request, ServerCallContext context)
        {
            return base.BeforeNodeActive(request, context);
        }
    }
}
