using System.IO;

namespace BeaconTower.Warehouse.TraceDB.Block
{
    /// <summary>
    /// TraceDB block manager
    /// </summary>
    internal partial class Manager
    {

        public Manager(DirectoryInfo directoryInfo)
        {
            this._blockDirectory = directoryInfo;
        }

        /// <summary>
        /// load or create this block
        /// <para>Will create the metadata or other init file when file not exists</para>
        /// </summary>
        public partial void LoadOrCreate();

        public partial bool SaveItem(long traceID, byte[] data);

    }
}
