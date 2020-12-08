using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.IO;
using System.Net.Sockets;

namespace SharedCommLib
{
    public class TcpScanServer : IScanServer
    {
        public event EventHandler<ScanRequestEventArgs> ScanRequested;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private TcpListener _listener = null;

        public TcpScanServer(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void StartListener()
        {
            if (_listener != null)
            {
                return;
            }

            using var scope = _serviceScopeFactory.CreateScope();
            _listener = scope.ServiceProvider.GetRequiredService<TcpListener>();

            _listener.Start();
            _listener.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpClientCallback), _listener);
        }

        public void StopListener()
        {
            if (_listener == null)
            {
                return;
            }

            _listener.Stop();
            _listener = null;
        }

        private void DoAcceptTcpClientCallback(IAsyncResult ar)
        {
            // Get the TcpClient.
            var listener = (TcpListener)ar.AsyncState;
            var client = listener.EndAcceptTcpClient(ar);

            try
            {
                // Read the received data.
                var buffer = new byte[256];
                using var ms = new MemoryStream();
                using var stream = client.GetStream();

                var numberOfBytesRead = 0;
                do
                {
                    numberOfBytesRead = stream.Read(buffer, 0, buffer.Length);
                    ms.Write(buffer, 0, numberOfBytesRead);
                }
                while (stream.DataAvailable);
                stream.Close();
                ms.Position = 0;

                // Convert data into ScanRequestEventArgs.
                using var dataReader = new BsonDataReader(ms);
                var serializer = new JsonSerializer();
                var args = serializer.Deserialize<ScanRequestEventArgs>(dataReader);
                ms.Close();

                // Call the event callback.
                ScanRequested(this, args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                // Close client connection.
                client.Close();

                // Restart listener.
                listener.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpClientCallback), listener);
            }
        }
    }
}
