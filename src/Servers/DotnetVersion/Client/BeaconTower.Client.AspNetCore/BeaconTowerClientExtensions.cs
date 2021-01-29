using BeaconTower.Client.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BeaconTower.Client.AspNetCore
{
    public static class BeaconTowerClientExtensions
    {

        public static IServiceCollection AddBeaconTower(this IServiceCollection service
            , Action<BeaconTowerOptions> setupAction = null
            , params AbsMessageServer[] servers)
        {
            BeaconTowerOptions options = new BeaconTowerOptions()
            {
                NodeID = Guid.NewGuid().ToString("N"),
                NodeType = NodeType.Unset
            };
            setupAction?.Invoke(options);
            ServerManager.Instance.Init(options);
            if (servers != null && servers.Length != 0)
            {
                foreach (var item in servers)
                {
                    ServerManager.Instance.RegistServer(item);
                }
            }

            service.AddScoped<NodeTracer>((provider) =>
            {
                return ServerManager.Instance.CreateNodeTracer();
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
