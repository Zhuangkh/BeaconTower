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

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }
    }
}
