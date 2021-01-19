using System.Threading;

namespace BeaconTower.TraceDB.Slice
{
    /*We can improve the performance here, but currently, we need not do this~*/
    internal partial class Manager
    {

        private void StartChannelHandler()
        {
            Thread thread = new Thread(new ThreadStart(async () =>
            {
                while (true)
                {
                    var item = await _saveItemChannel.Reader.ReadAsync();
                    SaveTraceItemInfo(_metadata.CurrentPosition, item.TraceID, item.Timestamp, item.Data);
                    lock (_sliceHandle)
                    {
                        _sliceHandle.Position = _metadata.CurrentPosition;
                        _sliceHandle.Write(item.Data);
                        _sliceHandle.Flush();
                    }
                    SaveItemMetadataHandler(item.Data);
                }
            }))
            {
                IsBackground = true
            };
            thread.Start();
        }


    }
}
