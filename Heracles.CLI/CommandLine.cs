using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heracles.CLI
{
    public static class CommandLine
    {
        public static Command ExportTextCommand() {
            var exportScedic = new Command("scedic", "Export text from a SCEDIC.bin file into a .po file")
            {
                new Option<string>("--srcPath", "Path of the SCEDIC.bin file", ArgumentArity.ExactlyOne),
                new Option<string>("--dirPath", "The directory where the .po file will be saved", ArgumentArity.ExactlyOne),
            };
            exportScedic.Handler = CommandHandler.Create<string, string>(TextCommands.exportScedic);

            var exportUIMess = new Command("uimess", "Export text from a UIMess.bin file into a .po file (do not confuse with UIMess.arc)")
            {
                new Option<string>("--srcPath", "Path of the UIMess.bin file", ArgumentArity.ExactlyOne),
                new Option<string>("--dirPath", "The directory where the .po file will be saved", ArgumentArity.ExactlyOne),
            };
            exportUIMess.Handler = CommandHandler.Create<string, string>(TextCommands.exportUIMess);

            var exportDataDic = new Command("dic", "Export text from a DataDictionary.bin file into a .po file")
            {
                new Option<string>("--srcPath", "Path of the DataDictionary.eng file", ArgumentArity.ExactlyOne),
                new Option<string>("--dirPath", "The directory where the .po file will be saved", ArgumentArity.ExactlyOne),
            };
            exportDataDic.Handler = CommandHandler.Create<string, string>(TextCommands.exportDataDictionary);

            var exportItem = new Command("item", "Export text from an itemdata.bin file into a .po file")
            {
                new Option<string>("--srcPath", "Path of the itemdata.bin file", ArgumentArity.ExactlyOne),
                new Option<string>("--dirPath", "The directory where the .po file will be saved", ArgumentArity.ExactlyOne),
            };
            exportItem.Handler = CommandHandler.Create<string, string>(TextCommands.exportItemdata);

            var exportMons = new Command("mons", "Export text from a monsdata_e.bin file into a .po file")
            {
                new Option<string>("--srcPath", "Path of the monsdata_e.bin file", ArgumentArity.ExactlyOne),
                new Option<string>("--dirPath", "The directory where the .po file will be saved", ArgumentArity.ExactlyOne),
            };
            exportMons.Handler = CommandHandler.Create<string, string>(TextCommands.exportMonsdata);

            var exportTutorial = new Command("tut", "Export text from a tutorial.bin file into a .po file (extracted from a .arc file)")
            {
                new Option<string>("--srcPath", "Path of the tutorial.bin file", ArgumentArity.ExactlyOne),
                new Option<string>("--dirPath", "The directory where the .po file will be saved", ArgumentArity.ExactlyOne),
            };
            exportTutorial.Handler = CommandHandler.Create<string, string>(TextCommands.exportTutorial);

            var exportArm = new Command("arm", "Export text from an arm9.bin file into .po files")
            {
                new Option<string>("--srcPath", "Path of the itemdata.bin file", ArgumentArity.ExactlyOne),
                new Option<string>("--dirPath", "The directory where the .po files will be saved", ArgumentArity.ExactlyOne),
            };
            exportArm.Handler = CommandHandler.Create<string, string>(TextCommands.exportArm);

            return new Command("export-t", "Export text from different format files into .po files")
            {
                exportScedic,
                exportUIMess,
                exportDataDic,
                exportItem,
                exportMons,
                exportTutorial,
                exportArm,
            };
        }

        public static Command ImportTextCommand() {
            var importScedic = new Command("scedic", "Export text from a SCEDIC.bin file into a .po file")
            {
                new Option<string>("--srcPath", "Path of the SCEDIC.bin file", ArgumentArity.ExactlyOne),
                new Option<string>("--dirPath", "The directory where the .po file will be saved", ArgumentArity.ExactlyOne),
            };
            importScedic.Handler = CommandHandler.Create<string, string>(TextCommands.importScedic);

            var importUIMess = new Command("uimess", "Export text from a UIMess.bin file into a .po file (do not confuse with UIMess.arc)")
            {
                new Option<string>("--srcPath", "Path of the UIMess.bin file", ArgumentArity.ExactlyOne),
                new Option<string>("--dirPath", "The directory where the .po file will be saved", ArgumentArity.ExactlyOne),
            };
            importUIMess.Handler = CommandHandler.Create<string, string>(TextCommands.importUIMess);

            var importDataDic = new Command("dic", "Export text from a DataDictionary.bin file into a .po file")
            {
                new Option<string>("--srcPath", "Path of the DataDictionary.eng file", ArgumentArity.ExactlyOne),
                new Option<string>("--dirPath", "The directory where the .po file will be saved", ArgumentArity.ExactlyOne),
            };
            importDataDic.Handler = CommandHandler.Create<string, string>(TextCommands.importDataDictionary);

            var importItem = new Command("item", "Export text from an itemdata.bin file into a .po file")
            {
                new Option<string>("--srcPath", "Path of the itemdata.bin file", ArgumentArity.ExactlyOne),
                new Option<string>("--dirPath", "The directory where the .po file will be saved", ArgumentArity.ExactlyOne),
            };
            importItem.Handler = CommandHandler.Create<string, string>(TextCommands.importItemdata);

            var importMons = new Command("mons", "Export text from a monsdata_e.bin file into a .po file")
            {
                new Option<string>("--srcPath", "Path of the monsdata_e.bin file", ArgumentArity.ExactlyOne),
                new Option<string>("--dirPath", "The directory where the .po file will be saved", ArgumentArity.ExactlyOne),
            };
            importMons.Handler = CommandHandler.Create<string, string>(TextCommands.importMonsdata);

            var importTutorial = new Command("tut", "Export text from a tutorial.bin file into a .po file (extracted from a .arc file)")
            {
                new Option<string>("--srcPath", "Path of the tutorial.bin file", ArgumentArity.ExactlyOne),
                new Option<string>("--dirPath", "The directory where the .po file will be saved", ArgumentArity.ExactlyOne),
            };
            importTutorial.Handler = CommandHandler.Create<string, string>(TextCommands.importTutorial);

            var importArm = new Command("arm", "Export text from an arm9.bin file into .po files")
            {
                new Option<string>("--srcPath", "Path of the itemdata.bin file", ArgumentArity.ExactlyOne),
                new Option<string>("--dirPath", "The directory where the .po files will be saved", ArgumentArity.ExactlyOne),
            };
            importArm.Handler = CommandHandler.Create<string, string>(TextCommands.importArm);


            return new Command("import-t", "Import text from .po files into their original file format")
            {
                importScedic,
                importUIMess,
                importDataDic,
                importItem,
                importMons,
                importTutorial,
                importArm,
            };
        }

        public static Command ExportContainerCommand() {
            var exportArc3 = new Command("arc3", "Export files from a version 3 .arc file")
            {
                new Option<string>("--srcPath", "Path of the .arc file", ArgumentArity.ExactlyOne),
                new Option<string>("--dirPath", "Directory where the extracted files will be saved", ArgumentArity.ExactlyOne),
            };
            exportArc3.Handler = CommandHandler.Create<string, string>(ContainerCommands.exportArc3);

            return new Command("export-c", "Export files from a container")
            {
                exportArc3,
            };
        }

        public static Command ImportContainerCommand() {
            var importArc3 = new Command("arc3", "Import files from a folder into a version 3 .arc file")
            {
                new Option<string>("--srcPath", "Directory with the files", ArgumentArity.ExactlyOne),
                new Option<string>("--dirPath", "Directory where the .arc file will be saved", ArgumentArity.ExactlyOne),
            };
            importArc3.Handler = CommandHandler.Create<string, string>(ContainerCommands.importArc3);

            return new Command("import-c", "Import files into a container")
            {
                importArc3,
            };
        }
    }
}
