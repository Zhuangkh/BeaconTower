﻿using BeaconTower.Client.Abstract;
using BeaconTower.Client.ClassLibrary;
using BeaconTower.Client.Console;
using BeaconTower.Client.Warehouse.Grpc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BeaconTower.Warehouse.ClassLibrary.Client.Example
{
    public class Program
    {

        public static void Main(string[] _)
        {

            var instance = TracerClient.Instance.Init("./appsettings.json");

            Task.Run(() =>
            {
                Task.Delay(1000).Wait();
                //while (true)
                //{
                var level = 0;
                SendTrace(null, "Main", "", level,0);
                //}
            });
            Console.ReadLine();
            Console.WriteLine("Hello World!");

        }

        public static void SendTrace(long? traceID, string path, string previousNodeID, int level,int childNum)
        {
            if (level > 3)
            {
                return;
            }
            var tracer1 = TracerClient.Instance.CreateNodeTracer(traceID, previousNodeID);
            tracer1.NodeID = $"Node{level}:{childNum}:{previousNodeID}";
            tracer1.TimeStamp = DateTime.Now.Ticks;
            tracer1.Path = path;
            tracer1.QueryString = string.Empty;
            tracer1.BeforeNodeActiveAsync();
            var childCount = new Random().Next(1,10); 
            for (int i = 0; i < childCount; i++)
            {
                SendTrace(tracer1.TraceID, $"{tracer1.Path}-{level}:child{i}", tracer1.NodeID, level + 1,i);
            }
            Task.Delay(new Random().Next(1, 100)).Wait();
            tracer1.TimeStamp = DateTime.Now.Ticks;
            tracer1.AfterNodeActivedAsync();
        }
    }
}
