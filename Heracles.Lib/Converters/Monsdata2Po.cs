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
    public class Monsdata2Po : IConverter<Monsdata, Po>, IConverter<Po, Monsdata>
    {
        public Po Convert(Monsdata mons) {
            var po = HeraclesPo.generate();

            for(int i = 0; i < mons.numEntries; i++) {
                po.Add(new PoEntry(mons.names[i])
                {
                    Context = $"{i}",
                    ExtractedComments = System.Convert.ToBase64String(mons.metadata[i])
                });
            }

            return po;
        }

        public Monsdata Convert(Po po) {
            var mons = new Monsdata();

            mons.numEntries = (uint)po.Entries.Count;
            foreach(PoEntry entry in po.Entries) {
                mons.names.Add(entry.Text);
                mons.metadata.Add(System.Convert.FromBase64String(entry.ExtractedComments));
            }

            return mons;
        }
    }
}
