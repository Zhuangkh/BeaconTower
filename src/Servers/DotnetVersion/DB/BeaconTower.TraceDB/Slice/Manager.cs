using BeaconTower.TraceDB.Slice.Models;
using System.Collections.Generic;

namespace BeaconTower.TraceDB.Slice
{
    internal partial class Manager
    {
        /// <summary>
        /// manager's construct method, use to build the slice manager
        /// </summary>
        /// <param name="fileFullPath">this slice's full path</param>
        /// <param name="fileName">slice's file name, use to open the target file stream</param>
        public Manager(string fileFullPath, long fileName)
        {
            _fileFullPath = fileFullPath;
            _fileName = fileName;
        }

        /// <summary>
        /// load or create the slice
        /// </summary>
        internal partial void LoadOrCreate();
        /// <summary>
        /// close this slice 
        /// </summary>
        internal partial void Close();
        /// <summary>
        /// save the trace item to db
        /// </summary>
        /// <returns></returns>
        internal partial bool SaveItem(long traceID, long timeStamp, byte[] data);

        /// <summary>
        /// slice name, slice name is file name
        /// </summary>
        internal long FileName => _fileName;

        /// <summary>
        /// Get the trace item list from this slice
        /// </summary>
        /// <param name="traceID"></param>
        /// <returns></returns>
        internal List<TraceItem> GetTraceItems(long traceID)
        {
            List<TraceItemMetadata> targetIndex = new();
            lock (_traceItemsInfo)
            {
                for (int i = 0; i < _traceItemsInfo.Count; i++)
                {
                    if (_traceItemsInfo[i].TraceID == traceID)
                    {
                        targetIndex.Add(_traceItemsInfo[i]);
                    }
                }
            }
            var res = new List<TraceItem>();
            if (targetIndex.Count == 0)
            {
                return res;
            }
            foreach (var item in targetIndex)
            {
                TraceItem temp = new()
                {
                    TraceID = item.TraceID,
                    TimeStamp = item.TimeStamp,
                    Data = new byte[item.Length]
                };
                lock (_sliceHandle)
                {
                    _sliceHandle.Position = item.Position;
                    _sliceHandle.Read(temp.Data);
                }
                res.Add(temp);
            }
            return res;
        }

    }
}
