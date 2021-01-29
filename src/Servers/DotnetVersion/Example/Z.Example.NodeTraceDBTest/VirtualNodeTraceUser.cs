using BeaconTower.Client.Abstract;
using BeaconTower.TraceDB.NodeTraceDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Z.Example.NodeTraceDBTest
{
    public class VirtualNodeTraceUser
    {
        private readonly DBManager _db;
        public VirtualNodeTraceUser(string nodeName)
        {
            _db = DBManager.Instance.RegistNodeTraceDB(
                  nameof(VirtualNodeTraceUser)
                  , (new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName)
                 );
            ServerManager.Instance.Init(new BeaconTowerOptions()
            {
                NodeID = nodeName,
                NodeType = NodeType.WebServer
            });
        }
        public void InsertNodeInfo()
        {
            var test = ServerManager.Instance.CreateNodeTracer();
            _db.SaveItem(test);
        }
    }
}
