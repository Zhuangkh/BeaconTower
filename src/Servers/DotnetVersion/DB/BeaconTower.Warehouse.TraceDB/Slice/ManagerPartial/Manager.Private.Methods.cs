using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BeaconTower.Warehouse.TraceDB.Slice.Models;
using static BeaconTower.Warehouse.TraceDB.Slice.MetadataDefinitions;

namespace BeaconTower.Warehouse.TraceDB.Slice
{
    internal partial class Manager
    {
        private void InitMetadata()
        {
            if (_handle.Length == 0)
            {
                lock (this)
                {
                    if (_handle.Length == 0)
                    {
                        _metadata = new();
                        _metadata.FromTraceID = long.MaxValue;
                        _metadata.ToTraceID = long.MinValue;
                        _metadata.TraceItemCount = 0;
                        SaveMetadataInfo();
                    }
                }
            }
            else
            {
                lock (this)
                {
                    _handle.Position = 0;
                    byte[] headBuffer = new byte[Marshal.SizeOf<Metadata>()];
                    _handle.Read(headBuffer);
                    _metadata = LuanNiao.Core.StructUtilTools.StructUtilTools.ToStruct<Metadata>(headBuffer);
                }
            }
        }
        private void SaveMetadataInfo()
        {
            var _metadataBuffer = LuanNiao.Core.StructUtilTools.StructUtilTools.ToData(in _metadata);
            LuanNiao.Core.NetTools.CRC16IBM.SetCRC16(_metadataBuffer, 0, _metadataBuffer.Length, 0);
            using var headBufferRenter = MemoryPool<byte>.Shared.Rent(Metadata_Head_Size);
            var buffer = headBufferRenter.Memory.Slice(0, Metadata_Head_Size).Span;
            _metadataBuffer.CopyTo(buffer);
            _handle.Write(buffer);
            _handle.Flush();
        }
    }
}
