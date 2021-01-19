using BeaconTower.TraceDB.Slice.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Channels;

namespace BeaconTower.TraceDB.Slice
{
    internal partial class Manager
    {
        private class SaveRequestItem
        {
            public long TraceID { get; set; }
            public long Timestamp { get; set; }
            public byte[] Data { get; set; }
        }

        private readonly string _fileFullPath;
        private readonly long _fileName;
        private Metadata _metadata;

        /// <summary>
        /// hahahahha Unbounded!!!!!!!!!!!
        /// </summary>
        private readonly Channel<SaveRequestItem> _saveItemChannel = Channel.CreateUnbounded<SaveRequestItem>();

        private readonly List<TraceItemMetadata> _traceItemsInfo = new List<TraceItemMetadata>();


        private FileStream _sliceHandle;
        private FileStream _traceItemIndexHandle;

        /// <summary>
        /// this slice's trace item count
        /// <para>this method will provisionally calculate all data!</para>
        /// </summary>
        internal int ItemCount => _traceItemsInfo.Count;

        internal IList<long> TraceIDs => _traceItemsInfo.Select(item => item.TraceID).ToList();

    }
}
