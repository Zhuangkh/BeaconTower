using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using BeaconTower.Warehouse.TraceDB.Slice.Models;

namespace BeaconTower.Warehouse.TraceDB.Slice
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


        public FileStream _sliceHandle;
        public FileStream _traceItemIndexHandle;


    }
}
