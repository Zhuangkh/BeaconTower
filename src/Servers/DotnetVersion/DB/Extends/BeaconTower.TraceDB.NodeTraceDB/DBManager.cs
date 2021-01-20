using BeaconTower.Client.Abstract;
using BeaconTower.TraceDB.NodeTraceDB.Common;
using System.IO;
using IndexManager = BeaconTower.TraceDB.NodeTraceDB.Index.Manager;
using DBRoot = BeaconTower.TraceDB.Root.Manager;
using LuanNiao.JsonConverterExtends;
using System.Collections.Generic;
using BeaconTower.TraceDB.NodeTraceDB.Index;

namespace BeaconTower.TraceDB.NodeTraceDB
{
    public class DBManager
    {
        public static readonly DBManager Instance = new();
        private string _sourceFolder = "";
        private string _dbFolder = "";
        private DBRoot _dbRoot;

        private IndexManager _indexManager;

        public DBManager RegistNodeTraceDB(string projectName
            , string folderPath
            , string dbFolder = Constants.SourceDBFolder)
        {
            _sourceFolder = Path.Combine(folderPath
                , $"{projectName}{Constants.Suffix}");
            _dbFolder = dbFolder;
            _indexManager = new IndexManager(_sourceFolder);
            _dbRoot = DataBase.Instance.RegistDB(projectName
                , _sourceFolder
                , _dbFolder);
            return this;
        }

        public void StartServer()
        {
            DataBase.Instance.StartServer();
            _indexManager.StartServer();
        }

        public List<NodeIDMapSummaryInfo> AllNodeInfo => _indexManager.AllNodeID;

        public List<long> NodeTraceIDList(NodeIDMapSummaryInfo nodeInfo)
        {
            if (nodeInfo == null)
            {
                return default(List<long>);
            }
            return _indexManager.NodeTraceIDList(nodeInfo);
        }

        public List<PathMapSummaryInfo> AllPathInfo => _indexManager.AllPathInfo;




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

        public bool TryGetRawTraceItem(long traceID,out List<TraceItem> traceItems)
        {            
            return _dbRoot.TryGetItem(traceID, out traceItems);            
        }
    }
}
