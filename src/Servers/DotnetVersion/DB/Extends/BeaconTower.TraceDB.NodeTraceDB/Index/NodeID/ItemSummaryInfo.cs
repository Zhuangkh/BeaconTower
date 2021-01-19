using System.Runtime.InteropServices;

namespace BeaconTower.TraceDB.NodeTraceDB.Index.NodeID
{
    /// <summary>
    /// 存储每一个NodeID不同的trace信息
    /// 存放在NodeIndex文件夹下
    /// 比如
    /// A节点 X X X X
    /// B节点 Y Y Y Y
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ItemSummaryInfo
    {
        public int NodeIDLength { get; set; }
        public long TraceCount { get; set; }
    }
}
