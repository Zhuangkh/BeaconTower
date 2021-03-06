using BeaconTower.Client.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace BeaconTower.Client.AspNetCore
{

    internal class NodeTracerMiddleware
    {
        private const string _traceIDHeadKey = "BeaconTower-TraceID";
        private const string _previousEventHeadKey = "BeaconTower-NodeID-Previous-EventID";
        private readonly RequestDelegate _next;
        public NodeTracerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, NodeTracer tracer)
        {
            if (context.Request.Headers.TryGetValue(_traceIDHeadKey,out var traceIDHeadInfo)
                &&
                long.TryParse(traceIDHeadInfo, out var traceID)
                )
            {
                tracer.TraceID = traceID;
            }
            if (context.Request.Headers.TryGetValue(_previousEventHeadKey, out var previousEventIDHeadInfo)
                &&
                long.TryParse(previousEventIDHeadInfo, out var previousEventID)
              )
            {
                tracer.PreviousEventID = previousEventID;
            }
            tracer.TimeStamp = DateTime.Now.Ticks;
            tracer.Path = context.Request.Path.ToString();
            tracer.QueryString = context.Request.QueryString.ToString();
            tracer.BeforeNodeActiveAsync();
            await _next(context);
            tracer.TimeStamp = DateTime.Now.Ticks;
            tracer.AfterNodeActivedAsync();
        }
    }
}
