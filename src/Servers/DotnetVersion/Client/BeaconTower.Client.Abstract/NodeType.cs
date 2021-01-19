namespace BeaconTower.Client.Abstract
{
    public enum NodeType
    {
        WebServer = 0,
        Mqconsumer = 1,
        Gateway = 2,
        AuthCentral = 3,
        Unset = 0xff,
    }
}
