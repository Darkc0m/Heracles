using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;
using Yarhl.IO.Serialization.Attributes;

namespace Heracles.Lib.Utils
{
    public class HeraclesWriter : DataWriter
    {
        public HeraclesWriter(DataStream stream) : base(stream) {
            DefaultEncoding = Encoding.GetEncoding("shift_jis");
        }

        public void Write(string s) {
            base.Write(HeraclesReplacer.Encode(s));
        }

        public void Write(string s, int i, bool nullTerminator = true) {
            base.Write(HeraclesReplacer.Encode(s), i, nullTerminator);
        }

        public int GetByteCount(string s) {
            return DefaultEncoding.GetByteCount(HeraclesReplacer.Encode(s)) + 1;
        }

        public void WriteTextPointers32(List<string> texts, uint initialAddress) {
            uint currentAddress = initialAddress;
            foreach (string s in texts) {
                Write(currentAddress);
                currentAddress += (uint)DefaultEncoding.GetByteCount(HeraclesReplacer.Encode(s)) + 1;
            }
        }

        public void WriteTextPointers16(List<string> texts, ushort initialAddress) {
            ushort currentAddress = initialAddress;
            foreach (string s in texts) {
                Write(currentAddress);
                currentAddress += (ushort)(DefaultEncoding.GetByteCount(HeraclesReplacer.Encode(s)) + 1);
            }
        }

        public void WriteTextList(List<string> texts, int padding = 0) {
            foreach (string s in texts) {
                Write(s);
                WritePadding(0x00, padding);
            }
        }
    }
}
