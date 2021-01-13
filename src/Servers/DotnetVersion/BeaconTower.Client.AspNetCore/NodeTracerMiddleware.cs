using BeaconTower.Protocol;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace BeaconTower.Client.AspNetCore
{
    internal class NodeTracerMiddleware
    {
        private readonly RequestDelegate _next;
        public NodeTracerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, NodeTracer tracer)
        {
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
