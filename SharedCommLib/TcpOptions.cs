namespace SharedCommLib
{
    public class TcpOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }

        public TcpOptions()
        { }

        public TcpOptions(string host, int port)
        {
            Host = host;
            Port = port;
        }
    }
}
