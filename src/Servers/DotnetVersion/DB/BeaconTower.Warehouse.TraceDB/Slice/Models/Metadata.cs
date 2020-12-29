using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BeaconTower.Warehouse.TraceDB.Slice.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct Metadata
    {
        public ushort CRC16;
        public uint TraceItemCount;
        public long FromTraceID;
        public long ToTraceID;
        public long CurrentPosition;
    }
}
