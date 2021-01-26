using BeaconTower.Client.Abstract;
using BeaconTower.TraceDB.NodeTraceDB.Index;
using BeaconTower.Warehouse.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using NodeTraceDBManager = BeaconTower.TraceDB.NodeTraceDB.DBManager;

namespace BeaconTower.Warehouse
{
    [ApiController]
    [Route("api/[controller]")]
    public class NodeTraceController : MyControllerBase
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


        [HttpGet("alias")]
        public Response<string> GetInstanceAlias()
        {
            return Success(_dbInstance.Alias);
        }

        [HttpGet("state")]
        public Response<bool> GetInstanceState()
        {
            return Success(_dbInstance.State);
        }

        [HttpGet("slice/items/count")]
        public Response<int> GetSliceItemCount()
        {
            return Success(_dbInstance.SliceCount);
        }

        [HttpGet("block/items/count")]
        public Response<int> GetBlockItemCount()
        {
            return Success(_dbInstance.BlockCount);
        }

        [HttpGet("trace/items/count")]
        public Response<int> GetTraceItemCount()
        {
            return Success(_dbInstance.TraceItemCount);
        }

        [HttpGet("folder/path")]
        public Response<string> GetFolderPath()
        {
            return Success(_dbInstance.FolderPath);
        }

        [HttpGet("folder/name")]
        public Response<string> GetFolderName()
        {
            return Success(_dbInstance.FolderName);
        }

    }
}
