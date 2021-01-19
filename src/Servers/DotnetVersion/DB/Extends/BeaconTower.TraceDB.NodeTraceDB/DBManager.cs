using BeaconTower.TraceDB.NodeTraceDB.Common;
using System.IO;
using IndexManager = BeaconTower.TraceDB.NodeTraceDB.Index.Manager;

namespace BeaconTower.TraceDB.NodeTraceDB
{
    public class DBManager
    {
        public static DBManager Instance = new();
        private string _sourceFolder = "";
        private string _dbFolder = "";

        private IndexManager _indexManager;

        public DBManager RegistNodeTraceDB(string projectName
            , string folderPath
            , string dbFolder = Constants.SourceDBFolder)
        {
            _sourceFolder = Path.Combine(folderPath
                , $"{projectName}{Constants.Suffix}");
            _dbFolder = dbFolder;
            _indexManager = new IndexManager(_sourceFolder);
            DataBase.Instance.RegistDB(projectName
                , _sourceFolder
                , _dbFolder);
            return this;
        }

        public void StartServer()
        {
            DataBase.Instance.StartServer();
            _indexManager.StartServer();
        }
    }
}
