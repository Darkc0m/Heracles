using Heracles.Lib.Formats;
using Heracles.Lib.Utils;
using System.Text;
using Yarhl.FileFormat;
using Yarhl.IO;

namespace Heracles.Lib.Converters
{
    public class Binary2Scedic : IConverter<BinaryFormat, Scedic>, IConverter<Scedic, BinaryFormat>
    {
        public Scedic Convert(BinaryFormat binary) {
            var reader = new HeraclesReader(binary.Stream);
            var sce = new Scedic();

            sce.numEntries = reader.ReadUInt32() + 1;
            for (int i = 0; i < sce.numEntries; i++) {
                sce.text.Add(reader.ReadPointer32String());
            }

            return sce;
        }

        public BinaryFormat Convert(Scedic sce) {
            var bin = new BinaryFormat();
            var writer = new HeraclesWriter(bin.Stream);

            writer.Write(sce.numEntries - 1);
            writer.WriteTextPointers32(sce.text, (uint)(writer.Stream.Position + sce.numEntries * 4));
            writer.WriteTextList(sce.text);

            return bin;
        }
    }
}
