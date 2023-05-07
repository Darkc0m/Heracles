using Yarhl.FileFormat;

namespace Heracles.Lib.Formats
{
    public class Itemdata : IFormat
    {
        public uint numItems;
        public uint textSize;
        public List<string> text;

        public Itemdata() {
            text = new List<string>();
        }
    }
}
