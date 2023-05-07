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

namespace Heracles.Test
{
    public class ScedicFormatTest
    {
        private string resPath;
        [SetUp]
        public void Setup() {
            string programDir = AppDomain.CurrentDomain.BaseDirectory;
            resPath = Path.GetFullPath(programDir + "/../../../" + "Resources/SCEDIC.eng");

            Assert.True(File.Exists(resPath), "The resource file does not exist", resPath);
        }

        [Test]
        public void ScedicTest() {
            using (var node = NodeFactory.FromFile(resPath)) {
                // BinaryFormat -> Scedic
                var expectedBin = node.GetFormatAs<BinaryFormat>();
                var binary2Scedic = new Binary2Scedic();
                Scedic expectedScedic = null;
                try {
                    expectedScedic = binary2Scedic.Convert(expectedBin);
                }
                catch (Exception ex) {
                    Assert.Fail($"Exception BinaryFormat -> Scedic with {node.Path}\n{ex}");
                }

                // Scedic -> Po
                var scedic2Po = new Scedic2Po();
                Po expectedPo = null;
                try {
                    expectedPo = scedic2Po.Convert(expectedScedic);
                }
                catch (Exception ex) {
                    Assert.Fail($"Exception Scedic -> Po with {node.Path}\n{ex}");
                }

                //new Po2Binary().Convert(expectedPo).Stream.WriteTo(AppDomain.CurrentDomain.BaseDirectory + "/../../../" + "Resources/SCEDIC.po");

                // Po -> Scedic
                Scedic actualScedic = null;
                try {
                    actualScedic = scedic2Po.Convert(expectedPo);
                }
                catch (Exception ex) {
                    Assert.Fail($"Exception Po -> Scedic with {node.Path}\n{ex}");
                }

                // Scedic -> BinaryFormat
                BinaryFormat actualBin = null;
                try {
                    actualBin = binary2Scedic.Convert(actualScedic);
                }
                catch (Exception ex) {
                    Assert.Fail($"Exception Scedic -> BinaryFormat with {node.Path}\n{ex}");
                }

                // Comparing Binaries
                Assert.True(expectedBin.Stream.Compare(actualBin.Stream), $"Scedic is not identical: {node.Path}");
            }
        }
    }
}
