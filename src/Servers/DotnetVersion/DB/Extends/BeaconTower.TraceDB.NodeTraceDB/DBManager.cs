using BeaconTower.Client.Abstract;
using BeaconTower.TraceDB.NodeTraceDB.Common;
using BeaconTower.TraceDB.NodeTraceDB.Index;
using LuanNiao.JsonConverterExtends;
using System.Collections.Generic;
using System.IO;
using DBRoot = BeaconTower.TraceDB.Root.Manager;
using IndexManager = BeaconTower.TraceDB.NodeTraceDB.Index.Manager;

namespace BeaconTower.TraceDB.NodeTraceDB
{
    public class DBManager
    {
        public static readonly DBManager Instance = new();
        private string _sourceFolder = "";
        private string _dbFolder = "";
        private string _alias = "";
        private DBRoot _dbRoot;

        private IndexManager _indexManager;

        public DBManager RegistNodeTraceDB(string projectName
            , string folderPath
            , string dbFolder = Constants.SourceDBFolder)
        {
            _alias = $"{projectName}{Constants.Suffix}";
            _sourceFolder = Path.Combine(folderPath
                , _alias);
            _dbFolder = dbFolder;
            _indexManager = new IndexManager(_sourceFolder);
            _dbRoot = DataBase.Instance.RegistDB(_alias
                , _sourceFolder
                , _dbFolder);
            return this;
        }

        public void StartServer()
        {
            DataBase.Instance.StartServer();
            _indexManager.StartServer();
        }


        public string Alias => _alias;
        public int UnhandledItemCount => _dbRoot.UnhandledItemCount;
        public bool State => _dbRoot.IsRunning;
        public string FolderPath => _dbRoot.FolderPath;
        public string FolderName => _dbRoot.FolderName;
        public int SliceCount => _dbRoot.SliceCount;
        public int BlockCount => _dbRoot.BlockCount;
        public int TraceItemCount => _dbRoot.TraceItemCount;

        public List<NodeIDMapSummaryInfo> AllNodeInfo => _indexManager.AllNodeID;
        public int NodeCount => _indexManager.AllNodeID.Count;

        public List<long> NodeTraceIDList(NodeIDMapSummaryInfo nodeInfo)
        {
            if (nodeInfo == null)
            {
                return default(List<long>);
            }
            return _indexManager.NodeTraceIDList(nodeInfo);
        }
        public List<long> GetTraceIDByPath(long pathAlias)
        {
            return _indexManager.PathTraceIDList(pathAlias);
        }
        public int NodeTraceItemCount(NodeIDMapSummaryInfo nodeInfo)
        {
            if (nodeInfo == null)
            {
                return 0;
            }
            return _indexManager.NodeTraceItemCount(nodeInfo);
        }

        public List<PathMapSummaryInfo> AllPathInfo => _indexManager.AllPathInfo;

        public int GetNodePathItemCount(long nodeAlias, long pathAlias)
        {
            return _indexManager.GetNodePathItemCount(nodeAlias, pathAlias);
        }


        public bool SaveItem(NodeTracer item)
        {
            if (item == null)
            {
                return false;
            }
            if (_indexManager.TrySaveItem(item))
            {
                return _dbRoot.SaveItem(item.TraceID, item.TimeStamp, item.GetBytes());
            }
            return false;
        }

        public bool TryGetNodeTraceItem(long traceID, out List<NodeTracer> nodeTracers)
        {
            nodeTracers = new List<NodeTracer>();
            if (_dbRoot.TryGetItem(traceID, out var items))
            {
                for (int i = 0; i < items.Count; i++)
                {
                    nodeTracers.Add(items[i].Data.GetObject<NodeTracer>());
                }
                return true;
            }
            return false;
        }
        public bool TryGetNodeTraceItemSummary(long traceID, out List<TraceItemSummary> data)
        {
            data = null;
            if (_dbRoot.TryGetItemMetadata(traceID, out var items))
            {
                data = items;
                return true;
            }
            data = new List<TraceItemSummary>();
            return false;
        }


        public bool TryGetRawTraceItem(long traceID, out List<TraceItem> traceItems)
        {
            return _dbRoot.TryGetItem(traceID, out traceItems);
        }
    }
}
