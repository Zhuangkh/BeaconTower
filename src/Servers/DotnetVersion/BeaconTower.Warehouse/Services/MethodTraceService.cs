using System;
using System.Threading.Tasks;
using BeaconTower.Protocol;
using Grpc.Core;

namespace BeaconTower.Warehouse.Services
{
    public class MethodTraceService : MethodTrace.MethodTraceBase
    {
        public override Task<NullResponse> AfterMethodInvoked(MethodActiveReuqest request, ServerCallContext context)
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"AfterMethodInvoked MethodName:{request.MethodName} {request.MethodID}  {request.PreMethodID}");
                return new NullResponse();
            });
        }
        public override Task<NullResponse> BeforMethodInvoke(MethodActiveReuqest request, ServerCallContext context)
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"BeforMethodInvoke MethodName:{request.MethodName} {request.MethodID}  {request.PreMethodID}");
                return new NullResponse();
            });
        }
    }
}
