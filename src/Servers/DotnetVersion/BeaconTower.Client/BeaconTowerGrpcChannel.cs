using BeaconTower.Protocol;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BeaconTower.Client
{
    public class BeaconTowerGrpcChannel
    {
        public bool Connected { get; private set; }
        public GrpcChannel Channel { get; init; }

        public BeaconTowerGrpcChannel(string addr)
        {
            Channel = GrpcChannel.ForAddress(addr);
            Watcher();
        }

        private void Watcher()
        {
            Thread job = new Thread(new ThreadStart(() =>
            {
                var jumperSleep = 1000;
                var times = 1;
                while (true)
                {
                    try
                    {
                        var client = new CommonRequest.CommonRequestClient(Channel);
                        var res = client.GetState(new GetStateRequest() { TimeStamp = DateTime.Now.Ticks });
                        Connected = true;
                        times = 1;
                    }
                    catch (RpcException ex)
                    {
                        Console.WriteLine($"Got error state from grpc Server:{Channel.Target} StatusCode:{Enum.GetName(typeof(StatusCode), ex.StatusCode)}");
                        Connected = false;
                        times += times < 20 ? 1 : 0;
                    }
                    Thread.Sleep(jumperSleep * times);
                }
            }))
            {
                IsBackground = true
            };
            job.Start();
        }
    }
}
