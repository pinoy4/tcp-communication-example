using McMaster.Extensions.CommandLineUtils;
using ClientApp.Commands;

namespace ClientApp
{
    internal class ConsoleApp : CommandLineApplication
    {
        new public string Name { get; } = "vautomation-app";
        new public string Description { get; } = "A cli application to test sending data to another application.";

        private readonly StartScanCommand _startScanCommand;

        public ConsoleApp(StartScanCommand startScanCommand)
        {
            _startScanCommand = startScanCommand;

            HelpOption("-h|--help");

            Command("start-scan", cmd => _startScanCommand.Configure(cmd));

            OnExecute(() =>
            {
                ShowHelp();
                return 1;
            });
        }
    }
}
