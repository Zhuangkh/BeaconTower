using BeaconTower.TraceDB.Slice.Models;
using System.Collections.Generic;
using System.IO;
using BlockManager = BeaconTower.TraceDB.Block.Manager;

namespace BeaconTower.TraceDB.Root
{
    public partial class Manager
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

        public partial bool SaveItem(long traceID, long timestamp, byte[] data)
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

        public partial bool TryGetItem(long traceID, out List<TraceItem> data)
        {
            data = null;
            var targetBlock = GetTargetBlock(traceID);
            if (targetBlock == null)
            {
                return false;
            }
            data = targetBlock.GetTraceItems(traceID);
            return true;
        }

        public partial bool TryGetItemMetadata(long traceID, out List<TraceItemSummary> data)
        {
            data = null;
            var targetBlock = GetTargetBlock(traceID);
            if (targetBlock == null)
            {
                return false;
            }
            data = new List<TraceItemSummary>();
            var res = targetBlock.GetTraceItemsMetadata(traceID);
            for (int i = 0; i < res.Count; i++)
            {
                var item = res[i];
                data.Add(new TraceItemSummary()
                {
                    TimeStamp = item.TimeStamp,
                    TraceID = item.TraceID,
                });
            }
            return true;
        }
    }
}
