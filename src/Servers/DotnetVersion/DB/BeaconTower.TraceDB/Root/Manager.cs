using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BeaconTower.TraceDB.Root
{
    public partial class Manager
    {

        internal Manager(string path, string folderName)
        {
            FolderPath = path;
            FolderName = folderName;
            _rootFolder = Path.Combine(path, folderName);
        }


        internal partial void Init();

        public partial bool SaveItem(long traceID, long timestamp, byte[] data);
        public partial bool TryGetItem(long traceID, out List<TraceItem> data);


        public int BlockCount => _allBlocks.Count;
        /// <summary>
        /// this db instance's slice count info
        /// <para>this method will provisionally calculate all data!</para>
        /// </summary>
        public int SliceCount => _allBlocks.Sum(item => item.SliceCount);
        /// <summary>
        /// this db instance's trace item count
        /// <para>this method will provisionally calculate all data!</para>
        /// </summary>
        public int TraceItemCount => _allBlocks.Sum(item => item.SliceCount);
        public string FolderPath { get; private set; }
        public string FolderName { get; private set; }
        public IList<long> AllTraceID
        {
            get
            {
                IList<long> result = new List<long>();

                foreach (var item in _allBlocks)
                {
                    var allItems = item.TraceIDs;
                    foreach (var idItem in allItems)
                    {
                        result.Add(idItem);
                    }
                }
                return result;
            }
        }

    }
}
