using DocoptNet;
using System.Linq;
using System.Reflection;

namespace Which
{
    public class MainArgs
    {
        private const string usage = @"dotnet-which - The .NET tool finder

Usage:
  dotnet which [options]

Options:
  --full-name, -n                   Show full command name
  --path, -p                        Show command path
  --quiet, -q                       Only prints the command names
  --verbose                         Verbose install and run
  --version, -v                     Show version number
  --help, -h                        Show help
";

        public MainArgs(string[] argv)
        {
            var fixedArgs = new[] { "which" }.Union(argv).ToArray();
            var version = Assembly.GetEntryAssembly().GetName().Version.ToString();
            var args = new Docopt().Apply(usage, fixedArgs, version: version, exit: true);
            Verbose = args["--verbose"].IsTrue;
            FullName = args["--full-name"].IsTrue;
            Path = args["--path"].IsTrue;
            Quiet = args["--quiet"].IsTrue;
        }

        public bool Verbose { get; }
        public bool FullName { get; }
        public bool Path { get; }
        public bool Quiet { get; }
    }
}
