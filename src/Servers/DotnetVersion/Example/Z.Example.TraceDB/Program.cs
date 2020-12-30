using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using BeaconTower.Warehouse.TraceDB;
using BenchmarkDotNet.Running;

namespace Z.Example.TraceDB
{
    public class Program
    {
        public static void Main(string[] _)
        {
            BTraceDB.Instance.StartServer();

#if DEBUG
            byte[] _data = null;
            var td = new Dictionary<string, string>();
            for (int i = 0; i < 1; i++)
            {
                td.Add(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            }
            for (int j = 0; j < 512; j++)
            {
                var id = LuanNiao.Core.IDGen.GetInstance().NextId();
                _data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(td));
                for (int i = 0; i < 1; i++)
                {
                    BTraceDB.Instance.SaveItem(id, DateTime.Now.Ticks, _data);
                }
            }
#else
            BenchmarkRunner.Run<SaveItemTest>();
#endif 
            Console.WriteLine("Hello World!");

        }


    }
}
