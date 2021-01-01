using System;
using static BeaconTower.Warehouse.TraceDB.Block.BlockDefinitions;

namespace BeaconTower.Warehouse.TraceDB.Block
{
    internal partial class Manager
    {
        public partial void LoadOrCreate()
        {
            InitMetadata();
            InitSliceItem();

            //Todo: load other info, for example: Index info.

        }

        //Todo: Change this method, can improve performance.
        public partial bool SaveItem(long traceID, long timestamp, byte[] data)
        {
            /*
                |   Method |     Mean |   Error |  StdDev |
                |--------- |---------:|--------:|--------:|
                | SaveItem | 243.0 ns | 0.45 ns | 0.42 ns |
             */


            if (traceID > _metadata.ToTraceID && _metadata.CurrentItemsCount > Block_TraceItem_Maximum)
            {
                return false;
            }
            if (_metadata.FromTraceID == 0 && _metadata.ToTraceID == 0)
            {
                _metadata.FromTraceID = _metadata.ToTraceID = traceID;
            }
            else if (traceID > _metadata.ToTraceID && _metadata.CurrentItemsCount < Block_TraceItem_Maximum)
            {
                _metadata.ToTraceID = traceID;
            }

            _metadata.CurrentItemsCount++;


            var targetSlice = _sliceLoop[(System.Threading.Interlocked.Increment(ref _currentSliceIndex) % Block_Maximum_Number_Of_Slice_Count)];
            return targetSlice.SaveItem(traceID, timestamp, data);
        }
    }
}
