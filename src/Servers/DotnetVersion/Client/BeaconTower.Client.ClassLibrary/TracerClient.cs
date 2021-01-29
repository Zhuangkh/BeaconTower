using BeaconTower.Client.Abstract;
using BeaconTower.Client.Console;
using BeaconTower.Client.Warehouse.Grpc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace BeaconTower.Client.ClassLibrary
{
    public class TracerClient
    {
        public static readonly TracerClient Instance = new();
        private bool _alreadInitialization = false;

        /// <summary>
        /// your client config file  path
        /// </summary>
        /// <param name="configurationFile">recommend use full path</param>
        /// <returns></returns>
        public TracerClient Init(string configurationFile)
        {
            if (_alreadInitialization)
            {
                return Instance;
            }
            lock (this)
            {
                if (_alreadInitialization)
                {
                    return Instance;
                }
                _alreadInitialization = true;
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile(configurationFile,
                    optional: false,
                    reloadOnChange: true).Build();
                var nodeID = configuration.GetValue<string>("node:id");
                var nodeType = configuration.GetValue<NodeType?>("node:type");
                BeaconTowerOptions options = new BeaconTowerOptions()
                {
                    NodeID = nodeID ?? Guid.NewGuid().ToString("N"),
                    NodeType = nodeType ?? NodeType.Unset
                };

                ServerManager.Instance.Init(options);

                var grpcType = configuration.GetValue<ServerType?>("server:grpc:type");
                if (grpcType != null)
                {
                    WarehouseGrpcServer grpcServer = new() { Type = grpcType.Value };
                    var grpcHostList = configuration.GetSection("server:grpc:host").GetChildren();
                    foreach (var item in grpcHostList)
                    {
                        grpcServer.RegistHost(item.Value);
                    }
                    ServerManager.Instance.RegistServer(grpcServer);
                }

                var consoleType = configuration.GetValue<ServerType?>("server:console:type");
                if (consoleType != null)
                {
                    ConsoleServer consoleServer = new() { Type = consoleType.Value };
                    ServerManager.Instance.RegistServer(consoleServer);
                }


                return Instance;
            }

        }
    }
}
