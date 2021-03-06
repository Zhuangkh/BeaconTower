using BeaconTower.TraceDB.Slice.Models;
using System;
using System.Collections.Generic;
using System.IO;
using static BeaconTower.TraceDB.Slice.SliceDefinitions;

namespace BeaconTower.TraceDB.Slice
{
    internal partial class Manager
    {
        internal partial void LoadOrCreate()
        {
            _sliceHandle = new FileInfo(Path.Combine(_fileFullPath, $"{_fileName}{Slice_File_Extension}")).Open(FileMode.OpenOrCreate, FileAccess.ReadWrite);
            _traceItemIndexHandle = new FileInfo(Path.Combine(_fileFullPath, $"{_fileName}{SliceItem_Index_FileName}")).Open(FileMode.OpenOrCreate, FileAccess.ReadWrite);
            InitMetadata();
        }


        internal partial void Close()
        {
            throw new NotSupportedException();
            //_sliceHandle.Flush();
            //_sliceHandle.Close();
            //_sliceHandle.Dispose();
            //_traceItemIndexHandle.Flush();
            //_traceItemIndexHandle.Close();
            //_traceItemIndexHandle.Dispose();
            //_sliceHandle = null;
        }

        internal partial bool SaveItem(long traceID, long timeStamp, byte[] data)
        {
            try
            {
                return _saveItemChannel.Writer.TryWrite(new SaveRequestItem()
                {
                    Data = data,
                    Timestamp = timeStamp,
                    TraceID = traceID
                });
            }
            catch
            {
                return false;
            }
            ///*
            //    |   Method |     Mean |   Error |  StdDev |
            //    |--------- |---------:|--------:|--------:|
            //    | SaveItem | 244.1 ns | 0.86 ns | 0.71 ns |
            // */
            //try
            //{
            //    //save index first
            //    SaveTraceItemInfo(_metadata.CurrentPosition, traceID, timeStamp, data);

            //    _sliceHandle.Position = _metadata.CurrentPosition;
            //    _sliceHandle.Write(data);
            //    _sliceHandle.Flush();
            //    SaveItemMetadataHandler(data);
            //    /*
            //     Have not query             
            //    |   Method |     Mean |    Error |   StdDev |
            //    |--------- |---------:|---------:|---------:|
            //    | SaveItem | 21.02 us | 0.415 us | 0.995 us |

            //     */
            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }


        internal partial List<TraceItemMetadata> GetTraceItemsMetadata(long traceID)
        {
            List<TraceItemMetadata> targetIndex = new();
            lock (_traceItemsInfo)
            {
                for (int i = 0; i < _traceItemsInfo.Count; i++)
                {
                    if (_traceItemsInfo[i].TraceID == traceID)
                    {
                        targetIndex.Add(_traceItemsInfo[i]);
                    }
                }
            }
            return targetIndex;
        }

        internal partial List<TraceItem> GetTraceItems(long traceID)
        {
            List<TraceItemMetadata> targetIndex = new();
            lock (_traceItemsInfo)
            {
                for (int i = 0; i < _traceItemsInfo.Count; i++)
                {
                    if (_traceItemsInfo[i].TraceID == traceID)
                    {
                        targetIndex.Add(_traceItemsInfo[i]);
                    }
                }
            }
            var res = new List<TraceItem>();
            if (targetIndex.Count == 0)
            {
                return res;
            }
            foreach (var item in targetIndex)
            {
                TraceItem temp = new()
                {
                    TraceID = item.TraceID,
                    TimeStamp = item.TimeStamp,
                    Data = new byte[item.Length]
                };
                lock (_sliceHandle)
                {
                    _sliceHandle.Position = item.Position;
                    _sliceHandle.Read(temp.Data);
                }
                res.Add(temp);
            }
            return res;
        }
    }
}
