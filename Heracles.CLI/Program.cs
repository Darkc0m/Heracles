using System.CommandLine;

namespace Heracles.CLI
{
    public static class Program
    {
        public static int Main(string[] args) {
            return new RootCommand("Export and import Glory of Heracles (NDS) files") {
                CommandLine.ExportTextCommand(),
                CommandLine.ImportTextCommand(),
                CommandLine.ExportContainerCommand(),
                CommandLine.ImportContainerCommand(),
            }.Invoke(args);
        }
    }
}