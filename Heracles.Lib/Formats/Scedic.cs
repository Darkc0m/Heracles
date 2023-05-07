using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heracles.Lib.Formats
{
    public class Scedic
    {
        public uint numEntries;
        public List<string> text;

        public Scedic() {
            text = new List<string>();
        }
    }
}
