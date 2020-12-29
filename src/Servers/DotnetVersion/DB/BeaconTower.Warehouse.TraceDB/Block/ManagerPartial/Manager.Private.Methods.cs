using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using BeaconTower.Warehouse.TraceDB.Block.Models;
using static BeaconTower.Warehouse.TraceDB.Block.BlockDefinitions;
using SliceManager = BeaconTower.Warehouse.TraceDB.Slice.Manager;

namespace BeaconTower.Warehouse.TraceDB.Block
{
    internal partial class Manager
    {

        private BlockMetadata InitMetadata()
        {
            BlockMetadata res;
            var fi = new FileInfo(Path.Combine(_blockDirectory.FullName, Metadata_File_Name));
            if (!fi.Exists)
            {
                res = CreateMetadata(fi);
            }
            else
            {
                res = LoadMetadataFile(fi);
            }
            return res;
        }

        private BlockMetadata CreateMetadata(FileInfo fi)
        {
            try
            {
                BlockMetadataHead head = new();
                head.CurrentItemsCount = 0;
                head.FromTraceID = 0;
                head.ToTraceID = 0;
                head.Version = Block_CurrentLib_Version;

                using var headBufferRenter = MemoryPool<byte>.Shared.Rent(Metadata_File_Size);
                var buffer = headBufferRenter.Memory.Slice(0, Metadata_File_Size).Span;
                buffer.Fill(0x00);
                var headBuffer = LuanNiao.Core.StructUtilTools.StructUtilTools.ToData(in head);
                LuanNiao.Core.NetTools.CRC16IBM.SetCRC16(headBuffer, 0, headBuffer.Length, 0);
                headBuffer.CopyTo(buffer);
                using var fs = fi.Create();
                fs.Write(buffer);
                fs.Close();

                return new(head);
            }
            catch //Todo: (Exception) handle detail info next version.
            {
                return null;
            }
        }

        private BlockMetadata LoadMetadataFile(FileInfo fi)
        {
            try
            {
                using var fs = fi.OpenRead();
                using var headBufferRenter = MemoryPool<byte>.Shared.Rent(Metadata_Head_Size);
                var headBuffer = headBufferRenter.Memory.Slice(0, Metadata_Head_Size).Span;
                fs.Read(headBuffer);
                var head = LuanNiao.Core.StructUtilTools.StructUtilTools.ToStruct<BlockMetadataHead>(headBuffer.Slice(0, Marshal.SizeOf<BlockMetadataHead>()));
                BlockMetadata res = new(head);
                //Todo: Needs to load payload content
                return res;
            }
            catch
            {
                return null;
            }
        }

        private void InitSliceItem()
        {

            var allFiles = _blockDirectory.GetFiles();
            foreach (var item in allFiles)
            {
                if (!item.Extension.Equals(BlockItem_File_Extension)
                    || !long.TryParse(item.Name.Replace(BlockItem_File_Extension, ""), out var fileName))
                {
                    continue;
                }
                SliceManager sliceItem = new(item.FullName, fileName);
                sliceItem.LoadOrCreate();
                _allSlice.Add(sliceItem);
            }
            if (_allSlice.Count == 0)
            {
                CreateSlice();
            }

        }

        private void CreateSlice()
        {
            var fileName = LuanNiao.Core.IDGen.GetInstance().NextId();
            SliceManager sliceItem = new(Path.Combine(_blockDirectory.FullName, $"{fileName}{BlockItem_File_Extension}"), fileName);
            sliceItem.LoadOrCreate();
            _allSlice.Add(sliceItem);
        }

        private void MoveNextAvailableSlice()
        {
            lock (this)
            {
                _currentSlice = _allSlice.FirstOrDefault(item => item.Available());
                if (_currentSlice==null)
                {
                    CreateSlice();
                }
                _currentSlice = _allSlice.FirstOrDefault(item => item.Available());
            }
        }


        
    }
}
