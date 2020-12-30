using System;
using System.IO;
using BlockManager = BeaconTower.Warehouse.TraceDB.Block.Manager;

namespace BeaconTower.Warehouse.TraceDB.Root
{
    internal partial class Manager
    {

        public partial void Init()
        {
            if (_initialized)
            {
                return;
            } 
            var dicInfo = new DirectoryInfo(_rootFolder);
            if (!dicInfo.Exists)
            {
                dicInfo.Create();
            }
            var dicList = dicInfo.GetDirectories();
            for (int i = 0; i < dicList.Length; i++)
            {
                var item = new BlockManager(dicList[i]);
                item.LoadOrCreate();
                _allBlocks.Add(item);
            }
            if (_allBlocks.Count == 0)
            {
                CreateBlock();
            }
            _initialized = true;
        }

        public partial bool SaveItem(long traceID, long timestamp, byte[] data)
        {
            var target = GetBlockManager(traceID);
            if (target == null)
            {
                CreateBlock();
                target = GetBlockManager(traceID);
            }
            return target.SaveItem(traceID, timestamp, data);
        }
    }
}
