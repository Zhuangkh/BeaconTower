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
    internal class Manager
    {
        private readonly string _srouceFolder = null;
        private readonly Dictionary<string, NodeIDIndexHandler> _nodeIDIndexMap = new Dictionary<string, NodeIDIndexHandler>();
        private readonly List<NodeIDMapSummaryInfo> _nodeIDMapping = new List<NodeIDMapSummaryInfo>();
        public Manager(string sourceFolder)
        {
            _srouceFolder = sourceFolder;
        }
        public async void StartServer()
        {
            var directoryInfo = new DirectoryInfo(_srouceFolder);
            if (!directoryInfo.Exists)
            {
                throw new InvalidOperationException($"Folder:{_srouceFolder} not exists!");
            }
            var allFolder = directoryInfo.GetDirectories();
            var indexFolder = allFolder.FirstOrDefault(item => item.Name.Equals(Constants.IndexFolder));
            if (indexFolder == null)
            {
                indexFolder = directoryInfo.CreateSubdirectory(Constants.IndexFolder);
            }
            var allIndexFiles = indexFolder.GetFiles();
            var taskList = new List<Task>();
            foreach (var item in allIndexFiles)
            {
                var ext = item.Extension;
                if (ext.Equals(Constants.NodeIDIndexFileExtends))
                {
                    var nodeIDIndex = new NodeIDIndexHandler(item);
                    _nodeIDIndexMap.Add(item.Name, nodeIDIndex);
                    taskList.Add(nodeIDIndex.LoadAsync());
                }
                else if (ext.Equals(Constants.NodeIDMapFileExtends))
                {
                    taskList.Add(Task.Run(() =>
                    {
                        var handler = item.OpenRead();
                        handler.Position = 0;
                        while (handler.CanRead)
                        {
                            var headBuffer = new byte[sizeof(long) + sizeof(int)];
                            handler.Read(headBuffer);
                            var orignalIDLength = BitConverter.ToInt32(headBuffer.AsSpan().Slice(sizeof(long)));
                            var orignalIDBuffer = new byte[orignalIDLength];
                            _nodeIDMapping.Add(new NodeIDMapSummaryInfo()
                            {
                                AliasName = BitConverter.ToInt64(headBuffer),
                                OrignalIDLength = orignalIDLength,
                                OrignalID = Encoding.UTF8.GetString(orignalIDBuffer)
                            });
                        }
                    }));
                }
            }
            await Task.WhenAll(taskList);

        }
    }
}
