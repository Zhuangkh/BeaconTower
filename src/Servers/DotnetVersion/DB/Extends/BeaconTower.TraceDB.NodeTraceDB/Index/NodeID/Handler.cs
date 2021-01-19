using LuanNiao.Core.StructUtilTools;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BeaconTower.TraceDB.NodeTraceDB.Index.NodeID
{
    internal class Handler
    {
        private readonly FileStream _indexFileHandler = null;
        public Handler(FileInfo fileInfo)
        {
            _indexFileHandler = fileInfo.OpenRead();
        }

        public Task LoadAsync()
        {
            return Task.Run(() =>
            {
                var summaryInfoSize = Marshal.SizeOf<ItemSummaryInfo>();
                var summaryInfoData = new byte[summaryInfoSize];
                _indexFileHandler.Position = 0;
                _indexFileHandler.Read(summaryInfoData);
                var summaryInfo = StructUtilTools.ToStruct<ItemSummaryInfo>(summaryInfoData);


            });
        }

    }
}
