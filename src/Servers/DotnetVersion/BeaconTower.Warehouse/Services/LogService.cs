using System;
using System.Threading.Tasks;
using BeaconTower.Protocol;
using Grpc.Core;

namespace BeaconTower.Warehouse.Services
{
    public class LogService : Log.LogBase
    {
        public override Task<NullResponse> Trace(LogRequest request, ServerCallContext context)
        {
            return base.Trace(request, context);
        }
        public override Task<NullResponse> Info(LogRequest request, ServerCallContext context)
        {
            return base.Info(request, context);
        }
        public override Task<NullResponse> Debug(LogRequest request, ServerCallContext context)
        {
            return base.Debug(request, context);
        }
        public override Task<NullResponse> Warning(LogRequest request, ServerCallContext context)
        {
            return base.Warning(request, context);
        }
        public override Task<NullResponse> Error(LogRequest request, ServerCallContext context)
        {
            return base.Error(request, context);
        }
        public override Task<NullResponse> Panic(LogRequest request, ServerCallContext context)
        {
            return base.Panic(request, context);
        }
    }
}
