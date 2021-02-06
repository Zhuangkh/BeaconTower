using LuanNiao.JsonConverterExtends;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeaconTower.Client.Abstract
{
    public class LogInfo : IUseLNJsonExtends
    {
        public LogInfo()
        {
        }
        public LogInfo(LogLevel level)
        {
            Level = level;
        }

        public LogInfo SetLevel(LogLevel level)
        {
            Level = level;
            return this;
        }


        public LogInfo SetMessage(string message)
        {
            Message = message;
            return this;
        }
        public LogInfo AddCustomData(string key, string value)
        {
            CustomData.Add(key, value);
            return this;
        }
        private bool _alreadSend = false;
        public LogLevel Level { get; set; } = LogLevel.Trace;
        public long EventID { get; set; }
        public long MethodEventID { get; set; }
        public long TraceID { get; set; }
        public string Message { get; set; }
        public long MethodID { get; set; }
        public string MethodName { get; set; }
        public string FileName { get; set; }
        public int LineNumber { get; set; }
        public long TimeStamp { get; } = DateTime.Now.Ticks;
        public Dictionary<string, string> CustomData { get; } = new Dictionary<string, string>();


        public async void SendAsync()
        {
            if (_alreadSend)
            {
                return;
            }
            _alreadSend = true;
            var serverList = ServerManager.Instance.GetAvailableServer();
            var taskList = new Task[serverList.Count];
            for (int i = 0; i < serverList.Count; i++)
            {
                var index = i;
                taskList[index] = Task.Run(async () =>
                {
                    try
                    {
                        await serverList[index].Log(this);
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
