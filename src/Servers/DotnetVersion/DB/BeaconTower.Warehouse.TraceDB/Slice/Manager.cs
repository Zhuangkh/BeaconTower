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

        /// <summary>
        /// load or create the slice
        /// </summary>
        public partial void LoadOrCreate();
        /// <summary>
        /// close this slice 
        /// </summary>
        public partial void Close();
        /// <summary>
        /// save the trace item to db
        /// </summary>
        /// <returns></returns>
        public partial bool SaveItem(long traceID,long timeStamp, byte[] data);
    }
}
