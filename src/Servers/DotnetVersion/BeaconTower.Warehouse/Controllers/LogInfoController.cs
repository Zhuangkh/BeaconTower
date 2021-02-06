using BeaconTower.Warehouse.APIModels;
using BeaconTower.Warehouse.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using LogDBManager = BeaconTower.TraceDB.LogDB.DBManager;

namespace BeaconTower.Warehouse
{
    /// <summary>
    /// improve performace here!!!!!!!!!!!! from here!!!!!!!!
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LogInfoController : MyControllerBase
    {
        private readonly LogDBManager _dbInstance;
        public LogInfoController(LogDBManager methodTrceDB)
        {
            _dbInstance = methodTrceDB;
        }

        [HttpGet("items/traceid({traceID})")]
        public Response<List<LogInfoItemResponse>> GetLogInfoByMethodEventID(
            [FromRoute] long traceID,
            [FromQuery] long? methodEventID)
        {

            _dbInstance.TryGetMethodTraceItem(traceID, out var rawData);
            if (methodEventID != null)
            {
                rawData = rawData.Where(item => item.EventID == methodEventID.Value).ToList();
            }
            rawData = rawData.OrderBy(item => item.TimeStamp).ToList();
            var result = new List<LogInfoItemResponse>();
            rawData.ForEach(item =>
            {
                result.Add(new LogInfoItemResponse(item));
            });
            return Success(result);
        }
    }
}
