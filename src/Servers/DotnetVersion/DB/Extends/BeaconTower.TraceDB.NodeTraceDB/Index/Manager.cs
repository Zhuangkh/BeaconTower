using BeaconTower.Client.Abstract;
using BeaconTower.TraceDB.NodeTraceDB.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodeIDIndexHandler = BeaconTower.TraceDB.NodeTraceDB.Index.NodeID.Handler;

namespace BeaconTower.TraceDB.NodeTraceDB.Index
{
    internal partial class Manager
    {
        public Manager(string sourceFolder)
        {
            _srouceFolder = sourceFolder;
        }
        public async void StartServer()
        {
            await InitAllFileInfo();
            StartConsumer();
        }

        public bool TrySaveItem(NodeTracer item)
        {
            return _nodeTraceChannel.Writer.TryWrite(item);
        }
    }
}
