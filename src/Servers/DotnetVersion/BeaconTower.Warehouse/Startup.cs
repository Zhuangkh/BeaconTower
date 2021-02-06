using BeaconTower.TraceDB;
using BeaconTower.Warehouse.Services;
using LuanNiao.JsonConverterExtends;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;

using NodeDB = BeaconTower.TraceDB.NodeTraceDB.DBManager;
using MethodDB = BeaconTower.TraceDB.MethodTraceDB.DBManager;
using LogDB = BeaconTower.TraceDB.LogDB.DBManager;

namespace BeaconTower.Warehouse
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var rootFolder = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName, "BTraceDB");

            NodeDB.Instance.RegistNodeTraceDB(
                   "Test"
                   , rootFolder
                  );
            MethodDB.Instance.RegistNodeTraceDB("Test"
                   , rootFolder
                );
            LogDB.Instance.RegistNodeTraceDB("Test"
                   , rootFolder
                );

            NodeDB.Instance.StartServer();
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(DataBase.Instance);
            services.AddSingleton(NodeDB.Instance);
            services.AddSingleton(LogDB.Instance);
            services.AddSingleton(MethodDB.Instance);
            services.AddControllers().AddJsonOptions((opt) =>
            {
                opt.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                opt.JsonSerializerOptions.Converters.Add(new DateTime2StringConverter("yyyy-MM-dd HH:mm:ss"));
                opt.JsonSerializerOptions.Converters.Add(new Long2StringConverter());
            });
            services.AddGrpc();
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "/UI";
            });


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
                endpoints.MapGrpcService<LogService>();
                endpoints.MapControllers();
            });
#if !DEBUG
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "UI"))
                    ),
            });
#endif
            app.UseSpa(spa =>
            {
#if DEBUG
                spa.UseProxyToSpaDevelopmentServer("http://127.0.0.1:8081");
#else 
                spa.Options.SourcePath = "UI";
                spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "UI")),
                };
#endif
            });
        }
    }
}
