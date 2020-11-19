using System.Threading;
using MagicOnion.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Tech.Server
{

    public class Program
    {
        public static CancellationTokenSource Source = new CancellationTokenSource();
        static async Task Main(string[] args)
        {
            using (var taskConsoleAsync = MagicOnionHost.CreateDefaultBuilder(true, LogLevel.Information)
                .UseMagicOnion()
                .RunConsoleAsync(Source.Token))
            {
                await taskConsoleAsync;
            }
        }
    }
}
