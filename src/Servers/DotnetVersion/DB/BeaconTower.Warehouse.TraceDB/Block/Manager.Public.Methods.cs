using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaconTower.Warehouse.TraceDB.Block
{
    internal partial class Manager
    {

        public partial BlockInfo CreateBlock()
        {
            BlockInfo info = new();
            info.FileName = LuanNiao.Core.IDGen.GetInstance().NextId();
            info.FromTraceID = 0;
            info.ToTraceID = 0;
            info.ItemsInfo = new List<Data.TraceMetadataItem>();
            return info;
        }

    }
}
