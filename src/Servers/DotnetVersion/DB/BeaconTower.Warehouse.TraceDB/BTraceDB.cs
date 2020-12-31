using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using RootManager = BeaconTower.Warehouse.TraceDB.Root.Manager;

namespace BeaconTower.Warehouse.TraceDB
{
    public class BTraceDB
    {

        private static readonly ConcurrentDictionary<int, BTraceDB> _instance = new();
        private readonly RootManager _rootManager;

        public static BTraceDB Instance => _instance.GetOrAdd(1, (k) =>
        {
            return new BTraceDB();
        });



        private BTraceDB()
        {
            _rootManager = new RootManager();
        }
        /// <summary>
        /// start he db server
        /// </summary>
        public void StartServer()
        {
            _rootManager.Init();
        }
        /// <summary>
        /// save the item to this db instance
        /// </summary>
        public void SaveItem(long traceID, long timestamp, byte[] data)
        {
            _rootManager.SaveItem(traceID, timestamp, data);
        }

   

    }
}
