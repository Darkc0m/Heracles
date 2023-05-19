using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileFormat;

namespace Heracles.Lib.Formats
{
    public class UIMess : IFormat
    {
        public Dictionary<int, string> idText;
        public ushort dataOffset1;
        public ushort dataOffset2;
        public ushort dataOffset3;
        public ushort textStart;
        public byte[] code;

        public UIMess() {
            idText = new Dictionary<int, string>();
        }
    }
}
