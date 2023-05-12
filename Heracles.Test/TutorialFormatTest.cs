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

                    new Po2Binary().Convert(expectedPo).Stream.WriteTo(AppDomain.CurrentDomain.BaseDirectory + "/../../../" + $"Resources/{node.Name}.po");

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

        private bool CompareTutorials(BinaryFormat original, BinaryFormat updated) {
            var o = new HeraclesReader(original.Stream);
            var u = new HeraclesReader(updated.Stream);
            o.Stream.Position = 0x00;
            u.Stream.Position = 0x00;

            ushort valueO, valueU;
            string stringO, stringU;
            long textStart = o.Stream.Length;
            long textEndO = 0, textEndU = 0;
            do {
                valueO = o.ReadUInt16();
                valueU = u.ReadUInt16();
                if (valueO != valueU)
                    return false;
                else if (valueO == 0x1100) {
                    uint offsetO = (uint)(o.ReadUInt16() * 2);
                    uint offsetU = (uint)(u.ReadUInt16() * 2);
                    textStart = offsetO < textStart ? offsetO : textStart;
                    o.Stream.PushToPosition(offsetO);
                    u.Stream.PushToPosition(offsetU);
                    stringO = o.ReadString();
                    stringU = u.ReadString();
                    textEndO = o.Stream.Position > textEndO ? o.Stream.Position : textEndO;
                    textEndU = u.Stream.Position > textEndU ? u.Stream.Position : textEndU;
                    if (stringO != stringU)
                        return false;
                    u.Stream.PopPosition();
                    o.Stream.PopPosition();
                }
            } while (o.Stream.Position < textStart);

            u.Stream.Position = textEndU;
            o.Stream.Position = textEndO;
            u.SkipPadding(0x02);
            o.SkipPadding(0x02);
            if (o.Stream.Length - o.Stream.Position != u.Stream.Length - u.Stream.Position)
                return false;

            do {
                valueO = o.ReadUInt16();
                valueU = u.ReadUInt16();
                if (valueO != valueU)
                    return false;
            } while (!o.Stream.EndOfStream);

            return true;
        }
    }
}
