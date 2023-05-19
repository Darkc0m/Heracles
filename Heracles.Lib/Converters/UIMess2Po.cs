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
    public class UIMess2Po : IConverter<UIMess, Po>, IConverter<Po, UIMess>
    {
        public Po Convert(UIMess ui) {
            var po = HeraclesPo.generate();

            po.Header.Extensions.Add("DataOffset1", $"{ui.dataOffset1}");
            po.Header.Extensions.Add("DataOffset2", $"{ui.dataOffset2}");
            po.Header.Extensions.Add("DataOffset3", $"{ui.dataOffset3}");
            po.Header.Extensions.Add("TextStart", $"{ui.textStart}");
            po.Header.Extensions.Add("Code", $"{System.Convert.ToBase64String(ui.code)}");

            foreach(KeyValuePair<int, string> kv in ui.idText) {
                po.Add(new PoEntry(kv.Value) { Context = $"{kv.Key}" });
            }

            return po;
        }

        public UIMess Convert(Po po) {
            var ui = new UIMess();

            ui.dataOffset1 = ushort.Parse(po.Header.Extensions["DataOffset1"]);
            ui.dataOffset2 = ushort.Parse(po.Header.Extensions["DataOffset2"]);
            ui.dataOffset3 = ushort.Parse(po.Header.Extensions["DataOffset3"]);
            ui.textStart = ushort.Parse(po.Header.Extensions["TextStart"]);
            ui.code = System.Convert.FromBase64String(po.Header.Extensions["Code"]);

            foreach(PoEntry entry in po.Entries) {
                ui.idText.Add(int.Parse(entry.Context), entry.Text);
            }

            return ui;
        }
    }
}
