using BlueShell.Core.Attributes;
using BlueShell.Core.Interfaces;
using BlueShell.Core.Models;
using BlueShell.Core.State;
using System.Net.NetworkInformation;

namespace BlueShell.Commands;

 [Command("ping", "Sends a ping request to a hostname or IP address.", "ping <host>")]
 public sealed class PingCommand : IShellCommand, IDetailedHelpProvider
{
    public int MinArgs => 1;

    public void Execute(ShellContext context, string[] args)
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

    public CommandHelp GetDetailedHelp() => new(
        "ping <host>",
        "Sends one ICMP ping request to a hostname or IP address.",
        Examples: ["ping example.com", "ping 127.0.0.1"],
        Notes: ["The target must be reachable and allow ICMP requests."]);
}
