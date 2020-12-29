using System.Collections.Generic;
using SliceManager = BeaconTower.Warehouse.TraceDB.Slice.Manager;

namespace BeaconTower.Warehouse.TraceDB.Block.Models
{
    internal class BlockMetadata
    {
        public BlockMetadata(BlockMetadataHead head)
        {
            this.Head = head;
            this.TraceSummaryInfo = new List<BlockTraceItem>();
        }
        public BlockMetadataHead Head { get; init; }
        public List<BlockTraceItem> TraceSummaryInfo { get; init; }
    }
}
