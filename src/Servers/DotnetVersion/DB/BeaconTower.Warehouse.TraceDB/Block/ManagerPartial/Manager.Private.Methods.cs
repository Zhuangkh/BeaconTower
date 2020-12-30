using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using BeaconTower.Warehouse.TraceDB.Block.Models;
using static BeaconTower.Warehouse.TraceDB.Block.BlockDefinitions;
using SliceManager = BeaconTower.Warehouse.TraceDB.Slice.Manager;
using static BeaconTower.Warehouse.TraceDB.Slice.SliceDefinitions;

namespace BeaconTower.Warehouse.TraceDB.Block
{
    internal partial class Manager
    {
        /// <summary>
        /// init metadata, call this method when creating block
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// create metadata, call this method when metadata not exists
        /// </summary>
        /// <param name="fi"></param>
        /// <returns></returns>
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

        /// <summary>
        /// load block metadata file, call this method when metadata exists
        /// </summary> 
        /// <returns></returns>
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

        /// <summary>
        /// Init the slice item, call this method when loading metadata
        /// </summary>
        private void InitSliceItem()
        {
            //Get current directory all files
            var allFiles = _blockDirectory.GetFiles();
            foreach (var item in allFiles)
            {
                //find all slice file
                if (!item.Extension.Equals(Slice_File_Extension)
                    || !long.TryParse(item.Name.Replace(Slice_File_Extension, ""), out var fileName))
                {
                    continue;
                }
                //load
                SliceManager sliceItem = new(item.Directory.FullName, fileName);
                sliceItem.LoadOrCreate();
                _sliceLoop.Add((uint)_sliceLoop.Count, sliceItem);
            }
            //if not exists
            if (_sliceLoop.Count == 0)
            {
                //create slice
                CreateSlice();
            }

        }

        /// <summary>
        /// create all slice file, call this method when block creating
        /// </summary>
        private void CreateSlice()
        {
            for (uint i = 0; i < Block_Maximum_Number_Of_Slice_Count; i++)
            {
                var fileName = LuanNiao.Core.IDGen.GetInstance().NextId();
                SliceManager sliceItem = new(_blockDirectory.FullName, fileName);
                sliceItem.LoadOrCreate();
                _sliceLoop.Add(i, sliceItem);
            }
        }



    }
}
