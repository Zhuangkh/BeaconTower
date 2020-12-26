using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using BeaconTower.Warehouse.TraceDB.Data;
using static BeaconTower.Warehouse.TraceDB.Block.CommonDefinition;

namespace BeaconTower.Warehouse.TraceDB.Block
{
    internal class Tools
    {

        public static BlockMetadata CreateBlockItem(DirectoryInfo directoryInfo)
        {
            var fi = new FileInfo(Path.Combine(directoryInfo.FullName, Metadata_File_Name));
            if (!fi.Exists)
            {
                return null;
            }
            try
            {
                using var fs = fi.OpenRead();
                using var headBufferRent = MemoryPool<byte>.Shared.Rent(Metadata_Head_Valid_Size);
                var headBuffer = headBufferRent.Memory.Slice(0, Metadata_Head_Valid_Size);
                fs.ReadAsync(headBuffer);
                BlockMetadata res = new();
                res.CRC16 = BitConverter.ToUInt16(headBuffer.Span[Metadata_Head_CRC16_Position..Metadata_Head_FromTraceID_Position]);
                res.FromTraceID = BitConverter.ToInt64(headBuffer.Span[Metadata_Head_FromTraceID_Position..Metadata_Head_ToTraceID_Position]);
                res.ToTraceID = BitConverter.ToInt64(headBuffer.Span[Metadata_Head_ToTraceID_Position..Metadata_Head_CurrentItemsCount_Position]);
                res.CurrentItemsCount = BitConverter.ToInt32(headBuffer.Span[Metadata_Head_CurrentItemsCount_Position..Metadata_Head_Reserve_Position]);
                res.Items = new List<BlockInfo>();

                var traceItemsBufferSize = res.CurrentItemsCount * Metadata_TraceDataItem_Size;
                using var traceItemBufferRent = MemoryPool<byte>.Shared.Rent(traceItemsBufferSize);
                var traceItemBuffer = traceItemBufferRent.Memory.Slice(0, traceItemsBufferSize).Span;

                fs.Position = Metadata_TraceDataItem_Position;
                fs.Read(traceItemBuffer);

                var traceItems = MemoryMarshal.Cast<byte, TraceMetadataItem>(traceItemBuffer);

                //Todo 在这里整理一下~                
                return res;
            }
            catch
            {
                return null;
            }
        }
    }
}
