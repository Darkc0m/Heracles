using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileFormat;

namespace Heracles.Lib.Formats
{
    public class Arc3 : IFormat
    {
        public const ushort VERSION = 3;

        public string headerName;
        public string name;
        public uint headerSize;
        public ushort numFiles;
        public ushort numPointerFiles;
        public string headerName2;
        public List<byte[]> files;

        public Arc3() {
            files = new List<byte[]>();
        }
    }
}
