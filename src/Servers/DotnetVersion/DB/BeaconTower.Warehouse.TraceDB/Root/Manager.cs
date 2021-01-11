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

        internal Manager()
        {
            _rootFolder = @"C:\Users\benla\Documents\GitHub\BeaconTower\src\Servers\DotnetVersion\Example\Z.Example.TraceDB\bin\Debug\net5.0\BTraceDB";// Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName, Default_Root_Folder_Name);
        }
        internal Manager(string path, string folderName)
        {
            throw new NotImplementedException($"Next version..");
        }

        internal partial void Init();

        internal partial bool SaveItem(long traceID, long timestamp, byte[] data);


        internal int BlockCount => _allBlocks.Count;

        internal List<BlockManager> Blocks => _allBlocks;

        public bool TryGetItem(long traceID, out List<TraceItem> data)
        {
            data = null;
            var targetBlock = GetTargetBlock(traceID);
            if (targetBlock == null)
            {
                return false;
            }
            data= targetBlock.GetTraceItems(traceID);
            return true;
        }
    }
}
