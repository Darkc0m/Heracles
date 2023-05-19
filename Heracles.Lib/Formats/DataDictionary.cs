using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileFormat;

namespace Heracles.Lib.Formats
{
    public class DataDictionary : IFormat
    {
        public uint numEntries;
        public List<string> text;

        public DataDictionary() {
            text = new List<string>();
        }
    }
}
