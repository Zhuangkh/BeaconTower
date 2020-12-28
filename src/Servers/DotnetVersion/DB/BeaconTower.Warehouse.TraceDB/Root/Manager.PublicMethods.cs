using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BeaconTower.Warehouse.TraceDB.Block;
using static BeaconTower.Warehouse.TraceDB.Root.CommonDefinition;
using BlockManager = BeaconTower.Warehouse.TraceDB.Block.Manager;

namespace BeaconTower.Warehouse.TraceDB.Root
{
    internal partial class Manager
    {

        public partial void Init()
        {
            if (_initialized)
            {
                return;
            }
            lock (_rootManagerLockKey)
            {
                if (_initialized)
                {
                    return;
                }
                var dicInfo = new DirectoryInfo(_rootFolder);
                var dicList = dicInfo.GetDirectories();
                for (int i = 0; i < dicList.Length; i++)
                {
                    var item = dicList[i];
                    var blockItem = Block.Tools.CreateBlockItem(item);
                }
                _initialized = true;
            }
        }

        public partial BlockInfo GetCurrentBlock()
        {
            if (_currentBlock!=null)
            {
                return _currentBlock;
            }
            lock (_rootManagerLockKey)
            {
                if (_currentBlock != null)
                {
                    return _currentBlock;
                }
                
            }
            return _currentBlock;
        }
    }
}
