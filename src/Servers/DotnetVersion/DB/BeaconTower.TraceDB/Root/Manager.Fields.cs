using System;
using System.Collections.Generic;
using BlockManager = BeaconTower.TraceDB.Block.Manager;

namespace BeaconTower.TraceDB.Root
{
    public partial class Manager
    {
        private readonly string _rootFolder;
        private readonly List<BlockManager> _allBlocks = new();
        private bool _initialized = false; 
    }
}
