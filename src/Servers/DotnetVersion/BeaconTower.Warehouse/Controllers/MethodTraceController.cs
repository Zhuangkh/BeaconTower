using BeaconTower.Warehouse.APIModels;
using BeaconTower.Warehouse.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using MethodTraceDBManager = BeaconTower.TraceDB.MethodTraceDB.DBManager;

namespace BeaconTower.Warehouse
{
    [ApiController]
    [Route("api/[controller]")]
    public class MethodTraceController : MyControllerBase
    {
        private readonly MethodTraceDBManager _dbInstance;
        public MethodTraceController(MethodTraceDBManager methodTrceDB)
        {
            _dbInstance = methodTrceDB;
        }
 
        [HttpGet("items/traceid({traceID})")]
        public Response<List<MethodTraceItemResponse>> GetMethodTraceInfoByTraceID(
            [FromRoute] long traceID,
            [FromQuery] long? eventID)
        {

            _dbInstance.TryGetMethodTraceItem(traceID, out var res);
            if (eventID != null)
            {
                res = res.Where(item => item.EventID == eventID.Value).ToList();
            }

            var result = MethodTraceItemConstructor.ConstructData(res);
            return Success(result);
        }
    }
}
