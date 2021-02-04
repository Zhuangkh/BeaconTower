using BeaconTower.Client.Abstract;
using System.Threading.Channels;

namespace BeaconTower.TraceDB.MethodTraceDB.Index
{
    internal partial class Manager
    {
        private readonly string _srouceFolder = null;
        private readonly Channel<MethodTracer> _methodTraceChannel = Channel.CreateUnbounded<MethodTracer>();
    }
}
