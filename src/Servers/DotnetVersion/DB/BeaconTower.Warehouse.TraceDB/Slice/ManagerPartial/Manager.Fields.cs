using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeaconTower.Warehouse.TraceDB.Slice.Models;

namespace BeaconTower.Warehouse.TraceDB.Slice
{
    internal partial class Manager
    {
        private readonly string _fileFullPath;
        private readonly long _fileName;
        public FileStream _handle;
        public long _fromTraceID;
        public long _toTraceID;
        private Metadata _metadata;        

    }
}
