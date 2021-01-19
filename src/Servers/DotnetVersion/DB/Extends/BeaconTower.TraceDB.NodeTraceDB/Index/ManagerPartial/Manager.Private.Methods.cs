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
        /// <summary>
        /// Init all file info
        /// </summary>
        /// <returns></returns>
        private async Task InitAllFileInfo()
        {
            var directoryInfo = new DirectoryInfo(_srouceFolder);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
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
                    taskList.Add(NodeIDIndexInit(item));
                }
                else if (ext.Equals(Constants.NodeIDMapFileExtends))
                {
                    taskList.Add(NodeIDMapInit(item));
                }
            }
            await Task.WhenAll(taskList);
            if (_nodeIDMappingHandler == null)
            {
                InitNodeIDMap();
            }
        }

        /// <summary>
        /// Load node id index file
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private Task NodeIDIndexInit(FileInfo item)
        {
            if (!long.TryParse(item.Name.Replace(Constants.NodeIDIndexFileExtends,""), out var nid))
            {
                return Task.Run(() => { });
            }
            var nodeIDIndex = new NodeIDIndexHandler(item);
            _nodeIDIndexMap.Add(nid, nodeIDIndex);
            return nodeIDIndex.LoadAsync();
        }
        private async void InitNodeIDMap()
        {
            await Task.Run(() =>
            {
                _nodeIDMappingHandler = new FileInfo(Path.Combine(_srouceFolder, Constants.IndexFolder, $"NodeIDMap{Constants.NodeIDMapFileExtends}")).Open(FileMode.OpenOrCreate, FileAccess.ReadWrite);
            });
        }
        /// <summary>
        /// load node id mapping file
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private Task NodeIDMapInit(FileInfo item)
        {
            return Task.Run(() =>
            {
                _nodeIDMappingHandler = item.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite);
                _nodeIDMappingHandler.Position = 0;
                for (int i = 0; i < _nodeIDMappingHandler.Length;)
                {
                    var headBuffer = new byte[sizeof(long) + sizeof(int)];
                    _nodeIDMappingHandler.Read(headBuffer);
                    var orignalIDLength = BitConverter.ToInt32(headBuffer.AsSpan()[sizeof(long)..]);
                    var orignalIDBuffer = new byte[orignalIDLength];
                    _nodeIDMappingHandler.Read(orignalIDBuffer);
                    _nodeIDMapping.Add(new NodeIDMapSummaryInfo()
                    {
                        AliasName = BitConverter.ToInt64(headBuffer),
                        OrignalIDLength = orignalIDLength,
                        OrignalID = Encoding.UTF8.GetString(orignalIDBuffer)
                    });
                    i += headBuffer.Length + orignalIDLength;
                }
            });
        }
    }
}
