using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileFormat;

namespace Heracles.Lib.Formats
{
    public class Tutorial : IFormat
    {
        public byte[] lastMetadata;
        public List<byte[]> metadata;
        public List<string> text;
        public byte[] code;
        public byte[] trailingText;
        public int textStart;

        public Tutorial() {
            metadata = new List<byte[]>();
            text = new List<string>();
        }
    }
}
