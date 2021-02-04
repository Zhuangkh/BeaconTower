using BeaconTower.Client.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BeaconTower.Warehouse.AspNetCore.Client.Example.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] _summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        public NodeTracer NodeTracer { get; set; }

        public WeatherForecastController(
            NodeTracer nodeTracer)
        {
            NodeTracer = nodeTracer;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            using var method = NodeTracer.CreateMethodTrace(123);

            method.BeforMethodInvokeAsync();
            Console.WriteLine($"Get:{NodeTracer.TraceID}");
            var rng = new Random();
            Method1();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = _summaries[rng.Next(_summaries.Length)]
            })
            .ToArray();
        }

        private void Method1()
        {
            using var method = NodeTracer.CreateMethodTrace(321);

            method.BeforMethodInvokeAsync();
            Console.WriteLine($"Method1:{NodeTracer.TraceID}");
        }
    }
}
