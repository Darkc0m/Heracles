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
    public class ItemdataFormatTest
    {
        private string resPath;
        [SetUp]
        public void Setup() {
            string programDir = AppDomain.CurrentDomain.BaseDirectory;
            resPath = Path.GetFullPath(programDir + "/../../../" + "Resources/itemdata.eng");

            Assert.True(File.Exists(resPath), "The resource file does not exist", resPath);
        }

        [Test]
        public void ItemdataTest() {
            using (var node = NodeFactory.FromFile(resPath)) {
                // BinaryFormat -> Itemdata
                var expectedBin = node.GetFormatAs<BinaryFormat>();
                var binary2Itemdata = new Binary2Itemdata();
                Itemdata expectedItemdata = null;
                try {
                    expectedItemdata = binary2Itemdata.Convert(expectedBin);
                }
                catch (Exception ex) {
                    Assert.Fail($"Exception BinaryFormat -> Itemdata with {node.Path}\n{ex}");
                }

                // Itemdata -> Po
                var itemdata2Po = new Itemdata2Po();
                Po expectedPo = null;
                try {
                    expectedPo = itemdata2Po.Convert(expectedItemdata);
                }
                catch (Exception ex) {
                    Assert.Fail($"Exception Itemdata -> Po with {node.Path}\n{ex}");
                }

                new Po2Binary().Convert(expectedPo).Stream.WriteTo(AppDomain.CurrentDomain.BaseDirectory + "/../../../" + "Resources/itemdata.po");

                // Po -> Itemdata
                Itemdata actualItemdata = null;
                try {
                    actualItemdata = itemdata2Po.Convert(expectedPo);
                }
                catch (Exception ex) {
                    Assert.Fail($"Exception Po -> Itemdata with {node.Path}\n{ex}");
                }

                // Itemdata -> BinaryFormat
                BinaryFormat actualBin = null;
                try {
                    actualBin = binary2Itemdata.Convert(actualItemdata);
                }
                catch (Exception ex) {
                    Assert.Fail($"Exception Itemdata -> BinaryFormat with {node.Path}\n{ex}");
                }

                //actualBin.Stream.WriteTo(AppDomain.CurrentDomain.BaseDirectory + "/../../../" + "Resources/new.eng");
                // Comparing Binaries
                Assert.True(expectedBin.Stream.Compare(actualBin.Stream), $"Itemdata is not identical: {node.Path}");
            }
        }
    }
}
