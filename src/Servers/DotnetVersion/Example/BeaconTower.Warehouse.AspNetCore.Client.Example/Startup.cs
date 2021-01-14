using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeaconTower.Client.AspNetCore;
using BeaconTower.Client;
using BeaconTower.Client.Abstract;
using BeaconTower.Client.Warehouse.Grpc;
using BeaconTower.Client.Console;

namespace BeaconTower.Warehouse.AspNetCore.Client.Example
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            WarehouseGrpcServer grpcServer = new();
            ConsoleServer console = new() { Type = ServerType.Demotion };
            grpcServer.RegistHost("http://127.0.0.1:5000");
            services.AddBeaconTower(c =>
            {
                c.NodeID = "Test Node";
                c.NodeType = NodeType.WebServer;
            }, grpcServer, console);
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BeaconTower.Warehouse.AspNetCore.Client.Example", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BeaconTower.Warehouse.AspNetCore.Client.Example v1"));
            }
            app.UseBeaconTowerNodeTracer();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
