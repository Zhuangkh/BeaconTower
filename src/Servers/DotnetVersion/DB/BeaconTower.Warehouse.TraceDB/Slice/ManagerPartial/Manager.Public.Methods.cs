using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BeaconTower.Warehouse.TraceDB.Slice.MetadataDefinitions;
using static BeaconTower.Warehouse.TraceDB.Slice.SliceDefinitions;

namespace BeaconTower.Warehouse.TraceDB.Slice
{
    internal partial class Manager
    {
        public partial void LoadOrCreate()
        {
            _sliceHandle = new FileInfo(Path.Combine(_fileFullPath, $"{_fileName}{Slice_File_Extension}")).Open(FileMode.OpenOrCreate, FileAccess.ReadWrite);
            _traceItemIndexHandle = new FileInfo(Path.Combine(_fileFullPath, $"{_fileName}{SliceItem_Index_FileName}")).Open(FileMode.OpenOrCreate, FileAccess.ReadWrite);
            InitMetadata();
        }

       
        public partial void Close()
        {
            _sliceHandle.Flush();
            _sliceHandle.Close();
            _sliceHandle.Dispose();
            _traceItemIndexHandle.Flush();
            _traceItemIndexHandle.Close();
            _traceItemIndexHandle.Dispose();
            _sliceHandle = null;
        }

        public partial bool SaveItem(long traceID, long timeStamp, byte[] data)
        {
            /*
                |   Method |     Mean |   Error |  StdDev |
                |--------- |---------:|--------:|--------:|
                | SaveItem | 244.1 ns | 0.86 ns | 0.71 ns |
             */
            //Console.WriteLine(this._fileName);
            return true;
            try
            {
                _sliceHandle.Position = _metadata.CurrentPosition;
                _sliceHandle.Write(BitConverter.GetBytes(traceID));
                _sliceHandle.Write(BitConverter.GetBytes(timeStamp));
                _sliceHandle.Write(BitConverter.GetBytes(data.Length));
                _sliceHandle.Write(data);
                _sliceHandle.Flush();
                SaveItemMetadataHandler(traceID, data);
                SaveTraceItemInfo(traceID, timeStamp, data);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
