namespace BeaconTower.TraceDB.Slice
{
    internal static class SliceItemDefinitions
    {

        public const ushort SliceItem_TraceID_Size = sizeof(long);
        public const ushort SliceItem_TimeStamp_Size = sizeof(long);
        public const ushort SliceItem_Length_Size = sizeof(int);
        public const ushort SliceItem_Head_Size = SliceItem_TraceID_Size + SliceItem_TimeStamp_Size + SliceItem_Length_Size;
    }
}
