using BeaconTower.TraceDB;
using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;

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
            DataBase.Instance.RegistDB();
            DataBase.Instance.StartServer();
            byte[] bytes = new byte[4];
            using System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            _data = DataBase.Instance.Default.AllTraceID;
            _random = new Random(BitConverter.ToInt32(bytes, 0));
            _count = _data.Count;
        }

        [Benchmark]
        public void GetItem()
        {
            DataBase.Instance.Default.TryGetItem(_data[_random.Next(0, _count)], out _);
        }
    }
}
