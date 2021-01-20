using BeaconTower.Client.Abstract;
using BeaconTower.TraceDB.NodeTraceDB;
using System;
using System.IO;
using System.Reflection;

namespace Z.Example.NodeTraceDBTest
{
    public class Program
    {
        public static void Main(string[] _)
        {
            var nodeDB = DBManager.Instance;
            nodeDB.RegistNodeTraceDB(
                 "Test"
                 , (new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName)
                ).StartServer();

            var test = ClientManager.Instance.CreateNodeTracer();
            nodeDB.SaveItem(test);

            foreach (var item in nodeDB.AllNodeInfo)
            {
                Console.WriteLine($"AliasName:{item.AliasName}");
                Console.WriteLine($"OrignalID:{item.OrignalID}");
                Console.WriteLine($"Item Count:{nodeDB.NodeTraceIDList(item).Count}");
            }

            foreach (var item in nodeDB.AllNodeInfo)
            {
                var ids = nodeDB.NodeTraceIDList(item);
                ids.ForEach(id =>
                {
                    nodeDB.TryGetNodeTraceItem(id, out var nodeTracers);
                    nodeTracers.ForEach(tItem =>
                    {                        
                        Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(tItem)); 
                    });
                });
            }


            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }
    }
}
