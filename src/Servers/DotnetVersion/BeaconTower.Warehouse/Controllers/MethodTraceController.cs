using BeaconTower.Warehouse.Controllers;
using Microsoft.AspNetCore.Mvc;
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
        public void GetMethodTraceInfoByTraceID([FromRoute] long traceID)
        {
            
            _dbInstance.TryGetMethodTraceItem(traceID, out var res);

        }
    }
}
