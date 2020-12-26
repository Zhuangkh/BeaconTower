﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BeaconTower.Warehouse.TraceDB.Block;
using static BeaconTower.Warehouse.TraceDB.Root.CommonDefinition;

namespace BeaconTower.Warehouse.TraceDB.Root
{
    internal partial class Manager
    {

        public async partial Task Init()
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
    }
}
