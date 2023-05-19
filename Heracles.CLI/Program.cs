using System.CommandLine;

namespace Heracles.CLI
{
    public static class Program
    {
        public static int Main(string[] args) {
            return new RootCommand("Export and import Glory of Heracles (NDS) files") {
                CommandLine.ExportTextCommand(),
                CommandLine.ImportTextCommand(),
            }.Invoke(args);
        }
    }
}