﻿using System;
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
    }
}
