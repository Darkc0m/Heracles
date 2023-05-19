using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace Heracles.Lib.Utils
{
    public class HeraclesReader : DataReader
    {
        public HeraclesReader(DataStream stream) : base(stream) {
            DefaultEncoding = Encoding.GetEncoding("shift_jis");
        }

        public string ReadString() {
            return HeraclesReplacer.Decode(base.ReadString());
        }

        public string ReadString(int num) {
            return HeraclesReplacer.Decode(base.ReadString(num).TrimEnd('\0'));
        }

        public string ReadPointer32String(int pointerBase = 0) {
            Stream.PushToPosition(ReadUInt32() + pointerBase);
            string s = ReadString();
            Stream.PopPosition();
            return s;
        }

        public string ReadPointer16String(uint pointerBase = 0) {
            Stream.PushToPosition(ReadUInt16() + pointerBase);
            string s = ReadString();
            Stream.PopPosition();
            return s;
        }
    }
}
