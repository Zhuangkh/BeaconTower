using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BeaconTower.Warehouse.TraceDB.Slice.Models;
using static BeaconTower.Warehouse.TraceDB.Slice.MetadataDefinitions;
using static BeaconTower.Warehouse.TraceDB.Slice.SliceItemDefinitions;

namespace BeaconTower.Warehouse.TraceDB.Slice
{
    /*We can improve the performance here, but currently, we need not do this~*/
    internal partial class Manager
    {
        /// <summary>
        /// Init the slice's metadata
        /// </summary>
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
                }
            }
            StartChannelHandler();
        }

        /// <summary>
        /// create new metadata file, call this method when file not exists
        /// </summary>
        private void CreateNewMetaData()
        {
            _metadata = new();
            _metadata.CurrentPosition = Metadata_Head_Size;
            SaveMetadataInfo();
        }

        /// <summary>
        /// load metadata file, call this method when file exists
        /// </summary>
        private void LoadMetadataInfo()
        {
            byte[] headBuffer = new byte[Marshal.SizeOf<Metadata>()];
            _sliceHandle.Read(headBuffer);
            _metadata = LuanNiao.Core.StructUtilTools.StructUtilTools.ToStruct<Metadata>(headBuffer);
            _sliceHandle.Position = Metadata_Head_Size;

        }

        /// <summary>
        /// call this method when we saved item to this slice.
        /// </summary> 
        private void SaveItemMetadataHandler(byte[] data)
        {
            _metadata.CurrentPosition += data.Length;
            _metadata.TraceItemCount++;
            SaveMetadataInfo();
        }

        /// <summary>
        /// save metadata info to file
        /// </summary>
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

        /// <summary>
        /// save trace item info to index file
        /// </summary>
        /// <param name="data"></param>
        private void SaveTraceItemInfo(long position, long traceID, long timestamp, byte[] data)
        {
            _traceItemIndexHandle.Position = _traceItemIndexHandle.Length;
            TraceItemMetadata info = new TraceItemMetadata();
            info.Position = position;
            info.TraceID = traceID;
            info.TimeStamp = timestamp;
            info.Length = data.Length;
            _traceItemIndexHandle.Write(LuanNiao.Core.StructUtilTools.StructUtilTools.ToData(in info));
            _traceItemIndexHandle.Flush();
        }

        /// <summary>
        /// load all trace item index info
        /// </summary>
        private void TestLoadData()
        {
            _traceItemIndexHandle.Position = 0;
            var buffer = new byte[Marshal.SizeOf<TraceItemMetadata>()];
            for (int index = 0; index < _traceItemIndexHandle.Length; index += 20)
            {
                _traceItemIndexHandle.Position = index;
                _traceItemIndexHandle.Read(buffer);
                var info = LuanNiao.Core.StructUtilTools.StructUtilTools.ToStruct<TraceItemMetadata>(buffer);
                var dataBuffer = new byte[info.Length];
                _sliceHandle.Position = info.Position;
                var dataInfo = _sliceHandle.Read(dataBuffer);

                var info1 = JsonSerializer.Deserialize<Dictionary<string, string>>(Encoding.UTF8.GetString(dataBuffer));
            }
        }



        

    }
}
