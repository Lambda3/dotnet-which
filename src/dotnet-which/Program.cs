using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Which
{
    class Program
    {
        private static ConsoleColor defaultConsoleColor;

        static int Main(string[] args)
        {
            Initialize();
            var arguments = new MainArgs(args);
            var quiet = !arguments.Verbose && arguments.Quiet;
            var executables = FindExecutables(arguments.Verbose);
            if (!executables.Any())
            {
                WriteLine("No executables found for dotnet.");
                return 0;
            }
            if (!quiet)
                WriteLine("Found the following tools:");
            foreach (var executable in executables)
            {
                Write(executable.cmd.Substring("dotnet-".Length));
                if (arguments.Path)
                    Write($" ({executable.path})");
                else if (arguments.FullName)
                    Write($" ({executable.cmd})");
                WriteLine();
            }
            return 0;
        }

        private static void Initialize() => defaultConsoleColor = Console.ForegroundColor;

        private static IList<(string path, string cmd)> FindExecutables(bool verbose)
        {
            string binary, arguments;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                binary = "where.exe";
                arguments = "dotnet-*.exe dotnet-*.cmd";
            }
            else
            {
                binary = "/usr/bin/env";
                arguments = "sh -c \"/usr/bin/env find $PATH -name 'dotnet-*' -type f '(' -perm -u+x -o -perm -g+x -o -perm -o+x ')'\"";
            }
            if (verbose)
                WriteLine($"Running: {binary} {arguments}");
            var startInfo = new ProcessStartInfo(binary, arguments)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                UseShellExecute = false
            };
            startInfo.Environment.Add("IFS", ":");
            var process = new Process() { StartInfo = startInfo };
            var sb = new StringBuilder();
            process.OutputDataReceived += (sender, e) => sb.AppendLine(e.Data);
            process.ErrorDataReceived += (sender, e) => WriteErrorLine(e.Data);
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            var commandOutput = sb.ToString();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return ParseWindowsWhere(commandOutput);
            else
                return ParsePosixFind(commandOutput);
        }

        private static string[] filterExtensionsForLinux = {".json", ".dll", ".nuspec", ".pdb"};
        private static IList<(string path, string cmd)> ParsePosixFind(string text) =>
            (from line in text.Split('\n')
             let trimmedLine = line.Trim()
             where !string.IsNullOrEmpty(trimmedLine)
             let lastSeparator = trimmedLine.LastIndexOf('/')
             let cmd = trimmedLine.Substring(lastSeparator + 1)
             where !filterExtensionsForLinux.Any(ext => cmd.EndsWith(ext))
             orderby cmd
             select (trimmedLine, cmd)).ToList();

        private static IList<(string path, string cmd)> ParseWindowsWhere(string text) =>
            (from line in text.Split('\n')
             let trimmedLine = line.Trim()
             where !string.IsNullOrEmpty(trimmedLine)
             let lastSeparator = trimmedLine.LastIndexOf('\\')
             let cmd = trimmedLine.Substring(lastSeparator + 1, trimmedLine.Length - (lastSeparator + 1) - ".exe".Length)
             orderby cmd
             select (trimmedLine, cmd)).ToList();


        private static readonly object sync = new object();

        private static void WriteLine(string message = "")
        {
            if (message == null)
                return;
            lock (sync)
                Console.WriteLine(message);
        }

        private static void Write(string message = "")
        {
            if (message == null)
                return;
            lock (sync)
                Console.Write(message);
        }

        private static void WriteErrorLine(string message)
        {
            if (message == null)
                return;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && message.StartsWith("INFO:"))
                return;
            lock (sync)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Error.WriteLine(message);
                Console.ForegroundColor = defaultConsoleColor;
            }
        }
    }
}
