using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Sockets;

namespace SharedCommLib
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddTcpServices(
            this IServiceCollection services,
            IConfigurationSection configuration
        )
        {
            // Register options.
            services.Configure<TcpOptions>(configuration);
            services.AddSingleton(provider => provider.GetRequiredService<IOptions<TcpOptions>>().Value);

            // Register client.
            services.AddTransient<TcpClient, TcpClient>();
            services.AddScoped<IScanClient, TcpScanClient>();

            // Register listener.
            services.AddScoped(provider =>
            {
                var tcpOptions = provider.GetRequiredService<TcpOptions>();
                return new TcpListener(
                    Dns.GetHostEntry(tcpOptions.Host).AddressList[0],
                    tcpOptions.Port
                );
            });
            services.AddScoped<IScanServer, TcpScanServer>();

            return services;
        }
    }
}
