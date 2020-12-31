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

        public Manager(DirectoryInfo directoryInfo)
        {
            _blockDirectory = directoryInfo;
            _blockName = long.Parse(directoryInfo.Name);
        }



        /// <summary>
        /// load or create this block
        /// <para>Will create the metadata or other init file when file not exists</para>
        /// </summary>
        public partial void LoadOrCreate();

        /// <summary>
        /// save the item to this block,
        /// </summary>
        /// <returns></returns>
        public partial bool SaveItem(long traceID, long timestamp, byte[] data);

        /// <summary>
        /// can this block save this item
        /// </summary>
        /// <returns></returns>
        public bool CanSave(long traceID)
        {

            return
                (traceID - _metadata.Head.FromTraceID >= 0 && traceID - _metadata.Head.ToTraceID <= 0)
                ||
                _metadata.Head.CurrentItemsCount <= Block_TraceItem_Maximum;

        }


        /// <summary>
        /// get this block's name, block's name is directory folder name
        /// </summary>
        public long BlockName => _blockName;




    }
}
