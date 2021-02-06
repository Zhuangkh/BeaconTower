using LuanNiao.JsonConverterExtends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BeaconTower.Client.Abstract
{
    public sealed class MethodTracer : IDisposable, IUseLNJsonExtends
    {
        public MethodTracer()
        {
        }
        public MethodTracer(Action onSendAfterInvoked)
        {
            _onSendAfterInvoked = onSendAfterInvoked;
        }

        private readonly Action _onSendAfterInvoked = null;

        private bool _alreadSendAfter = false;
        public long TraceID { get; set; }
        public string NodeID { get; set; } = string.Empty;
        public long EventID { get; set; }
        public long MethodEventID { get; set; } = LuanNiao.Core.IDGen.GetInstance().NextId();
        public long PreMethodEventID { get; set; }
        public long MethodID { get; set; }
        public long TimeStamp { get; set; }
        public string MethodName { get; set; }
        public string FileName { get; set; }
        public int LineNumber { get; set; }
        public Dictionary<string, string> CustomData { get; } = new Dictionary<string, string>();


        public LogInfo CreateLog(
            LogLevel level = LogLevel.Trace,
            string message = null,
             [CallerMemberName] string methodName = "",
             [CallerFilePath] string sourceFilePath = "",
             [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            var res = new LogInfo()
            {
                EventID = EventID,
                MethodID = MethodID,
                FileName = sourceFilePath == null ? "" : sourceFilePath.Split("\\").Last(),
                Level = level,
                LineNumber = sourceLineNumber,
                MethodEventID = MethodEventID,
                MethodName = methodName,
                TraceID = TraceID,
                Message = message
            };

            return res;
        }


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
            _onSendAfterInvoked?.Invoke();
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
