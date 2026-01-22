using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace BlueShell.Commands
{
    public class PingCommand : ShellCommandBase
    {
        protected override int MinArgs => 1;
        protected override void ExecuteLogic(ref string? dir, params string[]? args)
        {
            string? url = args[0].ToString();

            Ping ping = new Ping();
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
    }
}
