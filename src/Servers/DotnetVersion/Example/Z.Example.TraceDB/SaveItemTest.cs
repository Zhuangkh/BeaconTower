using BeaconTower.TraceDB;
using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Z.Example.TraceDB
{

    public class SaveItemTest
    {
        private byte[] _data = null;
        [GlobalSetup]
        public void Setup()
        {
            DataBase.Instance.RegistDB();
            DataBase.Instance.StartServer();
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
            DataBase.Instance.Default.SaveItem(LuanNiao.Core.IDGen.GetInstance().NextId(), DateTime.Now.Ticks, _data);
        }
    }
}
