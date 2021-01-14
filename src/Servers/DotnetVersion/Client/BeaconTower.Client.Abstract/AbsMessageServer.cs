using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaconTower.Client.Abstract
{
    public abstract class AbsMessageServer
    {
        public abstract bool Available { get; }
        public string Alias { get; protected set; }
        public ServerType Type { get; init; } = ServerType.Normal;

        public abstract  Task BeforMethodInvokeAsync(MethodTracer info);

        public abstract Task AfterMethodInvokedAsync(MethodTracer info);

        public abstract Task BeforeNodeActiveAsync(NodeTracer info);
        public abstract Task AfterNodeActivedAsync(NodeTracer info);
 
    }
}