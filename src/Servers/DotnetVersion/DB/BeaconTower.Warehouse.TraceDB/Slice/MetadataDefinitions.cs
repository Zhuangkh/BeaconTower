using System; 

namespace BeaconTower.Warehouse.TraceDB.Slice
{
    internal static class MetadataDefinitions
    {
        public const ushort Metadata_Head_CRC16_Position = 0;
        public const ushort Metadata_Head_CRC16_Size = sizeof(ushort);

        public const ushort Metadata_Head_TraceItemCount_Position = 0;
        public const ushort Metadata_Head_TraceItemCount_Size = sizeof(uint);


        public const ushort Metadata_Head_FromTraceID_Position = 0;
        public const ushort Metadata_Head_FromTraceID_Size = sizeof(long);


        public const ushort Metadata_Head_ToTraceID_Position = 0;
        public const ushort Metadata_Head_ToTraceID_Size = sizeof(long);

        public const ushort Metadata_Head_Size= 1024;
        
   

    }
}
