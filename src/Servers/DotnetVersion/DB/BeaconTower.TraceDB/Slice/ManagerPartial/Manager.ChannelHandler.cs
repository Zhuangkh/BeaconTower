using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BeaconTower.TraceDB.Slice.Models;
using static BeaconTower.TraceDB.Slice.MetadataDefinitions;
using static BeaconTower.TraceDB.Slice.SliceItemDefinitions;

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
