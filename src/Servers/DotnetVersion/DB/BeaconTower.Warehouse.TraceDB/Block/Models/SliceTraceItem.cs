using System.Runtime.InteropServices;

namespace BeaconTower.Warehouse.TraceDB.Block.Models
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct BlockTraceItem
    {
        public long TraceID;
        public long FileName;
    }
}
