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
            DBManager.Instance.RegistNodeTraceDB(
                 "Test"
                 , (new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName)
                ).StartServer();
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }
    }
}
