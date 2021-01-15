using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RootManager = BeaconTower.TraceDB.Root.Manager;

namespace BeaconTower.TraceDB
{
    /// <summary>
    /// 时间索引临时创建即可,在内存中用于查找,可尝试使用跳表
    /// </summary>
    public class BTraceDB
    {

        public static readonly BTraceDB Instance = new();
        private readonly Dictionary<string, RootManager> _dbRootPool = new Dictionary<string, RootManager>();
        private bool _started = false;


        public void RegistDB(string alias, string path = null, string folderName = null)
        {
            _dbRootPool.Add(alias, new RootManager(path, folderName));
        }
        public void RegistDB()
        {
            _dbRootPool.Add("Default", new RootManager());
        }

        /// <summary>
        /// start he db server
        /// </summary>
        public void StartServer()
        {
            if (_started)
            {
                return;
            }
            lock (this)
            {
                if (_started)
                {
                    return;
                }
                var dbInitList = new Task[_dbRootPool.Count];
                for (int i = 0; i < _dbRootPool.Count; i++)
                {

                }
                var index = 0;
                foreach (var item in _dbRootPool)
                {
                    dbInitList[index] = Task.Run(() =>
                    {
                        item.Value.Init();
                    });
                }
                Task.WhenAll(dbInitList).Wait();
                _started = true;
            }
        }

        public RootManager this[string alias] => _dbRootPool.ContainsKey(alias) ? _dbRootPool[alias] : null;
        public RootManager Default => _dbRootPool.Count == 0 ? null : _dbRootPool.Values.ToList()[0];
    }
}
