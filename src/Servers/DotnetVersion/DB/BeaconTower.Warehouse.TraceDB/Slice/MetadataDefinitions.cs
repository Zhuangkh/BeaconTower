using System;
using static BeaconTower.Warehouse.TraceDB.Block.BlockDefinitions;

namespace BeaconTower.Warehouse.TraceDB.Slice
{
    internal static class MetadataDefinitions
    {
        public const ushort Metadata_Head_CRC16_Position = 0;
        public const ushort Metadata_Head_CRC16_Size = sizeof(ushort);

        public const ushort Metadata_Head_TraceItemCount_Position = Metadata_Head_CRC16_Position + Metadata_Head_CRC16_Size;
        public const ushort Metadata_Head_TraceItemCount_Size = sizeof(uint);


        public const ushort Metadata_Head_FromTraceID_Position = Metadata_Head_TraceItemCount_Position + Metadata_Head_TraceItemCount_Size;
        public const ushort Metadata_Head_FromTraceID_Size = sizeof(long);


        public const ushort Metadata_Head_ToTraceID_Position = Metadata_Head_FromTraceID_Position + Metadata_Head_FromTraceID_Size;
        public const ushort Metadata_Head_ToTraceID_Size = sizeof(long);

        public const ushort Metadata_Head_CurrentPosition_Position = Metadata_Head_ToTraceID_Position + Metadata_Head_ToTraceID_Size;
        public const ushort Metadata_Head_CurrentPosition_Size = sizeof(long);

        public const ushort Metadata_Head_Size = 1024;


        public const ushort Metadata_TraceID_List_Position = Metadata_Head_Size;
        public const int Metadata_TraceID_List_Size = Block_TraceItem_Maximum / Block_Maximum_Number_Of_Slice_Count * sizeof(long);


    }
}
