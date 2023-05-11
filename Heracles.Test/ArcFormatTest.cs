using Heracles.Lib.Converters;
using Heracles.Lib.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileSystem;
using Yarhl.IO;

namespace Heracles.Test
{
    public class ArcFormatTest
    {
        string resPath;

        [SetUp]
        public void Setup() {
            string programDir = AppDomain.CurrentDomain.BaseDirectory;
            resPath = Path.GetFullPath(programDir + "/../../../Resources/Arc3/");

            if (!Directory.Exists(resPath))
                Assert.Ignore("Cannot find the folder to run the tests");
        }

        [Test]
        public void ArcTest() {
            //Check Arc
            foreach (var filefound in Directory.GetFiles(resPath, "*.arc", SearchOption.AllDirectories)) {
                using (var node = NodeFactory.FromFile(filefound)) {

                    // 1. Binary -> Arc3
                    var expectedBin = node.GetFormatAs<BinaryFormat>();
                    var binary2Arc3 = new Binary2Arc3(node.Name);
                    Arc3 expectedArc3 = null;
                    try {
                        expectedArc3 = binary2Arc3.Convert(expectedBin);
                    }
                    catch (Exception ex) {
                        Assert.Fail($"Exception Binary -> Arc3 with {node.Path}\n{ex}");
                    }

                    // 2. Arc3 -> NodeContainer
                    NodeContainerFormat expectedNodeContainer = null;
                    var arc3ToContainer = new Arc3ToContainer();
                    try {
                        expectedNodeContainer = arc3ToContainer.Convert(expectedArc3);
                    }
                    catch (Exception ex) {
                        Assert.Fail($"Exception Arc3 -> NodeContainerFormat with {node.Path}\n{ex}");
                    }

                    // 3. NodeContainer -> Arc3
                    Arc3 actualArc3 = null;
                    try {
                        actualArc3 = arc3ToContainer.Convert(expectedNodeContainer);
                    }
                    catch (Exception ex) {
                        Assert.Fail($"Exception NodeContainerFormat -> Arc3 with {node.Path}\n{ex}");
                    }

                    // 4. Arc3 -> Binary
                    BinaryFormat actualBin = null;
                    try {
                        actualBin = binary2Arc3.Convert(actualArc3);
                    }
                    catch (Exception ex) {
                        Assert.Fail($"Exception Arc3 -> Binary with {node.Path}\n{ex}");
                    }

                    // Comparing Binaries
                    Assert.True(expectedBin.Stream.Compare(actualBin.Stream), $"Arcs are not identical: {node.Path}");
                }
            }
        }
    }
}
