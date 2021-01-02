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
            //这里逻辑似乎最复杂
            //先要找,traceID对应的block,然后再去找traceID可以存的block...
            //其实Block里头也可以这样做,首先slice是直接1234567这样排序,然后item存入的地方按照traceID%X
            //然后这里就只需要找block,block就不需要寻址Trace了
            //这里忽略分配不均衡的问题,因为当前这个系统不需要考虑这个问题.
            //至于哈希环,如果用了它你还得存储它,因为下一次的哈希环并不能保证与上次的分配方式一致
            //哈希环是保证"本次"分配均衡而不是:每次分配到相同的目标上
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
