using System.Threading.Tasks;
using BeaconTower.Protocol;
using Grpc.Core;

namespace BeaconTower.Warehouse.Services
{
    public class MethodTraceService : MethodTrace.MethodTraceBase
    {
        public override Task<NullResponse> AfterMethodInvoked(AfterMethodInvokedReuqest request, ServerCallContext context)
        {
            return base.AfterMethodInvoked(request, context);
        }

        public override Task<NullResponse> BeforMethodInvoke(BeforMethodInvokeReuqest request, ServerCallContext context)
        {
            return base.BeforMethodInvoke(request, context);
        }
    }
}
