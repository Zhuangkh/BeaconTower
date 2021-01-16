using BeaconTower.TraceDB.NodeTraceDB.Common;
using System;
using System.IO;

namespace BeaconTower.TraceDB.NodeTraceDB
{
    public class DBManager
    {
        public static DBManager Instance = new();
        private string _sourceFolder = "";
        private string _dbFolder = "";

        public DBManager RegistNodeTraceDB(string projectName
            , string folderPath
            , string dbFolder = Constants.SourceDBFolder)
        {
            _sourceFolder = Path.Combine(folderPath
                , $"{projectName}{Constants.Suffix}");
            _dbFolder = dbFolder;
            DataBase.Instance.RegistDB(projectName
                , _sourceFolder
                , _dbFolder);
            return this;
        }

        public void StartServer()
        {
            DataBase.Instance.StartServer();
        }
    }
}
