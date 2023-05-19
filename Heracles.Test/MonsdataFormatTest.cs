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
    public class MonsdataFormatTest
    {
        private string resPath;
        [SetUp]
        public void Setup() {
            string programDir = AppDomain.CurrentDomain.BaseDirectory;
            resPath = Path.GetFullPath(programDir + "/../../../" + "Resources/monsdata_e.bin");

            Assert.True(File.Exists(resPath), "The resource file does not exist", resPath);
        }

        [Test]
        public void MonsdataTest() {
            using (var node = NodeFactory.FromFile(resPath)) {
                // BinaryFormat -> Monsdata
                var expectedBin = node.GetFormatAs<BinaryFormat>();
                var binary2Monsdata = new Binary2Monsdata();
                Monsdata expectedMonsdata = null;
                try {
                    expectedMonsdata = binary2Monsdata.Convert(expectedBin);
                }
                catch (Exception ex) {
                    Assert.Fail($"Exception BinaryFormat -> Monsdata with {node.Path}\n{ex}");
                }

                // Monsdata -> Po
                var Monsdata2Po = new Monsdata2Po();
                Po expectedPo = null;
                try {
                    expectedPo = Monsdata2Po.Convert(expectedMonsdata);
                }
                catch (Exception ex) {
                    Assert.Fail($"Exception Monsdata -> Po with {node.Path}\n{ex}");
                }

                //new Po2Binary().Convert(expectedPo).Stream.WriteTo(AppDomain.CurrentDomain.BaseDirectory + "/../../../" + "Resources/Monsdata.po");

                // Po -> Monsdata
                Monsdata actualMonsdata = null;
                try {
                    actualMonsdata = Monsdata2Po.Convert(expectedPo);
                }
                catch (Exception ex) {
                    Assert.Fail($"Exception Po -> Monsdata with {node.Path}\n{ex}");
                }

                // Monsdata -> BinaryFormat
                BinaryFormat actualBin = null;
                try {
                    actualBin = binary2Monsdata.Convert(actualMonsdata);
                }
                catch (Exception ex) {
                    Assert.Fail($"Exception Monsdata -> BinaryFormat with {node.Path}\n{ex}");
                }

                // Comparing Binaries
                Assert.True(expectedBin.Stream.Compare(actualBin.Stream), $"Monsdata is not identical: {node.Path}");
            }
        }
    }
}
