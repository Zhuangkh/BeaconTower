using System;
using System.Threading.Tasks;
using BeaconTower.Protocol;
using Grpc.Core;

namespace BeaconTower.Warehouse.Services
{
    public class CommonService : CommonRequest.CommonRequestBase
    {
        public override Task<GetStateResponse> GetState(GetStateRequest request, ServerCallContext context)
        {
            return Task.Run(() =>
            {
                return new GetStateResponse()
                {
                    ServerWillShutdown = false,
                    TimeStamp = DateTime.Now.Ticks
                };
            });
        }
    }
}
