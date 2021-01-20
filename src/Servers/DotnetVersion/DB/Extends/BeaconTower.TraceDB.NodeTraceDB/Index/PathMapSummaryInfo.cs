using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaconTower.TraceDB.NodeTraceDB.Index
{
    public class PathMapSummaryInfo
    {
        public long AliasName { get; set; }
        public int OrignalPathLength { get; set; }
        public string OrignalPath { get; set; }
    }
}
