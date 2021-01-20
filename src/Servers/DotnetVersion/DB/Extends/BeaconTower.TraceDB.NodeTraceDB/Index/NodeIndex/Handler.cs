using LuanNiao.Core.StructUtilTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BeaconTower.TraceDB.NodeTraceDB.Index.NodeIndex
{
    internal class Handler
    {
        private readonly FileStream _indexFileHandler = null;
        private long _traceCount = 0;
        private readonly byte[] _buffer = new byte[sizeof(long)];
        private readonly List<long> _allTraceIDInfo = new List<long>();
        public Handler(FileInfo fileInfo)
        {
            _indexFileHandler = fileInfo.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }


        public List<long> TraceIDList => _allTraceIDInfo;
        public Task LoadAsync()
        {
            return Task.Run(() =>
            {
                _indexFileHandler.Position = 0;
                _indexFileHandler.Read(_buffer);
                _traceCount = BitConverter.ToInt64(_buffer);
                for (int i = 0; i < _traceCount; i++)
                {
                    _indexFileHandler.Read(_buffer);
                    _allTraceIDInfo.Add(BitConverter.ToInt64(_buffer));
                } 
            });
        }

        public Handler SaveNewItem(long traceID)
        {
            lock (this)
            {
                _indexFileHandler.Position = 0;
                _indexFileHandler.Write(BitConverter.GetBytes(++_traceCount));
                _indexFileHandler.Position = _indexFileHandler.Length;
                _indexFileHandler.Write(BitConverter.GetBytes(traceID));
                _allTraceIDInfo.Add(traceID);
                _indexFileHandler.Flush();
            }
            return this;
        }

    }
}
