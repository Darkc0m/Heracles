using Heracles.Lib.Converters;
using Heracles.Lib.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileSystem;
using Yarhl.IO;
using Yarhl.Media.Text;

namespace Heracles.CLI
{
    public static class TextCommands
    {
        public static void exportScedic(string srcPath, string dirPath) {
            Node n = NodeFactory.FromFile(srcPath);

            n.TransformWith<Binary2Scedic>()
                .TransformWith<Scedic2Po>()
                .TransformWith<Po2Binary>()
                .Stream.WriteTo($"{dirPath}/{Path.GetFileNameWithoutExtension(n.Name)}.po");
        }

        public static void importScedic(string srcPath, string dirPath) {
            Node n = NodeFactory.FromFile(srcPath);

            n.TransformWith<Binary2Po>()
                .TransformWith<Scedic2Po>()
                .TransformWith<Binary2Scedic>()
                .Stream.WriteTo($"{dirPath}/{Path.GetFileNameWithoutExtension(n.Name)}.bin");
        }

        public static void exportUIMess(string srcPath, string dirPath) {
            Node n = NodeFactory.FromFile(srcPath);

            n.TransformWith<Binary2UIMess>()
                .TransformWith<UIMess2Po>()
                .TransformWith<Po2Binary>()
                .Stream.WriteTo($"{dirPath}/{Path.GetFileNameWithoutExtension(n.Name)}.po");
        }

        public static void importUIMess(string srcPath, string dirPath) {
            Node n = NodeFactory.FromFile(srcPath);

            n.TransformWith<Binary2Po>()
                .TransformWith<UIMess2Po>()
                .TransformWith<Binary2UIMess>()
                .Stream.WriteTo($"{dirPath}/{Path.GetFileNameWithoutExtension(n.Name)}.bin");
        }

        public static void exportDataDictionary(string srcPath, string dirPath) {
            Node n = NodeFactory.FromFile(srcPath);

            n.TransformWith<Binary2DataDictionary>()
                .TransformWith<DataDictionary2Po>()
                .TransformWith<Po2Binary>()
                .Stream.WriteTo($"{dirPath}/{Path.GetFileNameWithoutExtension(n.Name)}.po");
        }

        public static void importDataDictionary(string srcPath, string dirPath) {
            Node n = NodeFactory.FromFile(srcPath);

            n.TransformWith<Binary2Po>()
                .TransformWith<DataDictionary2Po>()
                .TransformWith<Binary2DataDictionary>()
                .Stream.WriteTo($"{dirPath}/{Path.GetFileNameWithoutExtension(n.Name)}.bin");
        }

        public static void exportItemdata(string srcPath, string dirPath) {
            Node n = NodeFactory.FromFile(srcPath);

            n.TransformWith<Binary2Itemdata>()
                .TransformWith<Itemdata2Po>()
                .TransformWith<Po2Binary>()
                .Stream.WriteTo($"{dirPath}/{Path.GetFileNameWithoutExtension(n.Name)}.po");
        }

        public static void importItemdata(string srcPath, string dirPath) {
            Node n = NodeFactory.FromFile(srcPath);

            n.TransformWith<Binary2Po>()
                .TransformWith<Itemdata2Po>()
                .TransformWith<Binary2Itemdata>()
                .Stream.WriteTo($"{dirPath}/{Path.GetFileNameWithoutExtension(n.Name)}.bin");
        }

        public static void exportMonsdata(string srcPath, string dirPath) {
            Node n = NodeFactory.FromFile(srcPath);

            n.TransformWith<Binary2Monsdata>()
                .TransformWith<Monsdata2Po>()
                .TransformWith<Po2Binary>()
                .Stream.WriteTo($"{dirPath}/{Path.GetFileNameWithoutExtension(n.Name)}.po");
        }

        public static void importMonsdata(string srcPath, string dirPath) {
            Node n = NodeFactory.FromFile(srcPath);

            n.TransformWith<Binary2Po>()
                .TransformWith<Monsdata2Po>()
                .TransformWith<Binary2Monsdata>()
                .Stream.WriteTo($"{dirPath}/{Path.GetFileNameWithoutExtension(n.Name)}.bin");
        }

        public static void exportTutorial(string srcPath, string dirPath) {
            Node n = NodeFactory.FromFile(srcPath);

            n.TransformWith<Binary2Scedic>()
                .TransformWith<Scedic2Po>()
                .TransformWith<Po2Binary>()
                .Stream.WriteTo($"{dirPath}/{Path.GetFileNameWithoutExtension(n.Name)}.po");
        }

        public static void importTutorial(string srcPath, string dirPath) {
            Node n = NodeFactory.FromFile(srcPath);

            n.TransformWith<Binary2Po>()
                .TransformWith<Tutorial2Po>()
                .TransformWith<Binary2Tutorial>()
                .Stream.WriteTo($"{dirPath}/{Path.GetFileNameWithoutExtension(n.Name)}.bin");
        }

        public static void exportArm(string srcPath, string dirPath) {
            Node n = NodeFactory.FromFile(srcPath);

            var arm = n.TransformWith<Binary2Arm>().GetFormatAs<Arm>();
            var po2bin = new Po2Binary();

            po2bin.Convert(arm.status).Stream.WriteTo($"{dirPath}/status.po");
            po2bin.Convert(arm.outskirtsLoc).Stream.WriteTo($"{dirPath}/outskirts.po");
            po2bin.Convert(arm.locations).Stream.WriteTo($"{dirPath}/locations.po");
            po2bin.Convert(arm.abilities).Stream.WriteTo($"{dirPath}/abilities.po");
            po2bin.Convert(arm.spells).Stream.WriteTo($"{dirPath}/spells.po");
            po2bin.Convert(arm.enemySpells).Stream.WriteTo($"{dirPath}/enemySpells.po");
            po2bin.Convert(arm.playerSkills).Stream.WriteTo($"{dirPath}/skills.po");
            po2bin.Convert(arm.combatSkillDialogs).Stream.WriteTo($"{dirPath}/combatDialog.po");
            po2bin.Convert(arm.dialogs).Stream.WriteTo($"{dirPath}/dialogs.po");
        }

        public static void importArm(string srcPath, string dirPath) {
            Node container = NodeFactory.FromDirectory(srcPath);

            var arm = new Arm();
            var bin2po = new Binary2Po();

            arm.status = bin2po.Convert(container.Children["status.po"].GetFormatAs<BinaryFormat>());
            arm.outskirtsLoc = bin2po.Convert(container.Children["outskirts.po"].GetFormatAs<BinaryFormat>());
            arm.locations = bin2po.Convert(container.Children["locations.po"].GetFormatAs<BinaryFormat>());
            arm.abilities = bin2po.Convert(container.Children["abilities.po"].GetFormatAs<BinaryFormat>());
            arm.spells = bin2po.Convert(container.Children["spells.po"].GetFormatAs<BinaryFormat>());
            arm.enemySpells = bin2po.Convert(container.Children["enemySpells.po"].GetFormatAs<BinaryFormat>());
            arm.playerSkills = bin2po.Convert(container.Children["skills.po"].GetFormatAs<BinaryFormat>());
            arm.combatSkillDialogs = bin2po.Convert(container.Children["combatDialog.po"].GetFormatAs<BinaryFormat>());
            arm.dialogs = bin2po.Convert(container.Children["dialogs.po"].GetFormatAs<BinaryFormat>());

            new Binary2Arm().Convert(arm).Stream.WriteTo($"{dirPath}/arm9.bin");
        }
    }
}
