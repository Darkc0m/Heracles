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
    public class Scedic2Po : IConverter<Scedic, Po>, IConverter<Po, Scedic>
    {
        public Po Convert(Scedic sce) {
            Po po = HeraclesPo.generate();

            for (int i = 0; i < sce.numEntries; i++) {
                po.Add(new PoEntry(sce.text[i]) { Context = i.ToString() });
            }

            return po;
        }

        public Scedic Convert(Po po) {
            var sce = new Scedic();

            sce.numEntries = (uint)po.Entries.Count;
            foreach (PoEntry entry in po.Entries) {
                sce.text.Add(entry.Text);
            }

            return sce;
        }
    }
}
