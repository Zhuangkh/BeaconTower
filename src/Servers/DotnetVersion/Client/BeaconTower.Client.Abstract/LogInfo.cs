using System;
using System.Collections.Generic;

namespace BeaconTower.Client.Abstract
{
    public class LogInfo
    {
        public LogInfo(LogLevel level = LogLevel.Trace)
        {
            Level = level;
        }
        public LogLevel Level { get; private set; }
        public long EventID { get; set; }
        public long MethodEventID { get; set; }
        public long TraceID { get; set; } = LuanNiao.Core.IDGen.GetInstance().NextId();
        public string Message { get; set; }
        public long TimeStamp { get; } = DateTime.Now.Ticks;
        public string MethodInfo { get; set; }
        public Dictionary<string, string> CustomData { get; } = new Dictionary<string, string>();
    }
}
