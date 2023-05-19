using System.CommandLine;

namespace Heracles.CLI
{
    public static class Program
    {
        public static int Main(string[] args) {
            return new RootCommand("Export and import Glory of Heracles (NDS) files") {
                new Command("export-t", "Export text from different format files into .po files")
                {
                    new Command("arm", "Export text from an arm9.bin file into multiple .po files")
                },
                new Command("import-t", "Import text from .po files into their original file format")
                {
                    new Command("arm", "Import text from .po files into an arm9.file (All .po files must be present!!!)")
                }
            }.Invoke(args);
        }
    }
}