using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeaconTower.Warehouse.TraceDB.Data;

namespace BeaconTower.Warehouse.TraceDB.Block
{
    internal class BlockMetadata
    {
        public UInt16 CRC16 { get; set; }
        /// <summary>
        /// 0XFFFF_FFFF
        /// 0X000_F000<--大版本号,非向下兼容Version
        /// </summary>
        public uint Version { get; set; }
        public long FromTraceID { get; set; }
        public long ToTraceID { get; set; }
        public int CurrentItemsCount { get; set; }        
        public List<BlockInfo> Items { get; set; }
    }
}
