using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BeaconTower.Warehouse.TraceDB.Data
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TraceMetadataItem
    {
        public long TraceID;
        public int Length;
        public long FileName;
    }
}
