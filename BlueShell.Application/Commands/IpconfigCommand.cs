using BlueShell.Core.Attributes;
using BlueShell.Core.Interfaces;
using BlueShell.Core.Models;
using BlueShell.Core.State;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace BlueShell.Commands
{
    [Command("ipconfig", "Shows your IP configuration", "ipconfig")]
    public sealed class IpconfigCommand : IShellCommand, IDetailedHelpProvider
    {
        public int MinArgs => 0;

        public void Execute(ShellContext context, string[] args)
        {
            Console.WriteLine("Ips are loading....\n");
            Task.Run(IpConfig).Wait();
        }

        private async Task IpConfig()
        {
            var ipv4 = GetIPv4();
            var ipv6 = GetIPv6();
            var publicIP = await GetPublicIp();

            Console.WriteLine($"IPv4:. . . :{ipv4}");
            Console.WriteLine($"IPv6:. . . :{ipv6}");
            Console.WriteLine($"PublicIP:. :{publicIP}");
        }

        private static async Task<string?> GetPublicIp()
        {
            using HttpClient client = new();
            return await client.GetStringAsync("https://api.ipify.org");
        }

        private static IPAddress? GetIPv4()
        {
            return Dns.GetHostEntry(Dns.GetHostName())
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip));
        }

        private static IPAddress? GetIPv6()
        {
            return Dns.GetHostEntry(Dns.GetHostName())
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetworkV6 && !IPAddress.IsLoopback(ip));
        }

        public CommandHelp GetDetailedHelp() => new(
            "ipconfig",
            "Shows the local IPv4 address, IPv6 address, and public IP address.",
            Notes: ["The public IP address is retrieved from an external service."]);
    }
}
