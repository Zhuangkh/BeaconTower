using System.Collections.Concurrent;
using RootManager = BeaconTower.Warehouse.TraceDB.Root.Manager;

namespace BeaconTower.Warehouse.TraceDB
{
    public class BTraceDB
    {

        private static readonly ConcurrentDictionary<int, BTraceDB> _instance = new ConcurrentDictionary<int, BTraceDB>();
        private readonly RootManager _rootManager;

        public static BTraceDB Instance => _instance.GetOrAdd(1, (k) =>
        {
            return new BTraceDB();
        });



        public BTraceDB()
        {
            _rootManager = new RootManager();            
        }

        public async void LoadDB()
        {
            await _rootManager.Init();
        }

    }
}
