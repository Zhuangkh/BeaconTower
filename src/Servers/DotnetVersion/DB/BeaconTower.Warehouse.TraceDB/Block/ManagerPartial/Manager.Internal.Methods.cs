using System;
using static BeaconTower.Warehouse.TraceDB.Block.BlockDefinitions;

namespace BeaconTower.Warehouse.TraceDB.Block
{
    internal partial class Manager
    {
        internal partial void LoadOrCreate()
        {
            InitMetadata();
            InitSliceItem();

            //Todo: load other info, for example: Index info.

        }

        //Todo: Change this method, can improve performance.
        internal partial bool SaveItem(long traceID, long timestamp, byte[] data)
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
            SaveMetadata();

            var targetSlice = _sliceLoop[(System.Threading.Interlocked.Increment(ref _currentSliceIndex) % Block_Maximum_Number_Of_Slice_Count)];
            /*
             * 到这里变慢与上面的那个数字的差距在于,block有一次IO落盘,把那个优化掉即可.
            |   Method |     Mean |     Error |    StdDev |
            |--------- |---------:|----------:|----------:|
            | SaveItem | 4.241 us | 0.1759 us | 0.5187 us |
             */


            //var targetSlice = _sliceLoop[(uint)(traceID % Block_Maximum_Number_Of_Slice_Count)];
            /*
            |   Method |     Mean |     Error |    StdDev |
            |--------- |---------:|----------:|----------:|
            | SaveItem | 3.927 us | 0.1691 us | 0.4985 us |
             */
            //return true;
            return targetSlice.SaveItem(traceID, timestamp, data);
        }
    }
}
