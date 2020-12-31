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

        public Manager()
        {
            _rootFolder = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName, Default_Root_Folder_Name);
        }
        public Manager(string path, string folderName)
        {
            throw new NotImplementedException($"Next version..");
        }

        public partial void Init();

        public partial bool SaveItem(long traceID, long timestamp, byte[] data);


    }
}
