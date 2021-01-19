using BeaconTower.Client.Abstract;
using System.Collections.Generic;
using System.IO;
using System.Threading.Channels;
using NodeIDIndexHandler = BeaconTower.TraceDB.NodeTraceDB.Index.NodeID.Handler;

namespace BeaconTower.TraceDB.NodeTraceDB.Index
{
    internal partial class Manager
    {
        private readonly string _srouceFolder = null;        
        private readonly Dictionary<long, NodeIDIndexHandler> _nodeIDIndexMap = new Dictionary<long, NodeIDIndexHandler>();
        private readonly List<NodeIDMapSummaryInfo> _nodeIDMapping = new List<NodeIDMapSummaryInfo>();
        private readonly Channel<NodeTracer> _nodeTraceChannel = Channel.CreateUnbounded<NodeTracer>();
        private FileStream _nodeIDMappingHandler;
    }
}
