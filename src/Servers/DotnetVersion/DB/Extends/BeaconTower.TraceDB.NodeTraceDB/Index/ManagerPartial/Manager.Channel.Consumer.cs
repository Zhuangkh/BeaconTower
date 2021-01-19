using BeaconTower.Client.Abstract;
using BeaconTower.TraceDB.NodeTraceDB.Common;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BeaconTower.TraceDB.NodeTraceDB.Index
{
    internal partial class Manager
    {
        private void StartConsumer()
        {
            Thread t = new Thread(new ThreadStart(async () =>
            {
                while (true)
                {
                    var item = await _nodeTraceChannel.Reader.ReadAsync();
                    await SaveNodeIDInfo(item);
                }
            }))
            {
                IsBackground = true
            };
            t.Start();
        }

        /// <summary>
        /// save the node id info
        /// </summary>
        private async Task SaveNodeIDInfo(NodeTracer info)
        {
            NodeIDMapSummaryInfo summaryInfo = null;
            lock (_nodeIDMapping)
            {
                var data = _nodeIDMapping.FirstOrDefault(item => item.OrignalID.Equals(info.NodeID));
                if (data != null)
                {
                    _nodeIDIndexMap[data.AliasName].SaveNewItem(info.TraceID);
                    return;
                }
                summaryInfo = new()
                {
                    AliasName = LuanNiao.Core.IDGen.GetInstance().NextId(),
                    OrignalID = info.NodeID,
                    OrignalIDLength = Encoding.UTF8.GetBytes(info.NodeID).Length
                };
                _nodeIDMapping.Add(summaryInfo);
            }
            lock (_nodeIDMappingHandler)
            {
                _nodeIDMappingHandler.Position = _nodeIDMappingHandler.Length;
                _nodeIDMappingHandler.Write(BitConverter.GetBytes(summaryInfo.AliasName));
                _nodeIDMappingHandler.Write(BitConverter.GetBytes(summaryInfo.OrignalIDLength));
                _nodeIDMappingHandler.Write(Encoding.UTF8.GetBytes(summaryInfo.OrignalID));
                _nodeIDMappingHandler.Flush();
            }
            var indexFile = new FileInfo(Path.Combine(_srouceFolder, Constants.IndexFolder, $"{summaryInfo.AliasName}{Constants.NodeIDIndexFileExtends}"));
            var indexHandler = new NodeID.Handler(indexFile);
            await indexHandler.LoadAsync(); 
            _nodeIDIndexMap.Add(summaryInfo.AliasName, indexHandler.SaveNewItem(info.TraceID));
        }
    }
}
