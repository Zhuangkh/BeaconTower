using System;

namespace BeaconTower.TraceDB.Block
{
    internal static class MetadataDefinitions
    {

        public const int Metadata_Head_CRC16_Position = 0;
        public const int Metadata_Head_CRC16_Size = sizeof(UInt16);

        public const int Metadata_Head_Version_Position = Metadata_Head_CRC16_Position + Metadata_Head_CRC16_Size;
        public const int Metadata_Head_Version_Size = sizeof(UInt16);

        public const int Metadata_Head_FromTraceID_Position = Metadata_Head_Version_Position + Metadata_Head_Version_Size;
        public const int Metadata_Head_FromTraceID_Size = sizeof(long);


        public const int Metadata_Head_ToTraceID_Position = Metadata_Head_FromTraceID_Position + Metadata_Head_FromTraceID_Size;
        public const int Metadata_Head_ToTraceID_Size = sizeof(long);

        public const int Metadata_Head_CurrentItemsCount_Position = Metadata_Head_ToTraceID_Position + Metadata_Head_ToTraceID_Size;
        public const int Metadata_Head_CurrentItemsCount_Size = sizeof(int);

        public const int Metadata_Head_Reserve_Position = Metadata_Head_CurrentItemsCount_Position + Metadata_Head_CurrentItemsCount_Size;


    }
}
