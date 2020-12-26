using System;
using BeaconTower.Warehouse.TraceDB;

namespace Z.Example.TraceDB
{
    public class Program
    {
        public static void Main(string[] _)
        {
            Console.WriteLine($"{(1024 << 10) / 1024 / 1024}");
            //BTraceDB.Instance.LoadDB();
            //Console.ReadLine();
            Console.WriteLine("Hello World!");
        }
    }
}
