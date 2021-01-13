using BeaconTower.Protocol;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BeaconTower.Client.AspNetCore
{
    public static class BeaconTowerClientExtensions
    {

        public static IServiceCollection AddBeaconTower(this IServiceCollection service
            , NodeTypeEnum nodeType
            , string nodeID
            , params string[] addressList)
        {
            if (addressList.Length > 1)
            {
                throw new System.NotSupportedException("Not supported load balance in this version");
            }
            RpcServerManager.Instance.InitSetting(nodeType, nodeID);
            foreach (var item in addressList)
            {
                var channel = Grpc.Net.Client.GrpcChannel.ForAddress(item);
                RpcServerManager.Instance.RegistHost(item);
            }


            service.AddScoped<NodeTracer>((provider) =>
           {
               return RpcServerManager.Instance.CreateNodeTracer();
           });


            return service;
        }

        public static IApplicationBuilder UseBeaconTowerNodeTracer(this IApplicationBuilder app)
        {
            if (app == null)
            {
                return app;
            }
            return app.UseMiddleware<NodeTracerMiddleware>();
        }
    }
}
