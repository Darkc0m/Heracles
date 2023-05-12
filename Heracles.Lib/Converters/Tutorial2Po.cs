using Heracles.Lib.Formats;
using Heracles.Lib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileFormat;
using Yarhl.Media.Text;

namespace Heracles.Lib.Converters
{
    public class Tutorial2Po : IConverter<Tutorial, Po>, IConverter<Po, Tutorial>
    {
        public Po Convert(Tutorial tut) {
            var po = HeraclesPo.generate();

            po.Header.Extensions.Add("LastMetadata", System.Convert.ToBase64String(tut.lastMetadata));
            po.Header.Extensions.Add("Code", System.Convert.ToBase64String(tut.code));
            po.Header.Extensions.Add("TrailingText", System.Convert.ToBase64String(tut.trailingText));
            po.Header.Extensions.Add("TextStart", $"{tut.textStart}");

            for (int i = 0; i < tut.text.Count; i++) {
                po.Add(new PoEntry(tut.text[i])
                {
                    Context = $"{i}",
                    ExtractedComments = System.Convert.ToBase64String(tut.metadata[i])
                });
            }

            return po;
        }

        public Tutorial Convert(Po po) {
            var tut = new Tutorial();

            tut.lastMetadata = System.Convert.FromBase64String(po.Header.Extensions["LastMetadata"]);
            tut.code = System.Convert.FromBase64String(po.Header.Extensions["Code"]);
            tut.trailingText = System.Convert.FromBase64String(po.Header.Extensions["TrailingText"]);
            tut.textStart = int.Parse(po.Header.Extensions["TextStart"]);

            for(int i = 0; i < po.Entries.Count; i++) {
                tut.text.Add(po.Entries[i].Text);
                tut.metadata.Add(System.Convert.FromBase64String(po.Entries[i].ExtractedComments));
            }

            return tut;
        }
    }
}
