using McMaster.Extensions.CommandLineUtils;
using SharedCommLib;
using System;

namespace ClientApp.Commands
{
    internal class StartScanCommand : ICommandConfigurer
    {
        private readonly IScanClient _scanClient;

        public StartScanCommand(IScanClient scanClient)
        {
            _scanClient = scanClient;
        }

        public CommandLineApplication Configure(CommandLineApplication command)
        {
            command.Description = "Tell the server application to start scanning.";
            var nameOption = command.Option(
                "-n|--name <NAME>",
                "A name to identify the requested scan in the results. Defaults to the current date-time.",
                CommandOptionType.SingleValue
            );
            command.HelpOption("-h|--help");

            command.OnExecuteAsync(async (cancellationToken) =>
            {
                var name = nameOption.Value() ?? DateTimeOffset.UtcNow.ToString();
                await _scanClient.RequestScanAsync(new ScanRequestEventArgs(name));
                return 0;
            });

            return command;
        }
    }
}