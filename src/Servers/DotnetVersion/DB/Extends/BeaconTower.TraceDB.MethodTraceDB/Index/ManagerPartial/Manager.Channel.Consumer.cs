using BeaconTower.Client.Abstract;
using System.Threading;

namespace BeaconTower.TraceDB.MethodTraceDB.Index
{
    internal partial class Manager
    {
        private void StartConsumer()
        {
            Thread t = new Thread(new ThreadStart(async () =>
            {
                while (true)
                {
                    var item = await _methodTraceChannel.Reader.ReadAsync();
                    SaveMethodInfo(item);
                }
            }))
            {
                IsBackground = true
            };
            t.Start();
        }

        private void SaveMethodInfo(MethodTracer item)
        {
            //问题是TraceID完全一样啊~~~~~
            //那应该就是webapi的事情,他可以去node里头查找,也以来methodDB来查找
            //目前应该还没有需要通信MethodTracer的地方,临时统计即可.
            //根据eventID查询
            //根据methodeventID 查询
            //根据PreMethodEventID查询
        }
    }
}
