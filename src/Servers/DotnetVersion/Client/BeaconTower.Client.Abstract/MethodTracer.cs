using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeaconTower.Client.Abstract
{
    public sealed class MethodTracer : IDisposable
    {

        internal MethodTracer()
        {
        }


        private bool _alreadSendAfter = false;
        public long TraceID { get; init; }
        public string NodeID { get; init; } = string.Empty;
        public long EventID { get; set; }
        public long MethodEventID { get; set; } = LuanNiao.Core.IDGen.GetInstance().NextId();
        public long PreMethodEventID { get; init; }
        public long MethodID { get; init; }
        public long TimeStamp { get; internal set; }
        public string MethodName { get; init; }
        public Dictionary<string, string> CustomData { get; } = new Dictionary<string, string>();



        public void Dispose()
        {
            if (_alreadSendAfter)
            {
                return;
            }
            TimeStamp = DateTime.Now.Ticks;
            this.AfterMethodInvokedAsync();
        }
        public async void BeforMethodInvokeAsync()
        {
            var serverList = ServerManager.Instance.GetAvailableServer();
            var taskList = new Task[serverList.Count];
            for (int i = 0; i < serverList.Count; i++)
            {
                var index = i;
                taskList[index] = Task.Run(async () =>
                {
                    try
                    {
                        await serverList[index].BeforMethodInvokeAsync(this);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Send message to Server: {serverList[index].Alias} failed. Message:{ex.Message}");
                    }
                });
            }

            await Task.WhenAll(taskList);
        }



        public async void AfterMethodInvokedAsync()
        {
            if (_alreadSendAfter)
            {
                return;
            }
            _alreadSendAfter = true;
            var serverList = ServerManager.Instance.GetAvailableServer();
            var taskList = new Task[serverList.Count];
            for (int i = 0; i < serverList.Count; i++)
            {
                var index = i;
                taskList[index] = Task.Run(async () =>
                {
                    try
                    {
                        await serverList[index].AfterMethodInvokedAsync(this);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Send message to Server: {serverList[index].Alias} failed. Message:{ex.Message}");
                    }
                });
            }

            await Task.WhenAll(taskList);
        }

    }
}
