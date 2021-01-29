namespace BeaconTower.Client.Abstract
{
    public enum NodeType
    {
        WebServer = 0,
        MqConsumer = 1,
        Gateway = 2,
        AuthCentral = 3,
        ConsoleApp = 4,
        ClientApp = 5,
        Unset = 0xff,
    }
}
