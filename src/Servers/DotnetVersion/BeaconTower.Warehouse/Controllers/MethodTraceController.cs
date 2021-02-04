using BeaconTower.Client.Abstract;
using BeaconTower.TraceDB; 
using BeaconTower.Warehouse.APIModels;
using BeaconTower.Warehouse.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public void GetMethodTraceInfoByTraceID([FromRoute]long traceID)
        {
            _dbInstance.TryGetMethodTraceItem(traceID, out var res);

        }
    }
}
