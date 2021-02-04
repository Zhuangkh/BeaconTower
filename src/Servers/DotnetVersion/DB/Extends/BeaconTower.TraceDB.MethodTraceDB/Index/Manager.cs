namespace BeaconTower.TraceDB.MethodTraceDB.Index
{
    internal partial class Manager
    {
        public Manager(string sourceFolder)
        {
            _srouceFolder = sourceFolder;
        }
        public async void StartServer()
        {
            //await InitAllFileInfo();
            StartConsumer();
        }
    }
}
/*
 
 这里其实存储的应该是
MethodID对应的MethodEventID有多少
然后每一个MethodEventID对应下面的MethodEventID有多少,都已经到了实例级别的索引了
traceID的话应该是一致的
但是也不排除说,我要用MethodID去查询出这个函数对应的所有的TraceID出来
然后反找对吧...
算了先这样吧ヾ(。￣□￣)ﾂ゜゜゜ 不实现
 */