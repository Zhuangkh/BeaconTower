using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BeaconTower.Warehouse.TraceDB.Block;

namespace BeaconTower.Warehouse.TraceDB.Root
{
    internal partial class Manager
    {
        private readonly string _rootFolder;
        private readonly List<BlockMetadata> _blocks = new List<BlockMetadata>();
        private readonly object _rootManagerLockKey = $"{nameof(Manager)}{Guid.NewGuid().ToString("N")}";
        private bool _initialized = false;
        private BlockInfo _currentBlock = null;

    }
}
