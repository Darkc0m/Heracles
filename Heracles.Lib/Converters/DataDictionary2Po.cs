using Heracles.Lib.Formats;
using Heracles.Lib.Utils;
using Yarhl.FileFormat;
using Yarhl.Media.Text;

namespace Heracles.Lib.Converters
{
    public class DataDictionary2Po : IConverter<DataDictionary, Po>, IConverter<Po, DataDictionary>
    {
        public Po Convert(DataDictionary data) {
            Po po = HeraclesPo.generate();

            for (int i = 0; i < data.text.Count; i++) {
                po.Add(new PoEntry(data.text[i]) { Context = i.ToString() });
            }

            return po;
        }

        public DataDictionary Convert(Po po) {
            var data = new DataDictionary();

            data.numEntries = (uint)((po.Entries.Count - 1) / 11);
            foreach (PoEntry entry in po.Entries) {
                data.text.Add(entry.Text);
            }

            return data;
        }
    }
}
