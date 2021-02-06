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
            if (level > 3)
            {
                return;
            }
            using var tracer1 = TracerClient.Instance.CreateNodeTracer(traceID, previousEventID);
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
        }

        public static void CallMethod(NodeTracer item)
        {
            if (item == null)
            {
                return;
            }
            using var mT = item.CreateMethodTrace(1);
            mT.BeforMethodInvokeAsync();
            mT.CreateLog(message: $"{nameof(CallMethod)}调用了一下~").SendAsync();
            
            var level1 = 0;
            CallMethod1(item, ref level1);
            var level2 = 0;
            CallMethod2(item, ref level2); 
        }

        public static void CallMethod1(NodeTracer item, ref int level)
        {
            if (level > 3)
            {
                return;
            }
            if (item == null)
            {
                return;
            }
            using var mT = item.CreateMethodTrace(2);
            mT.CreateLog(message: $"这时候进入了{nameof(CallMethod1)}调用了一下~,这时候是level:{level}").SendAsync();
            mT.BeforMethodInvokeAsync();
            level++;
            CallMethod1(item, ref level);
        }
        public static void CallMethod2(NodeTracer item, ref int level)
        {
            if (level > 3)
            {
                return;
            }
            if (item == null)
            {
                return;
            }
            using var mT = item.CreateMethodTrace(3);
            mT.CreateLog(message: $"这时候进入了{nameof(CallMethod2)}调用了一下~,这时候是level:{level}").SendAsync();
            mT.BeforMethodInvokeAsync();
            level++;
            CallMethod2(item, ref level);
        }
    }
}
