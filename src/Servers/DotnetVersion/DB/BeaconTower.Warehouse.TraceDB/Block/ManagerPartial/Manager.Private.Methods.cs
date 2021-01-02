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
        private void InitMetadata()
        {
            _metadataFileHandle = new FileInfo(Path.Combine(_blockDirectory.FullName, Metadata_File_Name)).Open(FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if (_metadataFileHandle.Length == 0)
            {
                CreateMetadata();
            }
            else
            {
                LoadMetadataFile();
            }
        }

        /// <summary>
        /// create metadata, call this method when metadata not exists
        /// </summary>
        /// <param name="fi"></param>
        /// <returns></returns>
        private void CreateMetadata()
        {

            _metadata.CurrentItemsCount = 0;
            _metadata.FromTraceID = 0;
            _metadata.ToTraceID = 0;
            _metadata.Version = Block_CurrentLib_Version;

            SaveMetadata();
        }

        /// <summary>
        /// load block metadata file, call this method when metadata exists
        /// </summary> 
        /// <returns></returns>
        private void LoadMetadataFile()
        {
            var buffer = new byte[Marshal.SizeOf<BlockMetadata>()];
            _metadataFileHandle.Read(buffer);
            _metadata = LuanNiao.Core.StructUtilTools.StructUtilTools.ToStruct<BlockMetadata>(buffer);
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
                SliceManager sliceItem = new(_blockDirectory.FullName, i);
                sliceItem.LoadOrCreate();
                _sliceLoop.Add(i, sliceItem);
            }
        }

        private void SaveMetadata()
        {  
            var headBuffer = LuanNiao.Core.StructUtilTools.StructUtilTools.ToData(in _metadata);            
            LuanNiao.Core.NetTools.CRC16IBM.SetCRC16(headBuffer, 0, headBuffer.Length, 0); 
            _metadataFileHandle.Position = 0; 
            _metadataFileHandle.Write(headBuffer);
            _metadataFileHandle.Flush();
        }

    }
}
