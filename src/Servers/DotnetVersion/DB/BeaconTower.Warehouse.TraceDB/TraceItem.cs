using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaconTower.Warehouse.TraceDB
{
    public class TraceItem
    {
        public long TraceID { get; set; }
        public long TimeStamp { get; set; }
        public byte[] Data { get; set; }
    }
}
