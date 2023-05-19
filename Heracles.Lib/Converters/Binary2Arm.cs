using Heracles.Lib.Formats;
using Heracles.Lib.Utils;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileFormat;
using Yarhl.IO;
using Yarhl.Media.Text;

namespace Heracles.Lib.Converters
{
    public class Binary2Arm : IConverter<BinaryFormat, Arm>, IConverter<Arm, BinaryFormat>
    {
        public const int ARM_POINTER_BASE = -0x02000000;

        public const long STATUS_OFFSET = 0x0ED25C;
        public const int STATUS_ENTRIES = 47, STATUS_METADATA_SIZE = 0x06, STATUS_TEXT_SIZE = 0x18;

        public const long OUTS_OFFSET = 0x0F12CC;
        public const int OUTS_ENTRIES = 19, OUTS_METADATA_SIZE = 0x04, OUTS_TEXT_SIZE = 0x20;

        public const long LOCATIONS_OFFSET = 0x0F76B8;
        public const int LOCATIONS_ENTRIES = 275, LOCATIONS_METADATA_SIZE = 0x04, LOCATIONS_TEXT_SIZE = 0x20;

        public const long ABILITIES_OFFSET = 0x113644;
        public const int ABILITIES_ENTRIES = 47;

        public const long SKILLS_OFFSET = 0x116B94;
        public const int SKILLS_ENTRIES = 173;

        public const long SPELLS_OFFSET = 0x113D9C;
        public const int SPELLS_ENTRIES = 80, SPELLS_METADATA_SIZE = 0x2C;

        public const long ENEMY_SPELLS_OFFSET = 0x1152DC;
        public const int ENEMY_SPELLS_ENTRIES = 113, ENEMY_SPELLS_METADATA_SIZE = 0X20;

        public const long DIALOGS_OFFSET = 0x11A1B0;
        public const int DIALOGS_ENTRIES = 931;

        public const long COMBAT_SKILLS_DIALOGS_OFFSET = 0x0ED054;
        public const int COMBAT_SKILLS_DIALOGS_ENTRIES = 58;

        public const uint TEXT_BLOCK_1_OFFSET_START = 0x1105F0, TEXT_BLOCK_1_OFFSET_END = 0x113644;
        public const uint TEXT_BLOCK_2_OFFSET_START = 0x11B044, TEXT_BLOCK_2_OFFSET_END = 0x11ED44;

        uint writingOffset = TEXT_BLOCK_1_OFFSET_START;
        Dictionary<string, uint> texts;

        public Arm Convert(BinaryFormat bin) {
            var reader = new HeraclesReader(bin.Stream);
            var arm = new Arm();

            arm.original = reader.ReadBytes((int)reader.Stream.Length);
            reader.Stream.Position = 0x00;

            arm.combatSkillDialogs = ReadCombatSkillDialog(reader, COMBAT_SKILLS_DIALOGS_OFFSET, COMBAT_SKILLS_DIALOGS_ENTRIES);
            arm.status = readArray(reader, STATUS_OFFSET, STATUS_ENTRIES, STATUS_METADATA_SIZE, STATUS_TEXT_SIZE);
            arm.status.Header.Extensions.Add("Original", System.Convert.ToBase64String(arm.original));
            arm.outskirtsLoc = readArray(reader, OUTS_OFFSET, OUTS_ENTRIES, OUTS_METADATA_SIZE, OUTS_TEXT_SIZE);
            arm.locations = readArray(reader, LOCATIONS_OFFSET, LOCATIONS_ENTRIES, LOCATIONS_METADATA_SIZE, LOCATIONS_TEXT_SIZE);
            arm.abilities = readAbilities(reader, ABILITIES_OFFSET, ABILITIES_ENTRIES);
            arm.spells = readSpells(reader, SPELLS_OFFSET, SPELLS_ENTRIES, SPELLS_METADATA_SIZE, false);
            arm.enemySpells = readSpells(reader, ENEMY_SPELLS_OFFSET, ENEMY_SPELLS_ENTRIES, ENEMY_SPELLS_METADATA_SIZE, true);
            arm.playerSkills = readSkills(reader, SKILLS_OFFSET, SKILLS_ENTRIES);
            arm.dialogs = readDialogs(reader, DIALOGS_OFFSET, DIALOGS_ENTRIES);

            return arm;
        }

        public BinaryFormat Convert(Arm arm) {
            var bin = new BinaryFormat();
            var originalBin = new BinaryFormat();
            var originalWriter = new HeraclesWriter(originalBin.Stream);
            var originalReader = new HeraclesReader(originalBin.Stream);
            var writer = new HeraclesWriter(bin.Stream);

            //Write unmodified first part of the arm
            originalWriter.Write(System.Convert.FromBase64String(arm.status.Header.Extensions["Original"]));
            originalReader.Stream.Position = 0x00;
            writer.Write(originalReader.ReadBytes((int)COMBAT_SKILLS_DIALOGS_OFFSET));

            //Write combat skill dialogs
            WriteCombatSkillDialog(writer, arm.combatSkillDialogs);
            originalReader.Stream.Position = writer.Stream.Position;
            writer.Write(originalReader.ReadBytes((int)(STATUS_OFFSET - writer.Stream.Position)));

            //Write status
            writeArray(writer, arm.status, STATUS_TEXT_SIZE);
            originalReader.Stream.Position = writer.Stream.Position;
            writer.Write(originalReader.ReadBytes((int)(OUTS_OFFSET - writer.Stream.Position)));

            //Write outskirts loc
            writeArray(writer, arm.outskirtsLoc, OUTS_TEXT_SIZE);
            originalReader.Stream.Position = writer.Stream.Position;
            writer.Write(originalReader.ReadBytes((int)(LOCATIONS_OFFSET - writer.Stream.Position)));

            //Write locations
            writeArray(writer, arm.locations, LOCATIONS_TEXT_SIZE);
            originalReader.Stream.Position = writer.Stream.Position;
            writer.Write(originalReader.ReadBytes((int)(ABILITIES_OFFSET - writer.Stream.Position)));

            //Write combat data
            writeAbilities(writer, arm.abilities, ABILITIES_ENTRIES);
            writeSpells(writer, arm.spells, false);
            writeSpells(writer, arm.enemySpells, true);
            writeSkills(writer, arm.playerSkills);
            originalReader.Stream.Position = writer.Stream.Position;
            writer.Write(originalReader.ReadBytes((int)(DIALOGS_OFFSET - writer.Stream.Position)));

            //Write dialogs
            writeDialogs(writer, arm.dialogs, DIALOGS_ENTRIES);
            originalReader.Stream.Position = writer.Stream.Position;
            writer.Write(originalReader.ReadBytes((int)(originalReader.Stream.Length - writer.Stream.Position)));

            //Write texts
            writeTexts(writer);

            return bin;
        }

        private uint addPointerString(string s) {
            if (texts == null) {
                texts = new Dictionary<string, uint>();
            }
                

            if (texts.ContainsKey(s))
                return texts[s] + 0x02000000;

            uint offset = writingOffset;
            var w = new HeraclesWriter(new DataStream());
            uint textSize = (uint)w.GetByteCount(s) + 1;
            if (writingOffset < TEXT_BLOCK_1_OFFSET_END &&
                writingOffset + textSize >= TEXT_BLOCK_1_OFFSET_END) {
                writingOffset = TEXT_BLOCK_2_OFFSET_START + textSize;
                offset = TEXT_BLOCK_2_OFFSET_START;
            }else if(writingOffset + textSize >= TEXT_BLOCK_2_OFFSET_END) {
                throw new Exception("Text is bigger than block space");
            }
            else {
                writingOffset += textSize;
            }

            texts.Add(s, offset);

            return offset + 0x02000000;
        }

        private void writeTexts(HeraclesWriter writer) {
            foreach(KeyValuePair<string, uint> kv in texts) {
                writer.Stream.Position = kv.Value;
                writer.Write(kv.Key);
            }
        }

        private Po readArray(HeraclesReader reader, long offset, int numEntries, int metadataSize, int textSize) {
            var po = HeraclesPo.generate();
            reader.Stream.Position = offset;

            for(int i = 0; i < numEntries; i++) {
                byte[] metadata = reader.ReadBytes(metadataSize);
                string text = reader.ReadString(textSize);
                po.Add(new PoEntry(text)
                {
                    Context = $"{i}",
                    ExtractedComments = $"{System.Convert.ToBase64String(metadata)}"
                });
            }

            return po;
        }

        private Po readAbilities(HeraclesReader reader, long offset, int numEntries) {
            var po = HeraclesPo.generate();
            reader.Stream.Position = offset;

            for(int i = 0; i < numEntries; i++) {
                byte[] metadata;
                metadata = reader.ReadBytes(0x10);
                string name = reader.ReadString(0x14);
                string description = reader.ReadPointer32String(ARM_POINTER_BASE);
                po.Add(new PoEntry(name)
                {
                    Context = $"{i * 2}",
                    ExtractedComments = System.Convert.ToBase64String(metadata)
                });
                po.Add(new PoEntry(description)
                {
                    Context = $"{i * 2 + 1}"
                });
            }

            return po;
        }

        private void writeAbilities(HeraclesWriter writer, Po po, int numEntries) {
            for(int i = 0; i < numEntries; i++) {
                writer.Write(System.Convert.FromBase64String(po.Entries[i * 2].ExtractedComments));
                writer.Write(po.Entries[i * 2].Text, 0x14, false);
                writer.Write(addPointerString(po.Entries[i * 2 + 1].Text));
            }
        }

        private Po readSpells(HeraclesReader reader, long offset, int numEntries, int metadataSize, bool ignoreDescription) {
            var po = HeraclesPo.generate();
            reader.Stream.Position = offset;

            for(int i = 0; i < numEntries; i++) {
                string name = reader.ReadString(0x14);
                byte[] metadata = reader.ReadBytes(metadataSize);
                string description = reader.ReadPointer32String(ARM_POINTER_BASE);
                po.Add(new PoEntry(name)
                {
                    Context = ignoreDescription ? $"{i}" : $"{i * 2}",
                    ExtractedComments = System.Convert.ToBase64String(metadata)
                }) ;
                if (!ignoreDescription) {
                    po.Add(new PoEntry(description)
                    {
                        Context = $"{i * 2 + 1}",
                    });
                }
            }

            return po;
        }

        private void writeSpells(HeraclesWriter writer, Po po, bool ignoreDescription) {
            int count = ignoreDescription ? po.Entries.Count : po.Entries.Count / 2;
            for (int i = 0; i < count; i++) {
                writer.Write(po.Entries[ignoreDescription ? i : i * 2].Text, 0x14, false);
                writer.Write(System.Convert.FromBase64String(po.Entries[ignoreDescription ? i : i * 2].ExtractedComments));
                if (ignoreDescription) {
                    writer.Write(addPointerString("<!empty>"));
                }
                else {
                    writer.Write(addPointerString(po.Entries[i * 2 + 1].Text));
                }
            }
        }

        private Po readSkills(HeraclesReader reader, long offset, int numEntries) {
            var po = HeraclesPo.generate();
            reader.Stream.Position = offset;

            for(int i = 0; i < numEntries; i++) {
                byte[] dataName = reader.ReadBytes(0x14);
                string name = reader.ReadString(0x14);
                string desc;
                byte[] dataDesc;
                if(name != "<!empty>") {
                    desc = reader.ReadPointer32String(ARM_POINTER_BASE);
                    dataDesc = reader.ReadBytes(0x24);
                }
                else {
                    desc = "<!empty>";
                    dataDesc = reader.ReadBytes(0x28);
                }
                po.Add(new PoEntry(name)
                {
                    Context = $"{i * 2}",
                    ExtractedComments = System.Convert.ToBase64String(dataName)
                });
                po.Add(new PoEntry(desc)
                {
                    Context = $"{i * 2 + 1}",
                    ExtractedComments = System.Convert.ToBase64String(dataDesc)
                });
            }

            return po;
        }

        private void writeSkills(HeraclesWriter writer, Po po) {
            for(int i = 0; i < po.Entries.Count / 2; i++) {
                writer.Write(System.Convert.FromBase64String(po.Entries[i * 2].ExtractedComments));
                writer.Write(po.Entries[i * 2].Text, 0x14);
                if (po.Entries[i * 2].Text != "<!empty>") {
                    writer.Write(addPointerString(po.Entries[i * 2 + 1].Text));
                }
                writer.Write(System.Convert.FromBase64String(po.Entries[i * 2 + 1].ExtractedComments));
            }
        }

        private Po readDialogs(HeraclesReader reader, long offset, int numEntries) {
            var po = HeraclesPo.generate();
            reader.Stream.Position = offset;

            for(int i = 0; i < numEntries; i++) {
                if(reader.ReadUInt32() != 0) {
                    reader.Stream.Position -= 0x04;
                    po.Add(new PoEntry(reader.ReadPointer32String(ARM_POINTER_BASE))
                    {
                        Context = $"{i}"
                    });
                }
            }

            return po;
        }

        private void writeDialogs(HeraclesWriter writer, Po po, int numEntries) {
            int i = 0, j = 0;
            while (i < po.Entries.Count) {
                if (int.Parse(po.Entries[i].Context) == j){
                    writer.Write(addPointerString(po.Entries[i].Text));
                    i++;
                    j++;
                }
                else {
                    while (j < int.Parse(po.Entries[i].Context)) {
                        writer.Write(0x00);
                        j++;
                    }
                }
            }
        }

        private Po ReadCombatSkillDialog(HeraclesReader reader, long offset, int numEntries) {
            var po = HeraclesPo.generate();
            reader.Stream.Position = offset;

            for(int i = 0; i < numEntries; i++) {
                byte[] metadata = reader.ReadBytes(4);
                if (reader.ReadUInt32() == 0)
                    continue;
                else
                    reader.Stream.Position -= 0x04;
                       
                po.Add(new PoEntry(reader.ReadPointer32String(ARM_POINTER_BASE))
                {
                    Context = $"{i}",
                    ExtractedComments = System.Convert.ToBase64String(metadata)
                });
            }

            return po;
        }

        private void WriteCombatSkillDialog(HeraclesWriter writer, Po po) {
            int i = 0, j = 0;
            while(i < po.Entries.Count) {
                if (int.Parse(po.Entries[i].Context) == j) {
                    writer.Write(System.Convert.FromBase64String(po.Entries[i].ExtractedComments));
                    writer.Write(addPointerString(po.Entries[i].Text));
                    i++;
                    j++;
                }
                else {
                    while(j < int.Parse(po.Entries[i].Context)) {
                        writer.Write(0x00);
                        writer.Write(0x00);
                        j++;
                    }
                }
            }
        }

        private void writeArray(HeraclesWriter writer, Po po, int textSize) {
            foreach(PoEntry entry in po.Entries) {
                writer.Write(System.Convert.FromBase64String(entry.ExtractedComments));
                writer.Write(entry.Text, textSize, false);
            }
        }
    }
}
