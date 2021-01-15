using System;
using System.Runtime.InteropServices;

namespace BeaconTower.TraceDB.Block.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct BlockMetadata
    {

        public UInt16 CRC16;
        /// <summary>
        /// 0XFFFF_FFFF
        /// 0X000_F000<--大版本号,非向下兼容Version
        /// </summary>
        public uint Version;
        public long FromTraceID;
        public long ToTraceID;
        public int CurrentItemsCount;
    }
}
