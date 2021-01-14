using BeaconTower.Protocol;
using System.Collections.Generic;

namespace BeaconTower.Client
{
    public class BeaconTowerOptions
    {
        public NodeTypeEnum NodeType { get; set; }
        public string NodeID { get; set; }
        public List<string> HostList { get; } = new List<string>();
    }
}
