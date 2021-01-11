using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using RootManager = BeaconTower.Warehouse.TraceDB.Root.Manager;
using BlockManager = BeaconTower.Warehouse.TraceDB.Block.Manager;
using System.Linq;

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

        /// <summary>
        /// this db instance's block count info
        /// </summary>
        public int BlockCount => _rootManager.Blocks.Count;

        /// <summary>
        /// this db instance's slice count info
        /// <para>this method will provisionally calculate all data!</para>
        /// </summary>
        public int SliceCount => _rootManager.Blocks.Sum(item => item.SliceCount);

        /// <summary>
        /// this db instance's trace item count
        /// <para>this method will provisionally calculate all data!</para>
        /// </summary>
        public int TraceItemCount => _rootManager.Blocks.Sum(item => item.SliceCount);


        /// <summary>
        /// this db instance's trace id list
        /// <para>this method will provisionally calculate all data!</para>
        /// <para>this method is low performance!</para>        
        /// </summary>        
        public IList<long> AllTraceID
        {
            get
            {
                IList<long> result = new List<long>();
                
                foreach (var item in _rootManager.Blocks)
                {
                    var allItems = item.TraceIDs;
                    foreach (var idItem in allItems)
                    {
                        result.Add(idItem);
                    }
                }
                return result;
            }
        }

        public void TryGetItem(long traceID, out List<TraceItem> data)
        {
            _rootManager.TryGetItem(traceID, out data);
        }
        
    }
}
