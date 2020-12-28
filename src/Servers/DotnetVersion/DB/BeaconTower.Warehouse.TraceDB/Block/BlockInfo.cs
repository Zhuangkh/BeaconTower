using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeaconTower.Warehouse.TraceDB.Data;

namespace BeaconTower.Warehouse.TraceDB.Block
{
    /// <summary>
    /// 逻辑整合信息
    /// </summary>
    internal class BlockInfo
    {
        public long FileName { get; set; }
        public long FromTraceID { get; set; }
        public long ToTraceID { get; set; } 
        public List<TraceMetadataItem> ItemsInfo { get; set; } 
    }
}
