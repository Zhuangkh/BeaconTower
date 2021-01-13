using System;
using System.Collections.Generic;
using System.Diagnostics;
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

#if DEBUG 
            Stopwatch sw = new();
            sw.Start();
            BTraceDB.Instance.StartServer();
            sw.Stop();
            Console.WriteLine($"Start server use:{sw.ElapsedMilliseconds}ms");

#if PerformanceInfo
            GetPerformanceInfo();
#endif
#if InsertData
            InsertData();
#endif
#if GetItemPerformanceInfo
            GetItemPerformanceInfo();
#endif

#else
            BenchmarkRunner.Run<SaveItemTest>();
            //BenchmarkRunner.Run<GetItemTest>();
#endif
            Console.WriteLine("Press any key exit.");
            Console.ReadLine();

        }

#pragma warning disable IDE0051 // 删除未使用的私有成员
        private static void InsertData()
#pragma warning restore IDE0051 // 删除未使用的私有成员
        {
            byte[] _data;
            var td = new Dictionary<string, string>();
            for (int i = 0; i < 1; i++)
            {
                td.Add(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            }
            for (int j = 0; j < 1024 << 8; j++)
            {
                var id = LuanNiao.Core.IDGen.GetInstance().NextId();
                _data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(td));
                for (int i = 0; i < 1; i++)
                {
                    BTraceDB.Instance.SaveItem(id, DateTime.Now.Ticks, _data);
                }
            }
        }

#pragma warning disable IDE0051 // 删除未使用的私有成员
        private static void GetPerformanceInfo()
#pragma warning restore IDE0051 // 删除未使用的私有成员
        {
            Stopwatch sw = new();
            sw.Restart();
            var blockCount = BTraceDB.Instance.BlockCount;
            sw.Stop();
            Console.WriteLine($"get BlockCount:{blockCount} use:{sw.ElapsedMilliseconds}ms");


            sw.Restart();
            var sliceCount = BTraceDB.Instance.SliceCount;
            sw.Stop();
            Console.WriteLine($"get SliceCount:{sliceCount} use:{sw.ElapsedMilliseconds}ms");

            sw.Restart();
            var allIDS = BTraceDB.Instance.AllTraceID;
            sw.Stop();
            Console.WriteLine($"get all id:{allIDS.Count} use:{sw.ElapsedMilliseconds}ms");
        }

        private static void GetItemPerformanceInfo()
        {
            Stopwatch sw = new();
            sw.Restart();
            var allIDS = BTraceDB.Instance.AllTraceID;
            sw.Stop();
            Console.WriteLine($"get all id:{allIDS.Count} use:{sw.ElapsedMilliseconds}ms");

            sw.Restart();
            foreach (var item in allIDS)
            {
                BTraceDB.Instance.TryGetItem(item, out _);
            }
            sw.Stop();
            Console.WriteLine($"foreach all trace item:{allIDS.Count} use:{sw.ElapsedMilliseconds}ms");
        }


    }
}
