using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SharedCommLib
{
    public class TcpScanClient : IScanClient
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly TcpOptions _tcpOptions;

        public TcpScanClient(IServiceScopeFactory serviceScopeFactory, TcpOptions tcpOptions)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _tcpOptions = tcpOptions;
        }

        public async Task RequestScanAsync(ScanRequestEventArgs args)
        {
            // Convert args to byte[].
            using var ms = new MemoryStream();
            using var datawriter = new BsonDataWriter(ms);
            var serializer = new JsonSerializer();
            serializer.Serialize(datawriter, args);
            var data = ms.ToArray();

            // Get new instance of TcpClient.
            using var scope = _serviceScopeFactory.CreateScope();
            var tcpClient = scope.ServiceProvider.GetRequiredService<TcpClient>();

            // Connect TcpClient and send data.
            await tcpClient.ConnectAsync(_tcpOptions.Host, _tcpOptions.Port);
            using var stream = tcpClient.GetStream();
            await stream.WriteAsync(data, 0, data.Length);

            // Cleanup.
            stream.Close();
            tcpClient.Close();
        }
    }
}
