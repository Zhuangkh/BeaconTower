using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BeaconTower.Warehouse.TraceDB;
using BenchmarkDotNet.Attributes;

namespace Z.Example.TraceDB
{

    public class SaveItemTest
    {
        private byte[] _data = null;
        [GlobalSetup]
        public void Setup()
        {
            BTraceDB.Instance.StartServer();
            var td = new Dictionary<string, string>();
            for (int i = 0; i < 100; i++)
            {
                td.Add(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            }
            _data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(td));
        }

        [Benchmark]
        public void SaveItem()
        {
            BTraceDB.Instance.SaveItem(LuanNiao.Core.IDGen.GetInstance().NextId(), DateTime.Now.Ticks, _data);
        }
    }
}
