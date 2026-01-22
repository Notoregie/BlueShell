using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace BlueShell.Commands
{
    public sealed class IpconfigCommand : ShellCommandBase
    {
        protected override void ExecuteLogic(ref string? dir, params string[]? args)
        {
            Console.WriteLine("Ips are loading...." + Environment.NewLine);
            Task.Run(() => IpConfig()).Wait();
        }

        async Task IpConfig()
        {
            var ipv4 = GetIPv4();
            var ipv6 = GetIPv6();
            var publicIP = await GetPublicIp();

            Console.WriteLine($"IPv4:. . . :{ipv4}");
            Console.WriteLine($"IPv6:. . . :{ipv6}");
            Console.WriteLine($"PublicIP:. :{publicIP}");
        }

        async Task<string?> GetPublicIp()
        {
            HttpClient client = new HttpClient();
            return await client.GetStringAsync("https://api.ipify.org");
        }

        IPAddress? GetIPv4()
        {
            return Dns.GetHostEntry(Dns.GetHostName())
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip));
        }
        IPAddress? GetIPv6()
        {
            return Dns.GetHostEntry(Dns.GetHostName())
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetworkV6 && !IPAddress.IsLoopback(ip));
        }
    }
}
