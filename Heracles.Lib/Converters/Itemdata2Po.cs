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
    public class Itemdata2Po : IConverter<Itemdata, Po>, IConverter<Po, Itemdata>
    {
        public Po Convert(Itemdata item) {
            Po po = HeraclesPo.generate();

            for (int i = 0; i < item.text.Count; i++) {
                po.Add(new PoEntry(item.text[i]) { Context = i.ToString() });
            }

            return po;
        }

        public Itemdata Convert(Po po) {
            var item = new Itemdata();

            item.numItems = (uint)(po.Entries.Count / 2);
            foreach (PoEntry entry in po.Entries)
                item.text.Add(entry.Text);

            return item;
        }
    }
}
