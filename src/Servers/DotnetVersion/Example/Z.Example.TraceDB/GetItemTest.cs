using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BeaconTower.TraceDB;
using BenchmarkDotNet.Attributes;

namespace Z.Example.TraceDB
{

    [MemoryDiagnoser]
    public class GetItemTest
    {
        private IList<long> _data = null;
        private int _count = 0;
        private Random _random = null;
        [GlobalSetup]
        public void Setup()
        {
            BTraceDB.Instance.RegistDB();
            BTraceDB.Instance.StartServer();
            byte[] bytes = new byte[4];
            using System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            _data = BTraceDB.Instance.Default.AllTraceID;
            _random = new Random(BitConverter.ToInt32(bytes, 0));
            _count = _data.Count;
        }

        [Benchmark]
        public void GetItem()
        {
            BTraceDB.Instance.Default.TryGetItem(_data[_random.Next(0, _count)], out _);
        }
    }
}
