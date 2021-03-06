using BeaconTower.Client.Abstract;
using BeaconTower.TraceDB;
using BeaconTower.TraceDB.NodeTraceDB.Index;
using BeaconTower.Warehouse.APIModels;
using BeaconTower.Warehouse.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public Response<List<NodeIDMapSummaryInfo>> AllNodeInfo(
            [FromQuery] int? pageSize,
            [FromQuery] int? pageIndex)
        {
            var pSize = pageSize ?? 3;
            var pIndex = pageIndex ?? 0;
            return Success(_dbInstance.AllNodeInfo.Skip(pSize * (pIndex - 1)).Take(pSize).ToList(), _dbInstance.AllNodeInfo.Count);
        }

        [HttpGet("nodes/count")]
        public Response<int> NodeCount()
        {
            return Success(_dbInstance.NodeCount);
        }

        [HttpGet("nodes/alias({nodeAlias})/items")]
        public Response<List<long>> NodeTraceIDList([FromRoute] long nodeAlias)
        {
            var targetNode = _dbInstance.AllNodeInfo.FirstOrDefault(item => item.AliasName == nodeAlias);
            if (targetNode != null)
            {
                return Success(_dbInstance.NodeTraceIDList(targetNode));
            }
            return Success(new List<long>());
        }
        [HttpGet("nodes/alias({nodeAlias})")]
        public Response<NodeIDMapSummaryInfo> GetNodeSummary([FromRoute] long nodeAlias)
        {
            var targetNode = _dbInstance.AllNodeInfo.FirstOrDefault(item => item.AliasName == nodeAlias);
            if (targetNode != null)
            {
                return Success(targetNode);
            }
            return Success<NodeIDMapSummaryInfo>(null);
        }

        [HttpGet("nodes/alias({nodeAlias})/items/path/items")]
        public Response<List<PathMapSummaryInfo>> GetNodeTraceAllPaths(
            [FromRoute] long nodeAlias,
            [FromQuery] int? pageSize,
            [FromQuery] int? pageIndex
            )
        {
            var pSize = pageSize ?? 3;
            var pIndex = pageIndex ?? 0;
            var res = _dbInstance.AllPathInfo.Where(item => item.NodeAliasName == nodeAlias).ToList();
            return Success(res.Skip(pSize * (pIndex - 1)).Take(pSize).ToList(), res.Count);
        }

        [HttpGet("nodes/alias({nodeAlias})/items/path/alias({pathAlias})/count")]
        public Response<int> GetNodePathItemCount(
            [FromRoute] long nodeAlias,
            [FromRoute] long pathAlias
            )
        {
            var res = _dbInstance.GetNodePathItemCount(nodeAlias, pathAlias);
            return Success(res);
        }
        [HttpGet("nodes/alias({nodeAlias})/items/path/alias({pathAlias})/items/traceID")]
        public Response<List<long>> GetTraceIDListByNodeAndPath(
           [FromRoute] long _,
           [FromRoute] long pathAlias,
            [FromQuery] int? pageSize,
            [FromQuery] int? pageIndex
           )
        {
            var pSize = pageSize ?? 3;
            var pIndex = pageIndex ?? 0;
            var res = _dbInstance.GetTraceIDByPath(pathAlias);
            return Success(res.Skip(pSize * (pIndex - 1)).Take(pSize).ToList(), res.Count);
        }


        [HttpGet("nodes/alias({nodeAlias})/items/path/alias({pathAlias})/items/traceID({traceID})/begintime")]
        public Response<DateTime?> GetTraceBeginTime(
           [FromRoute] long traceID
           )
        {
            var timeStamp = _dbInstance.TryGetTraceBeginTime(traceID);
            return Success<DateTime?>(timeStamp == 0 ? null : new DateTime(timeStamp));
        }

        [HttpGet("nodes/alias({nodeAlias})/items/count")]
        public Response<int> NodeTraceItemCount([FromRoute] long nodeAlias)
        {
            var targetNode = _dbInstance.AllNodeInfo.FirstOrDefault(item => item.AliasName == nodeAlias);
            if (targetNode != null)
            {
                return Success(_dbInstance.NodeTraceItemCount(targetNode));
            }
            return Success(0);
        }

        [HttpGet("nodes/items/traceID({traceID})")]
        public Response<NodeTraceItemResponse> TryGetNodeTraceItem([FromRoute] long traceID)
        {
            _dbInstance.TryGetNodeTraceItem(traceID, out var res);


            var result = NodeTraceItemConstructor.ConstructData(res);

            var orderedData = res.OrderBy(item => item.TimeStamp).ToList();


            return Success(result);
        }

        [HttpGet("nodes/items/traceID({traceID})/summary")]
        public Response<List<TraceItemSummary>> TryGetNodeTraceItemSummaryInfo([FromRoute] long traceID)
        {
            _dbInstance.TryGetNodeTraceItemSummary(traceID, out var res);
            return Success(res);
        }




        [HttpGet("alias")]
        public Response<string> GetInstanceAlias()
        {
            return Success(_dbInstance.Alias);
        }
        [HttpGet("unhandled/items/count")]
        public Response<int> GetUnhandledItemCount()
        {
            return Success(_dbInstance.UnhandledItemCount);
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
