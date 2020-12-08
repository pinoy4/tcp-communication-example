using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedCommLib;
using ClientApp.Commands;

namespace ClientApp
{
    class Program
    {
        static int Main(string[] args)
        {
            // Build configuration.
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("tcpOptions.json", false, false);
            var configuration = configurationBuilder.Build();

            // Register dependencies in DI.
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTcpServices(configuration.GetSection("tcpOptions"));
            serviceCollection.AddTransient<ConsoleApp, ConsoleApp>();
            serviceCollection.AddTransient<StartScanCommand, StartScanCommand>();

            // Build DI container.
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Run the app.
            var app = serviceProvider.GetService<ConsoleApp>();
            return app.Execute(args);
        }
    }
}
