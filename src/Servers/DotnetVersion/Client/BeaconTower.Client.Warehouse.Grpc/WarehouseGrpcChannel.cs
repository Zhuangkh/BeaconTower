using BeaconTower.Protocol;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BeaconTower.Client.Warehouse.Grpc
{
    public class WarehouseGrpcChannel
    {
        public bool Connected { get; private set; }
        public GrpcChannel Channel { get; init; }

        public WarehouseGrpcChannel(string addr)
        {
            Channel = GrpcChannel.ForAddress(addr);
            Watcher();
        }

        private void Watcher()
        {
            Thread job = new(new ThreadStart(() =>
            {
                var jumperSleep = 1000;
                var times = 1;
                while (true)
                {
                    try
                    {
                        var client = new CommonRequest.CommonRequestClient(Channel);
                        var res = client.GetState(new GetStateRequest() { TimeStamp = DateTime.Now.Ticks });
                        times = 1;
                        if (!Connected)
                        {
                            Console.WriteLine($"Grpc Server:{Channel.Target} recovery");
                        }
                        Connected = true;
                    }
                    catch (RpcException ex)
                    {
                        Console.WriteLine($"Grpc Client: Got an error state from Server:{Channel.Target} StatusCode: {Enum.GetName(typeof(StatusCode), ex.StatusCode)}. The client will retry in {(times > 20 ? 20 : times)} seconds. This is the {times}th report.");
                        Connected = false;
                        times += 1;
                    }
                    Thread.Sleep(jumperSleep * (times > 20 ? 20 : times));
                }
            }))
            {
                IsBackground = true
            };
            job.Start();
        }
    }
}
