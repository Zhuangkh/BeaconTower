using System;
using static BeaconTower.Warehouse.TraceDB.Block.BlockDefinitions;

namespace BeaconTower.Warehouse.TraceDB.Block
{
    internal partial class Manager
    {
        public partial void LoadOrCreate()
        {
            _metadata = InitMetadata();
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
            _sliceLoop[(System.Threading.Interlocked.Increment(ref _currentSliceIndex) % Block_Maximum_Number_Of_Slice_Count)]
               .SaveItem(traceID, timestamp, data); 



            return true;
        }
    }
}
