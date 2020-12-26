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
        public long FromTraceID { get; set; }
        public long ToTraceID { get; set; }
        public int CurrentItemsCount { get; set; }
        public UInt16 CRC16 { get; set; }
        public List<BlockInfo> Items { get; set; }
    }
}
