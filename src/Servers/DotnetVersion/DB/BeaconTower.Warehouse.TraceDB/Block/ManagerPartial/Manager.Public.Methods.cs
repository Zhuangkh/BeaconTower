using System;

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

        public partial bool SaveItem(long traceID, byte[] data)
        {
            if (_currentSlice == null)
            {
                lock (this)
                {
                    if (_currentSlice == null)
                    {
                        MoveNextAvailableSlice();
                    }
                }
            }
             _currentSlice.SaveItem(traceID, data);
            return true;
        }
    }
}
