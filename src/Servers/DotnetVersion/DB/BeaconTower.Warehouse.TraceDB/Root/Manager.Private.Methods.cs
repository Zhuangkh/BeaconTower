using System;
using System.IO;
using BlockManager = BeaconTower.Warehouse.TraceDB.Block.Manager;

namespace BeaconTower.Warehouse.TraceDB.Root
{
    public partial class Manager
    {
        private BlockManager GetCanSaveBlock()
        {
            for (int i = 0; i < _allBlocks.Count; i++)
            {
                if (_allBlocks[i].HasSit())//这里少了一个逻辑,那就是往近了找
                {
                    return _allBlocks[i];
                }
            }
            return null;
        }

        private BlockManager GetTargetBlock(long traceID)
        {
            //两次查找,第一次查找有没有直接奴属于某一个range的,如果都没有,那么就找没有满的且最靠近的那个(也就是最接近block右边界的)
            for (int i = 0; i < _allBlocks.Count; i++)
            {
                if (_allBlocks[i].InRange(traceID))
                {
                    return _allBlocks[i];
                }
            }
            return null;
        }

        /// <summary>
        /// create a block, call this method when block not exists or all block was full
        /// </summary>
        private BlockManager CreateBlock()
        {
            var item = new BlockManager(new DirectoryInfo(_rootFolder).CreateSubdirectory($"{LuanNiao.Core.IDGen.GetInstance().NextId()}"));
            item.LoadOrCreate();
            _allBlocks.Add(item);
            return item;
        }
    }
}
