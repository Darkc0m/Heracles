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
    public class UIMessFormatTest
    {
        private string resPath;
        [SetUp]
        public void Setup() {
            string programDir = AppDomain.CurrentDomain.BaseDirectory;
            resPath = Path.GetFullPath(programDir + "/../../../" + "Resources/UIMess.bin");

            Assert.True(File.Exists(resPath), "The resource file does not exist", resPath);
        }

        [Test]
        public void UIMessTest() {
            using (var node = NodeFactory.FromFile(resPath)) {
                // BinaryFormat -> UIMess
                var expectedBin = node.GetFormatAs<BinaryFormat>();
                var binary2UIMess = new Binary2UIMess();
                UIMess expectedUIMess = null;
                try {
                    expectedUIMess = binary2UIMess.Convert(expectedBin);
                }
                catch (Exception ex) {
                    Assert.Fail($"Exception BinaryFormat -> UIMess with {node.Path}\n{ex}");
                }

                // UIMess -> Po
                var uiMess2Po = new UIMess2Po();
                Po expectedPo = null;
                try {
                    expectedPo = uiMess2Po.Convert(expectedUIMess);
                }
                catch (Exception ex) {
                    Assert.Fail($"Exception UIMess -> Po with {node.Path}\n{ex}");
                }

                // Po -> UIMess
                UIMess actualUIMess = null;
                try {
                    actualUIMess = uiMess2Po.Convert(expectedPo);
                }
                catch (Exception ex) {
                    Assert.Fail($"Exception Po -> UIMess with {node.Path}\n{ex}");
                }

                // UIMess -> BinaryFormat
                BinaryFormat actualBin = null;
                try {
                    actualBin = binary2UIMess.Convert(actualUIMess);
                }
                catch (Exception ex) {
                    Assert.Fail($"Exception UIMess -> BinaryFormat with {node.Path}\n{ex}");
                }

                // Comparing Binaries
                Assert.True(CompareUI(expectedBin, actualBin), $"UIMess is not identical: {node.Path}");
            }
        }

        private bool CompareUI(BinaryFormat original, BinaryFormat updated) {
            var o = new HeraclesReader(original.Stream);
            var u = new HeraclesReader(updated.Stream);
            o.Stream.Position = 0x00;
            u.Stream.Position = 0x00;

            string sO, sU;
            ushort offsetU, offsetO;
            long textStart = 0, codeStart = 0;
            for(int i = 0; i < 16; i++) {
                offsetO = (ushort)(o.ReadUInt16() * 2);
                offsetU = (ushort)(u.ReadUInt16() * 2);
                textStart = textStart == 0 ? offsetO : textStart;
                if (offsetO == 0 && offsetU == 0)
                    continue;
                o.Stream.PushToPosition(offsetO);
                u.Stream.PushToPosition(offsetU);
                sO = o.ReadString();
                sU = u.ReadString();
                o.Stream.PopPosition();
                u.Stream.PopPosition();
                if (sO != sU)
                    return false;
            }
            for(int i = 0; i < 3; i++) {
                offsetO = (ushort)(o.ReadUInt16() * 2);
                offsetU = (ushort)(u.ReadUInt16() * 2);
                codeStart = i == 0 ? offsetO : codeStart;
                if (offsetO != offsetU)
                    return false;

            }
            while(o.Stream.Position < codeStart) {
                offsetO = (ushort)(o.ReadUInt16() * 2);
                offsetU = (ushort)(u.ReadUInt16() * 2);
                if (offsetO == 0 && offsetU == 0)
                    continue;
                o.Stream.PushToPosition(offsetO);
                u.Stream.PushToPosition(offsetU);
                sO = o.ReadString();
                sU = u.ReadString();
                o.Stream.PopPosition();
                u.Stream.PopPosition();
                if (sO != sU)
                    return false;
            }
            while(o.Stream.Position < textStart) {
                offsetO = (ushort)(o.ReadUInt16() * 2);
                offsetU = (ushort)(u.ReadUInt16() * 2);
                if (offsetO != offsetU)
                    return false;
            }

            return true;
        }
    }
}
