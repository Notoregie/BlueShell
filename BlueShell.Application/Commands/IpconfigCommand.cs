using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using BlueShell.Core.State;

namespace BlueShell.Commands
{
    public sealed class IpconfigCommand : ShellCommandBase
    {
        protected override void ExecuteLogic(ShellContext context, string[] args)
        {
            Console.WriteLine("Ips are loading...." + Environment.NewLine);
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
    }
}
