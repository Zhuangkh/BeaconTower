using BeaconTower.Client.Abstract;
using BeaconTower.Protocol;
using Grpc.Core;
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
                    PreviousNodeID = request.PreviousNodeID,
                    QueryString = request.QueryString,
                    TimeStamp = request.TimeStamp,
                    TraceID = request.TraceID,
                    Type = (NodeType)request.NodeType
                };
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
                    PreviousNodeID = request.PreviousNodeID,
                    QueryString = request.QueryString,
                    TimeStamp = request.TimeStamp,
                    TraceID = request.TraceID,
                    Type = (NodeType)request.NodeType
                };
                _dbInstance.SaveItem(item);
                return new NullResponse();
            });
        }
    }
}
