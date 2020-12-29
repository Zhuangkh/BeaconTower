using System;
using BeaconTower.Warehouse.TraceDB;
using BenchmarkDotNet.Running;

namespace Z.Example.TraceDB
{
    public class Program
    {
        public static void Main(string[] _)
        {
            //BTraceDB.Instance.StartServer();
            //for (int i = 0; i < 100_000; i++)
            //{
               // BTraceDB.Instance.SaveItem(LuanNiao.Core.IDGen.GetInstance().NextId(), new byte[] { 0x01 });
            //}
            //Console.ReadLine();
            BenchmarkRunner.Run<SaveItemTest>();
            Console.WriteLine("Hello World!");

        }
    }
}
