using System.Threading.Tasks;

namespace SharedCommLib
{
    public interface IScanClient
    {
        Task RequestScanAsync(ScanRequestEventArgs args);
    }
}
