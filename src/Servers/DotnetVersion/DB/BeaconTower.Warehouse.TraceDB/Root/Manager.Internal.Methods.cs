using System;
using System.IO;
using BlockManager = BeaconTower.Warehouse.TraceDB.Block.Manager;

namespace BeaconTower.Warehouse.TraceDB.Root
{
    internal partial class Manager
    {

        internal partial void Init()
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

        internal partial bool SaveItem(long traceID, long timestamp, byte[] data)
        {

            var target = GetTargetBlock(traceID);
            if (target == null)
            {
                target = GetCanSaveBlock();
            }
            if (target == null)
            {
                target = CreateBlock();
            }
            /*
                |   Method |     Mean |   Error |  StdDev |
                |--------- |---------:|--------:|--------:|
                | SaveItem | 243.4 ns | 0.59 ns | 0.55 ns |
             */

            //return true;
            return target.SaveItem(traceID, timestamp, data);
        }
    }
}
