using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BeaconTower.Warehouse
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var grpcPort = 50000;
            var webapiPort = 60000;
            if (args != null)
            {
                foreach (var item in args)
                {
                    if (item.StartsWith("-grpc=")
                        && int.TryParse(item.Replace("-grpc=", ""), out var customGrpcPort)
                        && customGrpcPort < 65535
                        && customGrpcPort > 100
                        )
                    {
                        grpcPort = customGrpcPort;
                    }
                    else if (item.StartsWith("-webapi=")
                        && int.TryParse(item.Replace("-webapi=", ""), out var customWebApiPort)
                        && customWebApiPort < 65535
                        && customWebApiPort > 100
                        )
                    {
                        webapiPort = customWebApiPort;
                    }
                }
            }
            var webhost = new WebHostBuilder();
            webhost.UseKestrel()
            //.ConfigureLogging(builder => builder.AddConsole())
            .ConfigureKestrel(options =>
            {
                options.ListenLocalhost(grpcPort, o => o.Protocols =
                    HttpProtocols.Http2);

                options.ListenLocalhost(webapiPort, o => o.Protocols =
                    HttpProtocols.Http1);
            })
            .UseStartup<Startup>()
            .Build()
            .Run();
        }



    }
}
