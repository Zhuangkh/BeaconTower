using BeaconTower.Client.Abstract;
using System;
using System.Text;
using System.Threading.Tasks;

namespace BeaconTower.Client.Console
{
    public sealed class ConsoleServer : AbsMessageServer
    {
        public override bool Available => true;

        public ConsoleServer()
        {
            Alias = nameof(ConsoleServer);
        }

        private StringBuilder GetInfo(MethodTracer info, StringBuilder builder = null)
        {
            if (builder == null)
            {
                builder = new StringBuilder();
            }
            else
            {
                builder.Append("\r\n");
            }
            builder.AppendLine($"TraceID:{info.TraceID} ");
            builder.AppendLine($"MethodID:{info.MethodID} ");
            builder.AppendLine($"MethodName:{info.MethodName} ");
            builder.AppendLine($"EventID:{info.EventID} ");
            builder.AppendLine($"MethodEventID:{info.MethodEventID} ");
            builder.AppendLine($"PreMethodEventID:{info.PreMethodEventID} ");
            builder.AppendLine($"FileName:{info.FileName} LineNumber:{info.LineNumber} ");
            builder.AppendLine($"NodeID:{info.NodeID} ");
            builder.AppendLine($"TimeStamp:{info.TimeStamp} ");
            builder.AppendLine($"Time:{new DateTime(info.TimeStamp):yyyy-MM-dd HH:mm:ss:fff} ");
            builder.AppendLine($"CustomData:{System.Text.Json.JsonSerializer.Serialize(info.CustomData)} ");
            builder.AppendLine();
            return builder;
        }

        private StringBuilder GetInfo(NodeTracer info, StringBuilder builder = null)
        {
            if (builder == null)
            {
                builder = new StringBuilder();
            }
            else
            {
                builder.Append("\r\n");
            }
            builder.AppendLine($"TraceID:{info.TraceID} ");
            builder.AppendLine($"Path:{info.Path} ");
            builder.AppendLine($"QueryString:{info.QueryString} ");
            builder.AppendLine($"Type:{(Enum.GetName(typeof(NodeType), info.Type))} ");
            builder.AppendLine($"NodeID:{info.NodeID} ");
            builder.AppendLine($"EventID:{info.EventID} ");
            builder.AppendLine($"PreviousEventID:{info.PreviousEventID} ");
            builder.AppendLine($"TimeStamp:{info.TimeStamp} ");
            builder.AppendLine($"Time:{new DateTime(info.TimeStamp):yyyy-MM-dd HH:mm:ss:fff} ");
            builder.AppendLine($"CustomData:{System.Text.Json.JsonSerializer.Serialize(info.CustomData)} ");
            builder.AppendLine();
            return builder;
        }
        private StringBuilder GetInfo(LogInfo info, StringBuilder builder = null)
        {
            if (builder == null)
            {
                builder = new StringBuilder();
            }
            else
            {
                builder.Append("\r\n");
            }
            builder.AppendLine($"TraceID:{info.TraceID} ");
            builder.AppendLine($"EventID:{info.EventID} ");
            builder.AppendLine($"MethodID:{info.MethodID} ");
            builder.AppendLine($"MethodEventID:{info.MethodEventID} ");
            builder.AppendLine($"MethodName:{info.MethodName} ");
            builder.AppendLine($"FileName:{info.FileName} ");
            builder.AppendLine($"LineNumber:{info.LineNumber} ");
            builder.AppendLine($"Level:{Enum.GetName(typeof(LogLevel), info.Level)} ");
            builder.AppendLine($"Message:{info.Message} "); 
            builder.AppendLine($"Time:{new DateTime(info.TimeStamp):yyyy-MM-dd HH:mm:ss:fff} ");
            builder.AppendLine($"CustomData:{System.Text.Json.JsonSerializer.Serialize(info.CustomData)} ");
            builder.AppendLine();
            return builder;
        }

        public override async Task AfterMethodInvokedAsync(MethodTracer info)
        {
            if (info == null)
            {
                return;
            }
            await Task.Run(() => { System.Console.WriteLine(GetInfo(info, new StringBuilder($"{nameof(AfterMethodInvokedAsync)}:"))); });
        }

        public override async Task AfterNodeActivedAsync(NodeTracer info)
        {
            if (info == null)
            {
                return;
            }
            await Task.Run(() => { System.Console.WriteLine(GetInfo(info, new StringBuilder($"{nameof(AfterNodeActivedAsync)}:"))); });
        }

        public override async Task BeforeNodeActiveAsync(NodeTracer info)
        {
            if (info == null)
            {
                return;
            }
            await Task.Run(() => { System.Console.WriteLine(GetInfo(info, new StringBuilder($"{nameof(BeforeNodeActiveAsync)}:"))); });
        }

        public override async Task BeforMethodInvokeAsync(MethodTracer info)
        {
            if (info == null)
            {
                return;
            }
            await Task.Run(() => { System.Console.WriteLine(GetInfo(info, new StringBuilder($"{nameof(BeforMethodInvokeAsync)}:"))); });
        }

        public override async Task Log(LogInfo info)
        {
            if (info == null)
            {
                return;
            }
            await Task.Run(() => { System.Console.WriteLine(GetInfo(info, new StringBuilder($"{nameof(Log)}:"))); });
        }
    }
}
