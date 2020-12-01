using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SerilogMapToFileConsoleApp1
{
    class Program
    {
        static CancellationTokenSource source = null;
        static List<Task> tasks = null;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello SerilogMapToFileConsoleApp1!");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Async(
                     a => a.Map(e => e, (v, wt) => wt.File($@"logs\{v.Timestamp:yyyyMM}\app-{v.Timestamp:yyyyMMdd}-{v.Level.ToString().ToLower()}.txt"), sinkMapCountLimit: 1)
                    )
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            source = new CancellationTokenSource();
            tasks = new List<Task>();

            for (int i = 0; i < 1; i++)
            {
                tasks.Add(Task.Factory.StartNew(Logging, source.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default));
            }

            bool flag = false;

            do
            {
                string cmd = Console.ReadLine();
                flag = cmd == "q";
            } while (!flag);

            Console.WriteLine("quite");
            source.Cancel();

            Log.CloseAndFlush();

            Console.ReadKey();
        }

        private static void Logging()
        {
            while (!source.IsCancellationRequested)
            {
                Log.Debug("Hello World DebugDebugDebugDebugDebugDebugDebugDebugDebugDebugDebugDebugDebugDebugDebugDebugDebugDebugDebugDebugDebugDebugDebugDebug!");
                Log.Information("Hello World InformationInformationInformationInformationInformationInformationInformationInformationInformationInformationInformation!");
                Log.Warning("Hello World WarningWarningWarningWarningWarningWarningWarningWarningWarningWarningWarningWarningWarningWarningWarningWarningWarning!");
                Log.Error("Hello World ErrorErrorErrorErrorErrorErrorErrorErrorErrorErrorErrorErrorErrorErrorErrorErrorErrorErrorErrorErrorErrorErrorErrorError!");
                Log.Fatal("Hello World FatalFatalFatalFatalFatalFatalFatalFatalFatalFatalFatalFatalFatalFatalFatalFatalFatalFatalFatalFatalFatalFatalFatalFatal!");
            }
        }
    }
}
