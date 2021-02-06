using BeaconTower.Client.Abstract;
using BeaconTower.Protocol;
using Grpc.Core;
using System.Linq;
using System.Threading.Tasks;
using LogDBManager = BeaconTower.TraceDB.LogDB.DBManager;

namespace BeaconTower.Warehouse.Services
{
    public class LogService : Log.LogBase
    {
        private readonly LogDBManager _dbInstance;
        public LogService(LogDBManager methodTrceDB)
        {
            _dbInstance = methodTrceDB;
        }
        public override Task<NullResponse> Trace(LogRequest request, ServerCallContext context)
        {
            return SaveItem(request, LogLevel.Trace);
        }
        public override Task<NullResponse> Info(LogRequest request, ServerCallContext context)
        {
            return SaveItem(request, LogLevel.Info);
        }
        public override Task<NullResponse> Debug(LogRequest request, ServerCallContext context)
        {
            return SaveItem(request, LogLevel.Debug);
        }
        public override Task<NullResponse> Warning(LogRequest request, ServerCallContext context)
        {
            return SaveItem(request, LogLevel.Warning);
        }
        public override Task<NullResponse> Error(LogRequest request, ServerCallContext context)
        {
            return SaveItem(request, LogLevel.Error);
        }
        public override Task<NullResponse> Panic(LogRequest request, ServerCallContext context)
        {
            return SaveItem(request, LogLevel.Panic);
        }



        private Task<NullResponse> SaveItem(LogRequest request, LogLevel level)
        {
            return Task.Run(() =>
            {
                var item = new LogInfo()
                {
                    Level = level,
                    FileName = request.FileName,
                    LineNumber = request.LineNumber,
                    EventID = request.EventID,
                    MethodEventID = request.MethodEventID,
                    MethodID = request.TimeStamp,
                    TraceID = request.TraceID,
                    MethodName = request.MethodName,
                    Message = request.Message,
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
