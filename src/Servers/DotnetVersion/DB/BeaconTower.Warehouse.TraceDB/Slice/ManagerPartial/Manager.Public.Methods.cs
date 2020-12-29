using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BeaconTower.Warehouse.TraceDB.Slice.MetadataDefinitions;

namespace BeaconTower.Warehouse.TraceDB.Slice
{
    internal partial class Manager
    {
        public partial void LoadOrCreate()
        {
            _handle = new FileInfo(_fileFullPath).Open(FileMode.OpenOrCreate, FileAccess.ReadWrite);
            InitMetadata();
        }

        public partial bool Available()
        {
            return _metadata.TraceItemCount < Block.BlockDefinitions.Block_One_Block_Item_TraceItem_Maximum;
        }

        public partial bool SaveItem(long traceID, byte[] data)
        {
            try
            {
                if (_handle.Position < Metadata_Head_Size)
                {
                    _handle.Position = Metadata_Head_Size;
                }
                _handle.Write(data);
                _handle.Flush();
                _metadata.FromTraceID = traceID < _metadata.FromTraceID ? traceID : _metadata.FromTraceID;
                _metadata.ToTraceID = traceID > _metadata.ToTraceID ? traceID : _metadata.ToTraceID; 
                SaveMetadataInfo();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
