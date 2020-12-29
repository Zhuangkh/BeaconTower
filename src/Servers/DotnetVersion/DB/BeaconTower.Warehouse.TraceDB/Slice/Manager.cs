using System.IO;

namespace BeaconTower.Warehouse.TraceDB.Slice
{
    internal partial class Manager
    {
        public Manager(string fileFullPath, long fileName)
        {
            _fileFullPath = fileFullPath;
            _fileName = fileName;
        }

        public partial void LoadOrCreate();

        public partial bool Available();
        public partial bool SaveItem(long traceID, byte[] data);
    }
}
