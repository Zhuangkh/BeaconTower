using BeaconTower.Client.Abstract;
using BeaconTower.Protocol;
using Grpc.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using MethodTraceDBManager = BeaconTower.TraceDB.MethodTraceDB.DBManager;

namespace BeaconTower.Warehouse.Services
{
    public class MethodTraceService : MethodTrace.MethodTraceBase
    {
        private readonly MethodTraceDBManager _dbInstance;
        public MethodTraceService(MethodTraceDBManager methodTrceDB)
        {
            _dbInstance = methodTrceDB;
        }
        public override Task<NullResponse> AfterMethodInvoked(MethodActiveReuqest request, ServerCallContext context)
        {
            return Task.Run(() =>
            {
                var item = new MethodTracer()
                {
                    NodeID = request.NodeID,
                    FileName = request.FileName,
                    LineNumber = request.LineNumber,
                    EventID = request.EventID,
                    MethodEventID = request.MethodEventID,
                    MethodID = request.TimeStamp,
                    TraceID = request.TraceID,
                    MethodName = request.MethodName,
                    PreMethodEventID = request.PreMethodEventID,
                    TimeStamp = request.TimeStamp
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
        public override Task<NullResponse> BeforMethodInvoke(MethodActiveReuqest request, ServerCallContext context)
        {
            return Task.Run(() =>
            {
                var item = new MethodTracer()
                {
                    NodeID = request.NodeID,
                    FileName = request.FileName,
                    LineNumber = request.LineNumber,
                    EventID = request.EventID,
                    MethodEventID = request.MethodEventID,
                    MethodID = request.TimeStamp,
                    TraceID = request.TraceID,
                    MethodName = request.MethodName,
                    PreMethodEventID = request.PreMethodEventID,
                    TimeStamp = request.TimeStamp
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
