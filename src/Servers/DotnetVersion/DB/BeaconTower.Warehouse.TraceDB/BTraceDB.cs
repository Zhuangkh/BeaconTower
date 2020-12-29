using System;
using System.Collections.Concurrent;
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

        public void StartServer()
        {
            _rootManager.Init();
        }
        public void SaveItem(long traceID,byte[] data)
        {
            _rootManager.SaveItem(traceID, data);
        }

 



       

    }
}
