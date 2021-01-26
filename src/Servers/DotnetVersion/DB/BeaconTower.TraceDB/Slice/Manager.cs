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
        /// the item's count in queue
        /// </summary>
        internal int UnhandledItemCount => _saveItemChannel.Reader.Count;

        /// <summary>
        /// Get the trace item list from this slice
        /// </summary>
        /// <param name="traceID"></param>
        /// <returns></returns>
        internal partial List<TraceItem> GetTraceItems(long traceID);

    }
}
