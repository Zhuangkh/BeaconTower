using System.Runtime.InteropServices;

namespace BeaconTower.TraceDB.Slice.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct Metadata
    {
        public ushort CRC16;
        public uint TraceItemCount;
        public long CurrentPosition;
    }
}
