using Yarhl.IO;
using Yarhl.Media.Text;
using TextReader = Yarhl.IO.TextReader;

namespace Heracles.Lib.Utils
{
    public class HeraclesReplacer
    {
        private static string tablePath = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "/../../../../Heracles.Lib/Resources/table.tbl");
        private static Replacer replacer;

        public static string Decode(string s) {
            if (replacer == null)
                InitializeReplacer();

            if (string.IsNullOrEmpty(s))
                return "<!empty>";
            else
                return replacer.TransformForward(s);
        }

        public static string Encode(string s) {
            if (replacer == null)
                InitializeReplacer();

            if (s == "<!empty>")
                return string.Empty;
            else
                return replacer.TransformBackward(s);
        }

        private static void InitializeReplacer() {
            replacer = new Replacer();
            ReadTable();
        }

        private static void ReadTable() {
            string line;
            string[] chars;
            var reader = new TextReader(DataStreamFactory.FromFile(tablePath, FileOpenMode.Read));

            while (!reader.Stream.EndOfStream) {
                line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                if (line[0] == '#' || char.IsWhiteSpace(line[0]))
                    continue;

                chars = line.Split('=');
                replacer.Add(chars[0], chars[1]);
            }
        }
    }
}
