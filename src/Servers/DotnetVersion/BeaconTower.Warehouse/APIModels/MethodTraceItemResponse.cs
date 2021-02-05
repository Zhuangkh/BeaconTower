using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeaconTower.Warehouse.APIModels
{
    public class MethodTraceItemResponse
    {
        public string Key { get; } = Guid.NewGuid().ToString("N");
        public long TraceID { get; set; }
        public string NodeID { get; set; } 
        public long EventID { get; set; }
        public long MethodEventID { get; set; }
        public long PreMethodEventID { get; set; }
        public long MethodID { get; set; }
        public string MethodName { get; set; }
        public string FileName { get; set; }
        public int LineNumber { get; set; }



        public long BeginTimeStamp { get; set; }
        public DateTime BeginTime
        {
            get
            {
                return new DateTime(BeginTimeStamp);
            }
        }
        public long? EndTimeStamp { get; set; }
        public DateTime? EndTime
        {
            get
            {
                return EndTimeStamp == null ? null : new DateTime(EndTimeStamp.Value);
            }
        }

        public string Duration
        {
            get
            {
                if (EndTimeStamp == null)
                {
                    return null;
                }
                return (EndTime.Value - BeginTime).ToString("G");
            }
        }
        public Dictionary<string, string> BeginCustomData { get; set; } = null;
        public Dictionary<string, string> EndCustomData { get; set; } = null;
        public List<MethodTraceItemResponse> Children { get; } = new List<MethodTraceItemResponse>();
    }
}
