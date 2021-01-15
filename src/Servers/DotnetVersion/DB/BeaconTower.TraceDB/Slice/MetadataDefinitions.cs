using System;
using static BeaconTower.TraceDB.Block.BlockDefinitions;

namespace BeaconTower.TraceDB.Slice
{
    internal static class MetadataDefinitions
    {
        public const ushort Metadata_Head_CRC16_Position = 0;
        public const ushort Metadata_Head_CRC16_Size = sizeof(ushort);

        public const ushort Metadata_Head_TraceItemCount_Position = Metadata_Head_CRC16_Position + Metadata_Head_CRC16_Size;
        public const ushort Metadata_Head_TraceItemCount_Size = sizeof(uint);

        public const ushort Metadata_Head_CurrentPosition_Position = Metadata_Head_TraceItemCount_Position + Metadata_Head_TraceItemCount_Size;
        public const ushort Metadata_Head_CurrentPosition_Size = sizeof(long);

        public const ushort Metadata_Head_Size = 1024;

         
    }
}
