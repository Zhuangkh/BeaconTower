using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BeaconTower.Warehouse.TraceDB.Slice.Models;
using static BeaconTower.Warehouse.TraceDB.Slice.MetadataDefinitions;
using static BeaconTower.Warehouse.TraceDB.Slice.SliceItemDefinitions;

namespace BeaconTower.Warehouse.TraceDB.Slice
{
    /*We can improve the performance here, but currently, we need not do this~*/
    internal partial class Manager
    {
        private void InitMetadata()
        {
            if (_sliceHandle.Length == 0)
            {
                lock (this)
                {
                    if (_sliceHandle.Length == 0)
                    {
                        CreateNewMetaData();
                    }
                }
            }
            else
            {
                lock (this)
                {
                    _sliceHandle.Position = 0;
                    LoadMetadataInfo();
                    LoadMetadataTraceIDListInfo();
                }
            }
        }

        private void CreateNewMetaData()
        {
            _metadata = new();
            _metadata.FromTraceID = long.MaxValue;
            _metadata.ToTraceID = long.MinValue;
            _metadata.TraceItemCount = 0;
            _metadata.CurrentPosition = Metadata_Head_Size + Metadata_TraceID_List_Size;
            SaveMetadataInfo();
        }
        private void LoadMetadataTraceIDListInfo()
        {
            byte[] listBuffer = new byte[sizeof(long) * _metadata.TraceItemCount];
            _sliceHandle.Read(listBuffer);
            for (int index = 0; index < _metadata.TraceItemCount; index += sizeof(long))
            {
                _traceIDList.Add(BitConverter.ToInt64(listBuffer[index..]));
            }
        }
        private void LoadMetadataInfo()
        {
            byte[] headBuffer = new byte[Marshal.SizeOf<Metadata>()];
            _sliceHandle.Read(headBuffer);
            _metadata = LuanNiao.Core.StructUtilTools.StructUtilTools.ToStruct<Metadata>(headBuffer);
            _sliceHandle.Position = Metadata_TraceID_List_Position;
        }

        private void SaveItemMetadataHandler(long traceID, byte[] data)
        {
            _metadata.FromTraceID = traceID < _metadata.FromTraceID ? traceID : _metadata.FromTraceID;
            _metadata.ToTraceID = traceID > _metadata.ToTraceID ? traceID : _metadata.ToTraceID;
            _metadata.CurrentPosition += SliceItem_Head_Size + data.Length;
            SaveTraceID(traceID);
            SaveMetadataInfo();
        }

        private void SaveTraceID(long traceID)
        {
            _sliceHandle.Position = Metadata_TraceID_List_Position + _traceIDList.Count * sizeof(long);
            _sliceHandle.Write(BitConverter.GetBytes(traceID));
            _sliceHandle.Flush();
            if (!_traceIDList.Contains(traceID))
            {
                ++_metadata.TraceItemCount;
                _traceIDList.Add(traceID);
            }
        }

        private void SaveMetadataInfo()
        {
            var _metadataBuffer = LuanNiao.Core.StructUtilTools.StructUtilTools.ToData(in _metadata);
            LuanNiao.Core.NetTools.CRC16IBM.SetCRC16(_metadataBuffer, 0, _metadataBuffer.Length, 0);
            using var headBufferRenter = MemoryPool<byte>.Shared.Rent(Metadata_Head_Size);
            var buffer = headBufferRenter.Memory.Slice(0, Metadata_Head_Size).Span;
            _metadataBuffer.CopyTo(buffer);
            _sliceHandle.Position = 0;
            _sliceHandle.Write(buffer);
            _sliceHandle.Flush();
        }

        private void SaveTraceItemInfo(long traceID, long timestamp, byte[] data)
        { 
            _traceItemIndexHandle.Write(BitConverter.GetBytes(traceID));
            _traceItemIndexHandle.Write(BitConverter.GetBytes(timestamp));
            _traceItemIndexHandle.Write(BitConverter.GetBytes(data.Length));
            _traceItemIndexHandle.Flush();
        }

        private void LoadTraceItemInfo()
        {
            _traceItemIndexHandle.Position = 0;
            var allItems = new List<long>();
            var buffer = new byte[8];
            for (int index = 0; index < _traceItemIndexHandle.Length; index += 20)
            {
                _traceItemIndexHandle.Position = index;
                _traceItemIndexHandle.Read(buffer);
                allItems.Add(BitConverter.ToInt64(buffer));
            }
        }
    }
}
