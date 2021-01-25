using BeaconTower.Client.Abstract;
using BeaconTower.TraceDB;
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
    public class SystemController : MyControllerBase
    {
        private readonly DataBase _dbInstance;
        public SystemController(DataBase nodeTrceDB)
        {
            _dbInstance = nodeTrceDB;
        }
        [HttpGet("instance/items/alias")]
        public Response<List<string>> AllNodeInfo()
        {
            return Success(_dbInstance.AliasList);
        }


        [HttpGet("instance/items/alias({alias})/slice/items/count")]
        public Response<int> GetInstanceAliasSliceItemCount(string alias)
        {
            return Success(_dbInstance[alias].SliceCount);
        }

        [HttpGet("instance/items/alias({alias})/block/items/count")]
        public Response<int> GetInstanceAliasBlockItemCount(string alias)
        {
            return Success(_dbInstance[alias].BlockCount);
        }

        [HttpGet("instance/items/alias({alias})/trace/items/count")]
        public Response<int> GetInstanceAliasTraceItemCount(string alias)
        {
            return Success(_dbInstance[alias].TraceItemCount);
        }

        [HttpGet("instance/items/alias({alias})/folder/path")]
        public Response<string> GetInstanceAliasFolderPath(string alias)
        {
            return Success(_dbInstance[alias].FolderPath);
        }

        [HttpGet("instance/items/alias({alias})/folder/name")]
        public Response<string> GetInstanceAliasFolderName(string alias)
        {
            return Success(_dbInstance[alias].FolderName);
        }


    }
}
