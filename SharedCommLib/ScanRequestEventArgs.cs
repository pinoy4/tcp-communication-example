namespace SharedCommLib
{
    public class ScanRequestEventArgs
    {
        public string Name { get; }

        public ScanRequestEventArgs(string name)
        {
            Name = name;
        }
    }
}
