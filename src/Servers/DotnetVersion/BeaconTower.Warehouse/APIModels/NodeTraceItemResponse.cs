using BeaconTower.Client.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeaconTower.Warehouse.APIModels
{
    public class NodeTraceItemResponse
    {
        public NodeTraceItemResponse()
        {
        }

        public string Key { get; } = Guid.NewGuid().ToString("N");
        public long TraceID { get; set; }
        public string NodeID { get; set; }
        public NodeType Type { get; init; }
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

        public int? UseMS
        {
            get
            {
                if (EndTimeStamp == null)
                {
                    return null;
                }
                return (EndTime.Value - BeginTime).Milliseconds;
            }
        }
        public string PreviousNodeID { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string QueryString { get; set; } = string.Empty;
        public Dictionary<string, string> BeginCustomData { get; set; } = null;
        public Dictionary<string, string> EndCustomData { get; set; } = null;
        public List<NodeTraceItemResponse> NextNode { get; } = new List<NodeTraceItemResponse>();
    }
}
