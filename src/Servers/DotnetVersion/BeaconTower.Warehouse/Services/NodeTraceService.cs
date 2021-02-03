using BeaconTower.Client.Abstract;
using BeaconTower.Protocol;
using Grpc.Core;
using System.Linq;
using System.Threading.Tasks;
using NodeTraceDBManager = BeaconTower.TraceDB.NodeTraceDB.DBManager;

namespace BeaconTower.Warehouse.Services
{
    public class NodeTraceService : NodeTrace.NodeTraceBase
    {
        private readonly NodeTraceDBManager _dbInstance;
        public NodeTraceService(NodeTraceDBManager nodeTrceDB)
        {
            _dbInstance = nodeTrceDB;
        }
        public override Task<NullResponse> AfterNodeActived(NodeActiveRequest request, ServerCallContext context)
        {
            return Task.Run(() =>
            {
                var item = new NodeTracer()
                {
                    NodeID = request.NodeID,
                    Path = request.Path,
                    PreviousEventID = request.PreviousEventID,
                    EventID = request.EventID,
                    QueryString = request.QueryString,
                    TimeStamp = request.TimeStamp,
                    TraceID = request.TraceID,
                    Type = (NodeType)request.NodeType
                };
                if (request.CustomData != null)
                {
                    var keys = request.CustomData.Keys.ToArray();
                    for (int i = 0; i < keys.Length; i++)
                    {
                        item.CustomData.Add(keys[i], request.CustomData[keys[i]]);
                    }
                }
                _dbInstance.SaveItem(item);
                return new NullResponse();
            });
        }
        public override Task<NullResponse> BeforeNodeActive(NodeActiveRequest request, ServerCallContext context)
        {
            return Task.Run(() =>
            {
                var item = new NodeTracer()
                {
                    NodeID = request.NodeID,
                    Path = request.Path,
                    PreviousEventID = request.PreviousEventID,
                    QueryString = request.QueryString,
                    TimeStamp = request.TimeStamp,
                    EventID = request.EventID,
                    TraceID = request.TraceID,
                    Type = (NodeType)request.NodeType
                };
                if (request.CustomData != null)
                {
                    var keys = request.CustomData.Keys.ToArray();
                    for (int i = 0; i < keys.Length; i++)
                    {
                        item.CustomData.Add(keys[i], request.CustomData[keys[i]]);
                    }
                }
                _dbInstance.SaveItem(item);
                return new NullResponse();
            });
        }
    }
}
