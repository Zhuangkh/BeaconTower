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

                _handle.Position = _metadata.CurrentPosition;
                _handle.Write(BitConverter.GetBytes(traceID));
                _handle.Write(BitConverter.GetBytes(data.Length));
                _handle.Write(data);
                _handle.Flush();
                SaveItemMetadataHandler(traceID, data);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
