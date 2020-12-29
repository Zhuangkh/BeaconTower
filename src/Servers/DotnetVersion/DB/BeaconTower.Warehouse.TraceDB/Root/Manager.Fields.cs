using System;
using System.Collections.Generic;
using BlockManager = BeaconTower.Warehouse.TraceDB.Block.Manager;

namespace BeaconTower.Warehouse.TraceDB.Root
{
    internal partial class Manager
    {
        private readonly string _rootFolder;
        private readonly object _rootManagerLockKey = $"{nameof(Manager)}{Guid.NewGuid():N}";
        private readonly List<BlockManager> _allBlocks = new List<BlockManager>();
        private bool _initialized = false;
        private BlockManager _currentBlock = null;
    }
}
