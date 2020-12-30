using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BeaconTower.Warehouse.TraceDB.Slice.Models
{
    [StructLayout(LayoutKind.Sequential,Pack =1)]
    internal struct TraceItemMetadata
    {
        public long Position;
        public long TimeStamp;
        public uint Length;       
    }
}
