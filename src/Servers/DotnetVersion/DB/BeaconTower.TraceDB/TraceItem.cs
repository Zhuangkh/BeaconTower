namespace BeaconTower.TraceDB
{
    public class TraceItem
    {
        public long TraceID { get; set; }
        public long TimeStamp { get; set; }
        public byte[] Data { get; set; }
    }
}
