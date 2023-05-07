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
    public class DataDictionaryFormatTest
    {
        private string resPath;
        [SetUp]
        public void Setup() {
            string programDir = AppDomain.CurrentDomain.BaseDirectory;
            resPath = Path.GetFullPath(programDir + "/../../../Resources/DataDictionary.eng");

            Assert.True(File.Exists(resPath), "The resource file does not exist", resPath);
        }

        [Test]
        public void DataDictionaryTest() {
            using (var node = NodeFactory.FromFile(resPath)) {
                // BinaryFormat -> DataDictionary
                var expectedBin = node.GetFormatAs<BinaryFormat>();
                var binary2DataDictionary = new Binary2DataDictionary();
                DataDictionary expectedDataDictionary = null;
                try {
                    expectedDataDictionary = binary2DataDictionary.Convert(expectedBin);
                }
                catch (Exception ex) {
                    Assert.Fail($"Exception BinaryFormat -> DataDictionary with {node.Path}\n{ex}");
                }

                // DataDictionary -> Po
                var DataDictionary2Po = new DataDictionary2Po();
                Po expectedPo = null;
                try {
                    expectedPo = DataDictionary2Po.Convert(expectedDataDictionary);
                }
                catch (Exception ex) {
                    Assert.Fail($"Exception DataDictionary -> Po with {node.Path}\n{ex}");
                }

                //new Po2Binary().Convert(expectedPo).Stream.WriteTo(AppDomain.CurrentDomain.BaseDirectory + "/../../../" + "Resources/DataDictionary.po");

                // Po -> DataDictionary
                DataDictionary actualDataDictionary = null;
                try {
                    actualDataDictionary = DataDictionary2Po.Convert(expectedPo);
                }
                catch (Exception ex) {
                    Assert.Fail($"Exception Po -> DataDictionary with {node.Path}\n{ex}");
                }

                // DataDictionary -> BinaryFormat
                BinaryFormat actualBin = null;
                try {
                    actualBin = binary2DataDictionary.Convert(actualDataDictionary);
                }
                catch (Exception ex) {
                    Assert.Fail($"Exception DataDictionary -> BinaryFormat with {node.Path}\n{ex}");
                }

                // Comparing Binaries
                Assert.True(expectedBin.Stream.Compare(actualBin.Stream), $"DataDictionary is not identical: {node.Path}");
            }
        }
    }
}
