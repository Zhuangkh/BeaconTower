using BeaconTower.Client.Abstract;
using BeaconTower.Client.ClassLibrary;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace BeaconTower.Warehouse.ClassLibrary.Client.Example
{
    public class Program
    {

        public static void Main(string[] _)
        {

            var instance = TracerClient.Instance.Init("./appsettings.json");

            Task.Run(() =>
            {
                Task.Delay(1000).Wait();
                //while (true)
                //{
                var level = 0;
                SendTrace(null, "Main", 0, level);
                //}
            });
            Console.ReadLine();
            Console.WriteLine("Hello World!");

        }

        public static void SendTrace(long? traceID, string path, long previousEventID, int level)
        {
            if (level > 1)
            {
                return;
            }
            var tracer1 = TracerClient.Instance.CreateNodeTracer(traceID, previousEventID);
            tracer1.NodeID = $"Node{level}";
            tracer1.TimeStamp = DateTime.Now.Ticks;
            tracer1.Path = path;
            tracer1.QueryString = string.Empty;
            tracer1.PreviousEventID = previousEventID;
            tracer1.BeforeNodeActiveAsync();
            CallMethod(tracer1);
            var childCount = new Random().Next(1, 3);
            for (int i = 0; i < childCount; i++)
            {
                SendTrace(tracer1.TraceID, "main", tracer1.EventID, level + 1);
            }
            Task.Delay(new Random().Next(1, 100)).Wait();
            tracer1.TimeStamp = DateTime.Now.Ticks;
            tracer1.AfterNodeActivedAsync();
        }

        public static void CallMethod(NodeTracer item)
        {
            if (item == null)
            {
                return;
            }
            var mT = item.CreateMethodTrace(123);
            mT.BeforMethodInvokeAsync();
            mT.AfterMethodInvokedAsync();

        }
    }
}
