using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileFormat;

namespace Heracles.Lib.Formats
{
    public class Monsdata : IFormat
    {
        public uint numEntries;
        public List<byte[]> metadata;
        public List<string> names;

        public Monsdata() {
            metadata = new List<byte[]>();
            names = new List<string>();
        }
    }
}
