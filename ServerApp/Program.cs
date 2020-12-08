using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedCommLib;
using System;
using System.Threading.Tasks;

namespace ServerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Build configuration.
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("tcpOptions.json", false, false);
            var configuration = configurationBuilder.Build();

            // Register dependencies in DI.
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTcpServices(configuration.GetSection("tcpOptions"));

            // Build DI container.
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Run the app.
            var server = serviceProvider.GetRequiredService<IScanServer>();
            server.ScanRequested += ScanRequestReceived;
            server.StartListener();

            Task.Delay(System.Threading.Timeout.Infinite).Wait();
        }

        private static void ScanRequestReceived(object sender, ScanRequestEventArgs args)
        {
            Console.WriteLine($"Received request with name: {args.Name}");
        }
    }
}
