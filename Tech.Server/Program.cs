using System;
using System.Threading.Tasks;
using MagicOnion.Hosting;
using MagicOnion.Server;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Tech.Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //you can define address and port 
            await MagicOnionHost.CreateDefaultBuilder(true, LogLevel.Information)
                .UseMagicOnion()
                .RunConsoleAsync();

        }
    }
}
