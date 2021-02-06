﻿using BeaconTower.Client.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeaconTower.Warehouse.APIModels
{
    public class LogInfoItemResponse
    {
        public LogInfoItemResponse(LogInfo logInfo)
        {
            if (logInfo==null)
            {
                return;
            }
            this.Level = logInfo.Level;
            this.EventID = logInfo.EventID;
            this.MethodEventID = logInfo.MethodEventID;
            this.TraceID = logInfo.TraceID;
            this.Message = logInfo.Message;
            this.MethodID = logInfo.MethodID;
            this.MethodName = logInfo.MethodName;
            this.FileName = logInfo.FileName;
            this.LineNumber = logInfo.LineNumber;
            this.TimeStamp = logInfo.TimeStamp;
            this.CustomData = logInfo.CustomData;
        }
        public LogLevel Level { get; set; }
        public long EventID { get; set; }
        public long MethodEventID { get; set; }
        public long TraceID { get; set; }
        public string Message { get; set; }
        public long MethodID { get; set; }
        public string MethodName { get; set; }
        public string FileName { get; set; }
        public int LineNumber { get; set; }
        public long TimeStamp { get; } = DateTime.Now.Ticks;
        public DateTime TimeInfo
        {
            get
            {
                return new DateTime(TimeStamp);
            }
        }
        public Dictionary<string, string> CustomData { get; set; }
    }
}
