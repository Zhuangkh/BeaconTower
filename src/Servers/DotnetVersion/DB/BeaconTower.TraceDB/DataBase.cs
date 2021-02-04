using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using RootManager = BeaconTower.TraceDB.Root.Manager;

namespace BeaconTower.TraceDB
{
    /// <summary>
    /// 时间索引临时创建即可,在内存中用于查找,可尝试使用跳表
    /// </summary>
    public class DataBase
    {

        public static readonly DataBase Instance = new();
        private readonly Dictionary<string, RootManager> _dbRootPool = new Dictionary<string, RootManager>();
        private bool _started = false;

        /// <summary>
        /// Regist the new database to instance 
        /// <para>You can reuse the root path use the 'path' parameter</para>
        /// </summary>
        /// <param name="alias">database's alias </param>
        /// <param name="path">set the db's root path</param>
        /// <param name="folderName">db's folder name</param>
        public RootManager RegistDB(string alias, string path = null, string folderName = null)
        {
            var db = new RootManager(path, folderName);
            _dbRootPool.Add(alias, db);
            return db;
        }

        /// <summary>
        /// regist the databse use the default settings
        /// <para>DB name: "Default"</para>
        /// <para>Root path: this dll's path</para>
        /// <para>folder name: "TraceDB"</para>
        /// </summary>
        public void RegistDB() => RegistDB(
                "Default"
                , (new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName)
                , "TraceDB");

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
             
                var index = 0;
                foreach (var item in _dbRootPool)
                {
                    dbInitList[index++] = Task.Run(() =>
                    {
                        item.Value.Init();
                    });
                }
                Task.WhenAll(dbInitList).Wait();
                _started = true;
            }
        }

        /// <summary>
        /// use the database's alias to find out the DB instance, you will get the null instance when we haven't this DB
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public RootManager this[string alias] => _dbRootPool.ContainsKey(alias) ? _dbRootPool[alias] : null;
        /// <summary>
        /// try to get the first db in the list.
        /// <para>you will get the null instance when we haven't any instance here.</para>
        /// </summary>
        public RootManager Default => _dbRootPool.Count == 0 ? null : _dbRootPool.Values.ToList()[0];
        /// <summary>
        /// all database instance alias
        /// </summary>
        public List<string> AliasList => _dbRootPool.Keys.ToList();
    }
}
