using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static BeaconTower.Warehouse.TraceDB.Block.BlockDefinitions;

namespace BeaconTower.Warehouse.TraceDB.Block
{
    /// <summary>
    /// TraceDB block manager
    /// </summary>
    internal partial class Manager
    {

        internal Manager(DirectoryInfo directoryInfo)
        {
            _blockDirectory = directoryInfo;
            _blockName = long.Parse(directoryInfo.Name);
        }



        /// <summary>
        /// load or create this block
        /// <para>Will create the metadata or other init file when file not exists</para>
        /// </summary>
        internal partial void LoadOrCreate();

        /// <summary>
        /// save the item to this block,
        /// </summary>
        /// <returns></returns>
        internal partial bool SaveItem(long traceID, long timestamp, byte[] data);

        /// <summary>
        /// is this block can save item
        /// </summary>
        /// <returns></returns>
        internal bool HasSit() => _metadata.CurrentItemsCount < Block_TraceItem_Maximum;

        /// <summary>
        /// is this 'traceID' in this  data block's range
        /// </summary>
        /// <param name="traceID"></param>
        /// <returns></returns>
        internal bool InRange(long traceID)
        {
            return traceID >= _metadata.FromTraceID && traceID <= _metadata.ToTraceID;
        }


        /// <summary>
        /// get this block's name, block's name is directory folder name
        /// </summary>
        internal long BlockName => _blockName;

        /// <summary>
        /// ge this block's slice count
        /// </summary>
        internal int SliceCount => _sliceLoop.Count;

        /// <summary>
        /// this block's traceItemCount
        /// <para>this method will provisionally calculate all data!</para>
        /// </summary>
        internal int TraceItemCount => _sliceLoop.Values.Sum(item => item.ItemCount);

        /// <summary>
        /// this block's all trace item's ID list
        /// <para>this method will provisionally calculate all data!</para>
        /// </summary>
        internal IList<long> TraceIDs
        {
            get
            {
                IList<long> result = new List<long>();
                var allSlice = _sliceLoop.Values;
                foreach (var item in allSlice)
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

        internal List<TraceItem> GetTraceItems(long traceID)
        {
            List<TraceItem> res = new();
            var allSlice = _sliceLoop.Values;
            foreach (var item in allSlice)
            {
                res.AddRange(item.GetTraceItems(traceID));
            }
            return res;
        }
    }
}
