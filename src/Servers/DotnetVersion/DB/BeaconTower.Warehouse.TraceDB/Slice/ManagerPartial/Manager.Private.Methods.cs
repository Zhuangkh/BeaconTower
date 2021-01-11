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
            _sliceHandle.Position = 0;
            if (_sliceHandle.Length == 0)
            {
                if (_sliceHandle.Length == 0)
                {
                    CreateNewMetaData();
                }
            }
            else
            {
                LoadMetadataInfo();
            }
            LoadIndexInfo();
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
            lock (_sliceHandle)
            {
                _sliceHandle.Position = 0;
                _sliceHandle.Read(headBuffer);
            }
            _metadata = LuanNiao.Core.StructUtilTools.StructUtilTools.ToStruct<Metadata>(headBuffer);

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
            lock (_sliceHandle)
            {
                _sliceHandle.Position = 0;
                _sliceHandle.Write(buffer);
                _sliceHandle.Flush();
            }
        }

        /// <summary>
        /// save trace item info to index file
        /// </summary>
        /// <param name="data"></param>
        private void SaveTraceItemInfo(long position, long traceID, long timestamp, byte[] data)
        {
            TraceItemMetadata info = new TraceItemMetadata();
            info.Position = position;
            info.TraceID = traceID;
            info.TimeStamp = timestamp;
            info.Length = data.Length;
            lock (_traceItemIndexHandle)
            {
                _traceItemIndexHandle.Position = _traceItemIndexHandle.Length;
                _traceItemIndexHandle.Write(LuanNiao.Core.StructUtilTools.StructUtilTools.ToData(in info));
                _traceItemIndexHandle.Flush();
            }
            lock (_traceItemsInfo)
            {
                _traceItemsInfo.Add(info);
            }
        }

        /// <summary>
        /// load all trace item index info
        /// </summary>
        private void LoadIndexInfo()
        {
            var indexItemSize = Marshal.SizeOf<TraceItemMetadata>();
            var buffer = new byte[indexItemSize];
            for (int index = 0; index < _traceItemIndexHandle.Length; index += indexItemSize)
            {
                lock (_traceItemIndexHandle)
                {
                    _traceItemIndexHandle.Position = index;
                    _traceItemIndexHandle.Read(buffer);
                }
                var info = LuanNiao.Core.StructUtilTools.StructUtilTools.ToStruct<TraceItemMetadata>(buffer);
                _traceItemsInfo.Add(info);
            }
        }
    }
}
