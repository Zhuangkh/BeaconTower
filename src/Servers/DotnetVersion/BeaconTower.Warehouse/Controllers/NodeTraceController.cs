using BeaconTower.Client.Abstract;
using BeaconTower.TraceDB.NodeTraceDB.Index;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using NodeTraceDBManager = BeaconTower.TraceDB.NodeTraceDB.DBManager;

namespace BeaconTower.Warehouse
{
    [ApiController]
    [Route("api/[controller]")]
    public class NodeTraceController : ControllerBase
    {
        private readonly NodeTraceDBManager _dbInstance;
        public NodeTraceController(NodeTraceDBManager nodeTrceDB)
        {
            _dbInstance = nodeTrceDB;
        }
        [HttpGet("nodes")]
        public List<NodeIDMapSummaryInfo> AllNodeInfo()
        {
            return _dbInstance.AllNodeInfo;
        }

        [HttpGet("nodes/alias({nodeAlias})/items")]
        public List<long> NodeTraceIDList([FromRoute] long nodeAlias)
        {
            var targetNode = _dbInstance.AllNodeInfo.FirstOrDefault(item => item.AliasName == nodeAlias);
            if (targetNode != null)
            {
                return _dbInstance.NodeTraceIDList(targetNode);
            }
            return new List<long>();
        }
        [HttpGet("nodes/items/traceID({traceID})")]
        public List<NodeTracer> TryGetNodeTraceItem([FromRoute] long traceID)
        {
            _dbInstance.TryGetNodeTraceItem(traceID, out var res);
            return res;
        }

        //[HttpGet("path/items")]
        //public List<PathMapSummaryInfo> AllPathInfo()
        //{
        //    return _dbInstance.AllPathInfo;
        //}


    }
}
