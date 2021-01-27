using BeaconTower.Client.Abstract;
using BeaconTower.TraceDB.NodeTraceDB.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodeIDIndexHandler = BeaconTower.TraceDB.NodeTraceDB.Index.NodeIndex.Handler;

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

        public List<NodeIDMapSummaryInfo> AllNodeID => _nodeIDMapping;
        public List<long> NodeTraceIDList(NodeIDMapSummaryInfo nodeInfo)
        {
            if (_nodeIDIndexMap.ContainsKey(nodeInfo.AliasName))
            {
                return _nodeIDIndexMap[nodeInfo.AliasName].TraceIDList.Distinct().ToList();
            }
            return default(List<long>);
        }

        public int NodeTraceItemCount(NodeIDMapSummaryInfo nodeInfo)
        {
            if (_nodeIDIndexMap.ContainsKey(nodeInfo.AliasName))
            {
                return _nodeIDIndexMap[nodeInfo.AliasName].TraceIDList.Distinct().Count();
            }
            return 0;
        }

        public List<PathMapSummaryInfo> AllPathInfo => _pathMapping;
        public List<long> PathTraceIDList(PathMapSummaryInfo pathInfo)
        {
            if (_pathIndexMap.ContainsKey(pathInfo.AliasName))
            {
                return _pathIndexMap[pathInfo.AliasName].TraceIDList;
            }
            return default(List<long>);
        }


        public bool TrySaveItem(NodeTracer item)
        {
            return _nodeTraceChannel.Writer.TryWrite(item);
        }
    }
}
