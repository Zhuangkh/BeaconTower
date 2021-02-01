using BeaconTower.TraceDB.NodeTraceDB.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodeIDIndexHandler = BeaconTower.TraceDB.NodeTraceDB.Index.NodeIndex.Handler;
using PathIndexHandler = BeaconTower.TraceDB.NodeTraceDB.Index.PathIndex.Handler;

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
            taskList.Add(NodeIDMapInit(indexFolder));
            taskList.Add(PathMapInit(indexFolder));
            foreach (var item in allIndexFiles)
            {
                var task = item.Extension switch
                {
                    Constants.NodeIDIndexFileExtends => NodeIDIndexInit(item),
                    Constants.PathIndexFileExtends => PathIndexInit(item),
                    _ => null
                };

                if (task != null)
                {
                    taskList.Add(task);
                }
            }
            await Task.WhenAll(taskList);
        }

        /// <summary>
        /// Load node id index file
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private Task NodeIDIndexInit(FileInfo item)
        {
            if (!long.TryParse(item.Name.Replace(Constants.NodeIDIndexFileExtends, ""), out var nid))
            {
                return Task.Run(() => { });
            }
            var nodeIDIndex = new NodeIDIndexHandler(item);
            _nodeIDIndexMap.Add(nid, nodeIDIndex);
            return nodeIDIndex.LoadAsync();
        }

        /// <summary>
        /// load node id mapping file
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private Task NodeIDMapInit(DirectoryInfo dirInfo)
        {
            return Task.Run(() =>
            {
                var fileInfo = dirInfo.GetFiles().FirstOrDefault(item => item.Extension.Equals(Constants.NodeIDMapFileExtends));
                if (fileInfo == null)
                {
                    fileInfo = new FileInfo(Path.Combine(dirInfo.FullName, $"NodeIDMap{Constants.NodeIDMapFileExtends}"));
                }
                _nodeIDMappingHandler = fileInfo.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite);
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


        /// <summary>
        /// Load path id index file
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private Task PathIndexInit(FileInfo item)
        {
            if (!long.TryParse(item.Name.Replace(Constants.PathIndexFileExtends, ""), out var pid))
            {
                return Task.Run(() => { });
            }
            var pathIndex = new PathIndexHandler(item);
            _pathIndexMap.Add(pid, pathIndex);
            return pathIndex.LoadAsync();
        }

        /// <summary>
        /// load path id mapping file
        /// </summary>
        /// <param name="dirInfo">the folder info where we will to find the index file</param>
        /// <returns></returns>
        private Task PathMapInit(DirectoryInfo dirInfo)
        {
            return Task.Run(() =>
            {
                var fileInfo = dirInfo.GetFiles().FirstOrDefault(item => item.Extension.Equals(Constants.PathMapFileExtends));
                if (fileInfo == null)
                {
                    fileInfo = new FileInfo(Path.Combine(dirInfo.FullName, $"PathMap{Constants.PathMapFileExtends}"));
                }
                _pathMappingHandler = fileInfo.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite);
                _pathMappingHandler.Position = 0;
                for (int i = 0; i < _pathMappingHandler.Length;)
                {
                    var headBuffer = new byte[sizeof(long) + sizeof(long) + sizeof(int)];
                    _pathMappingHandler.Read(headBuffer);
                    var orignalPathLength = BitConverter.ToInt32(headBuffer.AsSpan()[sizeof(long)..]);
                    var orignalIDBuffer = new byte[orignalPathLength];
                    _pathMappingHandler.Read(orignalIDBuffer);
                    _pathMapping.Add(new PathMapSummaryInfo()
                    {
                        AliasName = BitConverter.ToInt64(headBuffer),
                        OrignalPathLength = orignalPathLength,
                        NodeAliasName = BitConverter.ToInt64(headBuffer.AsSpan()[(sizeof(long)/*AliasName*/+ sizeof(int)/*OrignalPathLength*/)..]),
                        OrignalPath = Encoding.UTF8.GetString(orignalIDBuffer)
                    });
                    i += headBuffer.Length + orignalPathLength;
                }
            });
        }
    }
}
