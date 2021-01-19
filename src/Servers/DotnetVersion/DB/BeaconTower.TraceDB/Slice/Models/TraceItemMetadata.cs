using System.Runtime.InteropServices;

namespace BeaconTower.TraceDB.Slice.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TraceItemMetadata
    {
        public long TraceID;
        public long Position;
        public long TimeStamp;
        public int Length;
    }
}
