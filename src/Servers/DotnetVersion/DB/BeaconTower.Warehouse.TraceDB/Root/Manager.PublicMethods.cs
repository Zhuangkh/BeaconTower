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
            lock (_rootManagerLockKey)
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
                    var item = new BlockManager(dicInfo.CreateSubdirectory($"{Guid.NewGuid():N}"));
                    item.LoadOrCreate();
                    _allBlocks.Add(item);
                }                
                _initialized = true;
            }
        }

        public partial bool SaveItem(long traceID, byte[] data)
        {
            if (_currentBlock != null)
            {
                return _currentBlock.SaveItem(traceID, data);
            }
            lock (this)
            {
                if (_currentBlock != null)
                {
                    return _currentBlock.SaveItem(traceID, data);
                }
                _currentBlock = _allBlocks[0];//Todo: If this block is full, move to next
            }
            return _currentBlock.SaveItem(traceID, data);
        }
    }
}
