namespace BeaconTower.TraceDB.Block
{
    internal static class BlockDefinitions
    {
        /// <summary>
        /// this assembly's block version
        /// </summary>
        public const uint Block_CurrentLib_Version = 0x0000_1000;

        /// <summary>
        /// Block can save the item's  maximum
        /// </summary>
        public const int Block_TraceItem_Maximum = 1024 << 6;
        public const int Block_Maximum_Number_Of_Slice_Count = 8;
        public const int Block_Slice_TraceItem_Maximum = Block_TraceItem_Maximum / Block_Maximum_Number_Of_Slice_Count;

        /// <summary>
        /// Metadata's head size
        /// </summary>
        public const int Metadata_Head_Size = 1024;

        /// <summary>
        /// Metadata file's name
        /// </summary>
        public const string Metadata_File_Name = "metadata.btd";



    }
}
