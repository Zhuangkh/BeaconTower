using BeaconTower.Client.Abstract;
using BeaconTower.TraceDB.NodeTraceDB.Common;
using System.IO;
using IndexManager = BeaconTower.TraceDB.NodeTraceDB.Index.Manager;
using DBRoot = BeaconTower.TraceDB.Root.Manager;
using LuanNiao.JsonConverterExtends;

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
    }
}
