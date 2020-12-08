using McMaster.Extensions.CommandLineUtils;

namespace ClientApp.Commands
{
    public interface ICommandConfigurer
    {
        CommandLineApplication Configure(CommandLineApplication command);
    }
}
