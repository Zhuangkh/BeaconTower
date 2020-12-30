using System.Collections.Generic;
using System.IO;
using BeaconTower.Warehouse.TraceDB.Block.Models;
using SliceManager = BeaconTower.Warehouse.TraceDB.Slice.Manager;

namespace BeaconTower.Warehouse.TraceDB.Block
{
    internal partial class Manager
    {
        private readonly DirectoryInfo _blockDirectory;
        private BlockMetadata _metadata = null;
        private readonly Dictionary<uint, SliceManager> _sliceLoop = new();
        private uint _currentSliceIndex;

    }
}
