using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Launcher
{
    public static class DebugProgram
    {
        private const string ExeName = "Shadowverse.exe";
        private static bool _isLoaded;

        public static void Run()
        {
            Console.WriteLine("Olivia - Dev Mode");
            Console.WriteLine("Watching for Shadowverse...");
            ManualResetEventSlim resetEventSlim = new ManualResetEventSlim();

            Console.CancelKeyPress += (sender, args) =>
            {
                resetEventSlim.Set();
            };
            while (!resetEventSlim.IsSet)
            {
                if (Console.KeyAvailable)
                {
                    var keyRead = Console.ReadKey();
                    var files = Directory.EnumerateFiles(Directory.GetCurrentDirectory()).ToArray();
                    var oliviaDll = "Olivia.dll";
                    if (keyRead.Key == ConsoleKey.L)
                    {
                        Load(oliviaDll);
                    }
                    if(keyRead.Key == ConsoleKey.U)
                    {
                        Unload(oliviaDll);
                    }
                }
                Thread.Sleep(1);
            }
        }

        private static void Load(string file)
        {
            var process = Process.Start("mono-injector\\mono-injector.exe",
                $"-dll {file} -target {ExeName} -namespace Olivia -class Loader -method Load");
            if (process != null && process.WaitForExit(50000))
            {
                var exitCode = process.ExitCode;
                if (exitCode == 0)
                {
                    Console.WriteLine("Loaded.");
                    _isLoaded = true;
                }
                else
                {
                    Console.WriteLine("Failed to Load.");
                }
            }
        }

        private static void Unload(string file)
        {
            var process = Process.Start("mono-injector\\mono-injector.exe",
                $"-dll {file} -target {ExeName} -namespace Olivia -class Loader -method Unload");
            if (process != null && process.WaitForExit(50000))
            {
                var exitCode = process.ExitCode;
                if (exitCode == 0)
                {
                    Console.WriteLine("Unloaded.");
                    _isLoaded = false;
                }
                else
                {
                    Console.WriteLine("Failed to Unload.");
                }
            }
        }
    }
}