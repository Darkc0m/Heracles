using Heracles.Lib.Converters;
using Heracles.Lib.Formats;
using Heracles.Lib.Utils;
using NUnit.Framework.Internal;
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
    public class TutorialFormatTest
    {
        private string resPath;
        [SetUp]
        public void Setup() {
            string programDir = AppDomain.CurrentDomain.BaseDirectory;
            resPath = Path.GetFullPath(programDir + "/../../../Resources/Tutorial/");

            if (!Directory.Exists(resPath))
                Assert.Ignore("Cannot find the folder to run the tests");
        }

        [Test]
        public void TutorialTest() {
            foreach (var filefound in Directory.GetFiles(resPath, "*.bin", SearchOption.AllDirectories)) {
                using (var node = NodeFactory.FromFile(filefound)) {
                    // BinaryFormat -> Tutorial
                    var expectedBin = node.GetFormatAs<BinaryFormat>();
                    var binary2Tutorial = new Binary2Tutorial();
                    Tutorial expectedTutorial = null;
                    try {
                        expectedTutorial = binary2Tutorial.Convert(expectedBin);
                    }
                    catch (Exception ex) {
                        Assert.Fail($"Exception BinaryFormat -> Tutorial with {node.Path}\n{ex}");
                    }

                    // Tutorial -> Po
                    var tutorial2Po = new Tutorial2Po();
                    Po expectedPo = null;
                    try {
                        expectedPo = tutorial2Po.Convert(expectedTutorial);
                    }
                    catch (Exception ex) {
                        Assert.Fail($"Exception Tutorial -> Po with {node.Path}\n{ex}");
                    }

                    //new Po2Binary().Convert(expectedPo).Stream.WriteTo(AppDomain.CurrentDomain.BaseDirectory + "/../../../" + $"Resources/{node.Name}.po");

                    // Po -> Tutorial
                    Tutorial actualTutorial = null;
                    try {
                        actualTutorial = tutorial2Po.Convert(expectedPo);
                    }
                    catch (Exception ex) {
                        Assert.Fail($"Exception Po -> Tutorial with {node.Path}\n{ex}");
                    }

                    // Tutorial -> BinaryFormat
                    BinaryFormat actualBin = null;
                    try {
                        actualBin = binary2Tutorial.Convert(actualTutorial);
                    }
                    catch (Exception ex) {
                        Assert.Fail($"Exception Tutorial -> BinaryFormat with {node.Path}\n{ex}");
                    }

                    actualBin.Stream.WriteTo(AppDomain.CurrentDomain.BaseDirectory + "/../../../" + $"Resources/{node.Name}.bin");

                    // Comparing Binaries
                    Assert.True(CompareTutorials(expectedBin, actualBin), $"Tutorial is not identical: {node.Path}");
                }
            }
        }

        private bool CompareTutorials(BinaryFormat originalBin, BinaryFormat updatedBin) {
            var original = new HeraclesReader(originalBin.Stream);
            var updated = new HeraclesReader(updatedBin.Stream);
            original.Stream.Position = 0x00;
            updated.Stream.Position = 0x00;

            ushort valueO, valueU;
            string stringO, stringU;
            long textStart = original.Stream.Length;
            long textEndO = 0, textEndU = 0;
            do {
                valueO = original.ReadUInt16();
                valueU = updated.ReadUInt16();
                if (valueO != valueU)
                    return false;
                else if (valueO == 0x1100) {
                    uint offsetO = (uint)(original.ReadUInt16() * 2);
                    uint offsetU = (uint)(updated.ReadUInt16() * 2);
                    textStart = offsetO < textStart ? offsetO : textStart;
                    original.Stream.PushToPosition(offsetO);
                    updated.Stream.PushToPosition(offsetU);
                    stringO = original.ReadString();
                    stringU = updated.ReadString();
                    textEndO = original.Stream.Position > textEndO ? original.Stream.Position : textEndO;
                    textEndU = updated.Stream.Position > textEndU ? updated.Stream.Position : textEndU;
                    if (stringO != stringU)
                        return false;
                    updated.Stream.PopPosition();
                    original.Stream.PopPosition();
                }
            } while (original.Stream.Position < textStart);

            updated.Stream.Position = textEndU;
            original.Stream.Position = textEndO;
            updated.SkipPadding(0x02);
            original.SkipPadding(0x02);
            if (original.Stream.Length - original.Stream.Position != updated.Stream.Length - updated.Stream.Position)
                return false;

            do {
                valueO = original.ReadUInt16();
                valueU = updated.ReadUInt16();
                if (valueO != valueU)
                    return false;
            } while (!original.Stream.EndOfStream);

            return true;
        }
    }
}
