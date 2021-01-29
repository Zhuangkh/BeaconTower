using BeaconTower.Client.Abstract;
using BeaconTower.Client.ClassLibrary;
using BeaconTower.Client.Console;
using BeaconTower.Client.Warehouse.Grpc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace BeaconTower.Warehouse.ClassLibrary.Client.Example
{
    public class Program
    {
        public static void Main(string[] _)
        {

            var instance = TracerClient.Instance.Init("./appsettings.json");

            
            Console.WriteLine("Hello World!");
        }
    }
}
