using LuanNiao.Core;
using LuanNiao.JsonConverterExtends;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeaconTower.Client.Abstract
{
    public sealed class NodeTracer : IDisposable, IUseLNJsonExtends
    {


        public NodeTracer()
        {
        }

        private bool _alreadSendAfter = false;
        private readonly Stack<long> _methodStack = new Stack<long>();

        public long TraceID { get; init; }
        public string NodeID { get; init; }
        public NodeType Type { get; init; }
        public long TimeStamp { get; set; }
        public string PreviousNodeID { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string QueryString { get; set; } = string.Empty;
        public Dictionary<string, string> CustomData { get; } = new Dictionary<string, string>();




        public void Dispose()
        {
            if (_alreadSendAfter)
            {
                return;
            }
            throw new NotImplementedException();
        }
        public MethodTracer CreateMethodTrace(string methodName)
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                throw new InvalidOperationException($"Parameter:{nameof(methodName)} was null or empty");
            }
            var thisMethodID = IDGen.GetInstance().NextId();
            var res = new MethodTracer()
            {
                NodeID = NodeID,
                TimeStamp = DateTime.Now.Ticks,
                TraceID = this.TraceID,
                MethodID = thisMethodID,
                PreMethodID = _methodStack.Count == 0 ? 0 : _methodStack.Peek(),
                MethodName = methodName
            };
            _methodStack.Push(thisMethodID);
            return res;
        }

        public async void BeforeNodeActiveAsync()
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
                        await serverList[index].BeforeNodeActiveAsync(this);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Send message to Server: {serverList[index].Alias} failed. Message:{ex.Message}");
                    }
                });
            }

            await Task.WhenAll(taskList);
        }



        public async void AfterNodeActivedAsync()
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
                        await serverList[index].AfterNodeActivedAsync(this);
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