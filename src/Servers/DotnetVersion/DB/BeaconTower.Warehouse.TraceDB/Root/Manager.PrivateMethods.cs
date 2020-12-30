using System;
using System.IO;
using BlockManager = BeaconTower.Warehouse.TraceDB.Block.Manager;

namespace BeaconTower.Warehouse.TraceDB.Root
{
    internal partial class Manager
    {
        private BlockManager GetBlockManager(long traceID)
        {
            for (int i = 0; i < _allBlocks.Count; i++)
            {
                if (_allBlocks[i].CanSave(traceID))
                {
                    return _allBlocks[i];
                }
            }
            return null;
        }

        /// <summary>
        /// create a block, call this method when block not exists or all block was full
        /// </summary>
        private void CreateBlock()
        {
            var item = new BlockManager(new DirectoryInfo(_rootFolder).CreateSubdirectory($"{Guid.NewGuid():N}"));
            item.LoadOrCreate();
            _allBlocks.Add(item);
        }
    }
}
