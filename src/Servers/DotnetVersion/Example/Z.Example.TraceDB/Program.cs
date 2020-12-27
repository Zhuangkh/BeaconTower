using System;
using BeaconTower.Warehouse.TraceDB;

namespace Z.Example.TraceDB
{
    public class Program
    {
        public static void Main(string[] _)
        {
            BTraceDB.Instance.StartServer();
            BTraceDB.Instance.SaveItem(123123123, new byte[] { 0x01, 0x02 });

            //Console.ReadLine();
            Console.WriteLine("Hello World!");

        }
    }
}
