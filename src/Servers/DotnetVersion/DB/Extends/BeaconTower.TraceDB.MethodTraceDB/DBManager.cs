using BeaconTower.Client.Abstract;
using BeaconTower.TraceDB.MethodTraceDB.Common;
using LuanNiao.JsonConverterExtends;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DBRoot = BeaconTower.TraceDB.Root.Manager;
using IndexManager = BeaconTower.TraceDB.MethodTraceDB.Index.Manager;

namespace BeaconTower.TraceDB.MethodTraceDB
{
    /// <summary>
    /// Method的DB目前就做一个壳子,用于未来扩展好了呀~~~ヾ(。￣□￣)ﾂ゜゜゜
    /// </summary>
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
            _alias = projectName;
            _sourceFolder = Path.Combine(folderPath
                , $"{projectName}{Constants.Suffix}");
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
        public bool SaveItem(MethodTracer item)
        {
            if (item == null)
            {
                return false;
            }
            //if (_indexManager.TrySaveItem(item))
            //{
            //    return _dbRoot.SaveItem(item.TraceID, item.TimeStamp, item.GetBytes());
            //}
            //return false;
            return _dbRoot.SaveItem(item.TraceID, item.TimeStamp, item.GetBytes());
        }

        public bool TryGetMethodTraceItem(long traceID, out List<MethodTracer> nodeTracers)
        {
            nodeTracers = new List<MethodTracer>();
            if (_dbRoot.TryGetItem(traceID, out var items))
            {
                for (int i = 0; i < items.Count; i++)
                {
                    nodeTracers.Add(items[i].Data.GetObject<MethodTracer>());
                }
                return true;
            }
            return false;
        }
        public bool TryGetMethodTraceItemSummary(long traceID, out List<TraceItemSummary> data)
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
