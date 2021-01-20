using BeaconTower.TraceDB.NodeTraceDB;
using BeaconTower.Warehouse.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;

namespace BeaconTower.Warehouse
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            DBManager.Instance.RegistNodeTraceDB(
                   "Test"
                   , (new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName)
                  ).StartServer();
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(DBManager.Instance);
            services.AddControllers();
            services.AddGrpc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<CommonService>();
                endpoints.MapGrpcService<MethodTraceService>();
                endpoints.MapGrpcService<NodeTraceService>();
                endpoints.MapControllers();
            });
        }
    }
}
