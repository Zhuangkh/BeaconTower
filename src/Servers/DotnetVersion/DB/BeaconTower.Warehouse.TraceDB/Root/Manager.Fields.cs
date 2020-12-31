using System;
using System.Collections.Generic;
using BlockManager = BeaconTower.Warehouse.TraceDB.Block.Manager;

namespace BeaconTower.Warehouse.TraceDB.Root
{
    internal partial class Manager
    {
        private readonly string _rootFolder;
        private readonly List<BlockManager> _allBlocks = new();
        private bool _initialized = false; 
    }
}
