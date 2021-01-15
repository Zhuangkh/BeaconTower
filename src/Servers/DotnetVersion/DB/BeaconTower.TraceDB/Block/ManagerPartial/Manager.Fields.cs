using BeaconTower.TraceDB.Block.Models;
using System.Collections.Generic;
using System.IO;
using SliceManager = BeaconTower.TraceDB.Slice.Manager;

namespace BeaconTower.TraceDB.Block
{
    internal partial class Manager
    {
        private readonly DirectoryInfo _blockDirectory;
        private BlockMetadata _metadata = new BlockMetadata();
        private readonly Dictionary<uint, SliceManager> _sliceLoop = new();
        private uint _currentSliceIndex;
        private readonly long _blockName;
        private FileStream _metadataFileHandle = null;

    }
}
