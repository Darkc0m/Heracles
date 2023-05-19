using Heracles.Lib.Converters;
using Heracles.Lib.Formats;
using Heracles.Lib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileSystem;
using Yarhl.IO;
using Yarhl.Media.Text;

namespace Heracles.Test
{
    public class ArmFormatTest
    {
        private string resPath;
        [SetUp]
        public void Setup() {
            string programDir = AppDomain.CurrentDomain.BaseDirectory;
            resPath = Path.GetFullPath(programDir + "/../../../" + "Resources/arm9.bin");

            Assert.True(File.Exists(resPath), "The resource file does not exist", resPath);
        }

        [Test]
        public void ArmTest() {
            using (var node = NodeFactory.FromFile(resPath)) {
                // BinaryFormat -> Arm
                var expectedBin = node.GetFormatAs<BinaryFormat>();
                var binary2Arm = new Binary2Arm();
                Arm expectedArm = null;
                try {
                    expectedArm = binary2Arm.Convert(expectedBin);
                }
                catch (Exception ex) {
                    Assert.Fail($"Exception BinaryFormat -> Arm with {node.Path}\n{ex}");
                }

                // Arm -> BinaryFormat
                BinaryFormat actualBin = null;
                try {
                    actualBin = binary2Arm.Convert(expectedArm);
                }
                catch (Exception ex) {
                    Assert.Fail($"Exception Arm -> BinaryFormat with {node.Path}\n{ex}");
                }

                // Comparing Binaries
                Assert.True(compareArms(expectedBin, actualBin));
            }
        }

        private bool compareArms(BinaryFormat originalBin, BinaryFormat updatedBin) {
            var original = new HeraclesReader(originalBin.Stream);
            var updated = new HeraclesReader(updatedBin.Stream);
            original.Stream.Position = 0x00;
            updated.Stream.Position = 0x00;

            if (original.Stream.Length != updated.Stream.Length)
                return false;

            if (!compareBytes(original, updated, Binary2Arm.COMBAT_SKILLS_DIALOGS_OFFSET))
                return false;

            for(int i = 0; i < Binary2Arm.COMBAT_SKILLS_DIALOGS_ENTRIES; i++) {
                if (!compareBytes(original, updated, 0x04))
                    return false;
                if (!comparePointerStrings(original, updated))
                    return false;
            }

            if (!compareBytes(original, updated, Binary2Arm.STATUS_OFFSET - updated.Stream.Position))
                return false;

            if (!compareBytes(original, updated, Binary2Arm.OUTS_OFFSET - updated.Stream.Position))
                return false;

            if (!compareBytes(original, updated, Binary2Arm.LOCATIONS_OFFSET - updated.Stream.Position))
                return false;

            if (!compareBytes(original, updated, Binary2Arm.TEXT_BLOCK_1_OFFSET_START - updated.Stream.Position))
                return false;

            original.Stream.Position = Binary2Arm.TEXT_BLOCK_1_OFFSET_END;
            updated.Stream.Position = Binary2Arm.TEXT_BLOCK_1_OFFSET_END;

            for(int i = 0; i < Binary2Arm.ABILITIES_ENTRIES; i++) {
                if (!compareBytes(original, updated, 0x24))
                    return false;
                if (!comparePointerStrings(original, updated))
                    return false;
            }

            for(int i = 0; i < Binary2Arm.SPELLS_ENTRIES; i++) {
                if (!compareBytes(original, updated, 0x14 + Binary2Arm.SPELLS_METADATA_SIZE))
                    return false;
                if (!comparePointerStrings(original, updated))
                    return false;
            }

            for (int i = 0; i < Binary2Arm.ENEMY_SPELLS_ENTRIES; i++) {
                if (!compareBytes(original, updated, 0x14 + Binary2Arm.ENEMY_SPELLS_METADATA_SIZE))
                    return false;
                original.Stream.Position += 0x04;
                updated.Stream.Position += 0x04;
            }

            for (int i = 0; i < Binary2Arm.SKILLS_ENTRIES; i++) {
                if (!compareBytes(original, updated, 0x28))
                    return false;
                if (!comparePointerStrings(original, updated))
                    return false;
                if (!compareBytes(original, updated, 0x24))
                    return false;
            }

            if (!compareBytes(original, updated, Binary2Arm.DIALOGS_OFFSET - updated.Stream.Position))
                return false;

            for (int i = 0; i < Binary2Arm.DIALOGS_ENTRIES; i++) {
                if (!comparePointerStrings(original, updated))
                    return false;
            }

            if (!compareBytes(original, updated, Binary2Arm.TEXT_BLOCK_2_OFFSET_START - updated.Stream.Position))
                return false;

            original.Stream.Position = Binary2Arm.TEXT_BLOCK_2_OFFSET_END;
            updated.Stream.Position = Binary2Arm.TEXT_BLOCK_2_OFFSET_END;

            if (!compareBytes(original, updated, updated.Stream.Length - updated.Stream.Position))
                return false;

            return true;
        }

        private bool compareBytes(HeraclesReader rA, HeraclesReader rB, long size) {
            return Enumerable.SequenceEqual(rA.ReadBytes((int)size), rB.ReadBytes((int)size));
        }

        private bool comparePointerStrings(HeraclesReader rA, HeraclesReader rB) {
            uint offsetA = rA.ReadUInt32(), offsetB = rB.ReadUInt32();
            if (offsetA == 0 && offsetB == 0)
                return true;

            rA.Stream.PushToPosition(offsetA - 0x02000000);
            rB.Stream.PushToPosition(offsetB - 0x02000000);
            string a = rA.ReadString(), b = rB.ReadString();
            rA.Stream.PopPosition();
            rB.Stream.PopPosition();
            return a == b;
        }
    }
}
