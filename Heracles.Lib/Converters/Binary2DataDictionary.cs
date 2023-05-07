using Heracles.Lib.Formats;
using Heracles.Lib.Utils;
using Yarhl.FileFormat;
using Yarhl.IO;

namespace Heracles.Lib.Converters
{
    public class Binary2DataDictionary : IConverter<BinaryFormat, DataDictionary>, IConverter<DataDictionary, BinaryFormat>
    {
        public DataDictionary Convert(BinaryFormat bin) {
            var reader = new HeraclesReader(bin.Stream);
            var data = new DataDictionary();

            data.numEntries = reader.ReadUInt32();
            for (int i = 0; i < data.numEntries * 11 + 1; i++) {
                data.text.Add(reader.ReadPointer32String());
            }

            return data;
        }

        public BinaryFormat Convert(DataDictionary data) {
            var bin = new BinaryFormat();
            var writer = new HeraclesWriter(bin.Stream);

            writer.Write(data.numEntries);

            uint currentOffset = sizeof(uint) + sizeof(uint) * (data.numEntries * 11 + 1);
            for (int i = 0; i < data.numEntries; i++) {
                writer.Write(currentOffset);
                currentOffset += 0x3E;
                for (int j = 0; j < 10; j++) {
                    writer.Write(currentOffset);
                    currentOffset += (uint)writer.GetByteCount(data.text[i * 11 + j + 1]);
                }
            }
            writer.Write(currentOffset);
            int id;
            for (int i = 0; i < data.numEntries; i++) {
                id = i * 11;
                writer.Write(data.text[id], 0x3E);
                for (int j = 0; j < 10; j++) {
                    id = i * 11 + j + 1;
                    writer.Write(data.text[id]);
                }
            }
            writer.Write(data.text[^1]);
            writer.Write((byte)0x00);

            return bin;
        }
    }
}
