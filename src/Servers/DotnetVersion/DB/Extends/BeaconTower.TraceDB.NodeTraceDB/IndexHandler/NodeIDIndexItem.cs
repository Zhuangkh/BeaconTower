using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BeaconTower.TraceDB.NodeTraceDB.IndexHandler
{
    /// <summary>
    /// 存储每一个NodeID不同的trace信息
    /// 存放在NodeIndex文件夹下
    /// 比如
    /// A节点 X X X X
    /// B节点 Y Y Y Y
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NodeIDIndexItem
    {
        public int NodeIDLength { get; set; }
        public long TraceCount { get; set; }
    }
}
