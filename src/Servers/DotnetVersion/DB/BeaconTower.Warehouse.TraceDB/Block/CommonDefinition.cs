using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeaconTower.Warehouse.TraceDB.Data;

namespace BeaconTower.Warehouse.TraceDB.Block
{
    internal static class CommonDefinition
    {
        public const int Block_TraceItem_Maximum = 1024 << 6;


        public const int Metadata_Head_Size = 1024;


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


        public const int Metadata_TraceDataItem_Position = Metadata_Head_Size;
        public const int Metadata_TraceDataItem_TraceID_Size = sizeof(long);
        public const int Metadata_TraceDataItem_FileName_Size = sizeof(long);
        public const int Metadata_TraceDataItem_Size = Metadata_TraceDataItem_TraceID_Size + Metadata_TraceDataItem_FileName_Size;
        public const int Metadata_BlockInfo_Size = Metadata_TraceDataItem_Size * Block_TraceItem_Maximum;

        public const string Metadata_File_Name = "metadata.btd";
        public const int Block_Metadata_File_Size = Metadata_Head_Size + Metadata_BlockInfo_Size;
    }
}
