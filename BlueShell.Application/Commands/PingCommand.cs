using BlueShell.Core.Models;
using BlueShell.Core.State;
using System.Net.NetworkInformation;

namespace BlueShell.Commands;

public sealed class PingCommand : ShellCommandBase
{
    protected override int MinArgs => 1;

    protected override void ExecuteLogic(ShellContext context, string[] args)
    {
        string url = args[0];

        using Ping ping = new();
        Console.WriteLine($"Send ping to {url}...");

        try
        {
            var reply = ping.Send(url);
            if (reply.Status is IPStatus.Success)
            {
                Console.WriteLine($"Response from: {reply.Address}");
                Console.WriteLine($"Time: {reply.RoundtripTime}ms");
            }
            else
            {
                Console.WriteLine($"Ping was not successfull to {reply.Address}");
            }
        }
        catch (PingException ex)
        {
            Console.WriteLine($"IP address not found [{ex.Message}]");
        }
    }

    protected override void PrintHelp(CommandMetadata metadata)
    {
        Console.WriteLine("Usage: ping <host>");
        base.PrintHelp(metadata);
    }
}
