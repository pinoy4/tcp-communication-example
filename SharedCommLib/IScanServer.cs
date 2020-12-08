using System;

namespace SharedCommLib
{
    public interface IScanServer
    {
        event EventHandler<ScanRequestEventArgs> ScanRequested;
        void StartListener();
        void StopListener();
    }
}
