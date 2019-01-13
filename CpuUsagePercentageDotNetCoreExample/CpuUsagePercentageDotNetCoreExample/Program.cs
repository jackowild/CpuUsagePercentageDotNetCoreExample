using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CpuUsagePercentageDotNetCoreExample
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var task = Task.Run(() => ConsumeCPU(50));

            while (true)
            {
                await Task.Delay(2000);
                var cpuUsage = await GetCpuUsageForProcess();

                Console.WriteLine(cpuUsage);
            }
        }

        public static void ConsumeCPU(int percentage)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (true)
            {
                if (watch.ElapsedMilliseconds > percentage)
                {
                    Thread.Sleep(100 - percentage);
                    watch.Reset();
                    watch.Start();
                }
            }
        }

        private static async Task<double> GetCpuUsageForProcess()
        {
            var startTime = DateTime.UtcNow;
            var startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;

            await Task.Delay(500);

            var endTime = DateTime.UtcNow;
            var endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;

            var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds;

            var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);

            return cpuUsageTotal * 100;
        }
    }
}
