using System;
using BeaconTower.Warehouse.TraceDB;

namespace Z.Example.TraceDB
{
    public class Program
    {
        public static void Main(string[] _)
        {
            BTraceDB.Instance.StartServer();
            BTraceDB.Instance.SaveItem(LuanNiao.Core.IDGen.GetInstance().NextId(), new byte[] { 0x01 });
            //Console.ReadLine();
            Console.WriteLine("Hello World!");

        }
    }
}
