using BeaconTower.Client.Abstract;
using System.Collections.Generic;
using System.IO;
using System.Threading.Channels;
using NodeIDIndexHandler = BeaconTower.TraceDB.NodeTraceDB.Index.NodeIndex.Handler;
using PathIndexHandler = BeaconTower.TraceDB.NodeTraceDB.Index.PathIndex.Handler;

namespace BeaconTower.TraceDB.NodeTraceDB.Index
{
    internal partial class Manager
    {
        private readonly string _srouceFolder = null;
        private readonly Channel<NodeTracer> _nodeTraceChannel = Channel.CreateUnbounded<NodeTracer>();
        



        private readonly Dictionary<long, NodeIDIndexHandler> _nodeIDIndexMap = new();
        private readonly List<NodeIDMapSummaryInfo> _nodeIDMapping = new();
        private FileStream _nodeIDMappingHandler;

        private readonly Dictionary<long, PathIndexHandler> _pathIndexMap = new();
        private readonly List<PathMapSummaryInfo> _pathMapping = new();
        private FileStream _pathMappingHandler;
    }
}
