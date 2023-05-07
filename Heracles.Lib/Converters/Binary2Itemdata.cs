using Heracles.Lib.Formats;
using Heracles.Lib.Utils;
using Yarhl.FileFormat;
using Yarhl.IO;

namespace Heracles.Lib.Converters
{
    public class Binary2Itemdata : IConverter<BinaryFormat, Itemdata>, IConverter<Itemdata, BinaryFormat>
    {
        public Itemdata Convert(BinaryFormat binary) {
            var reader = new HeraclesReader(binary.Stream);
            var item = new Itemdata();

            item.numItems = reader.ReadUInt32();
            item.textSize = reader.ReadUInt32();
            for (int i = 0; i < item.numItems * 2; i++) {
                item.text.Add(reader.ReadPointer16String(sizeof(uint) * 2 + item.numItems * sizeof(ushort) * 2));
            }

            return item;
        }

        public BinaryFormat Convert(Itemdata item) {
            var bin = new BinaryFormat();
            var writer = new HeraclesWriter(bin.Stream);

            writer.Write(item.numItems);
            writer.Stream.PushCurrentPosition();
            writer.Write(0x00);
            writer.WriteTextPointers16(item.text, 0);
            uint textStart = (uint)writer.Stream.Position;
            writer.WriteTextList(item.text);
            writer.Stream.PopPosition();
            writer.Write((uint)writer.Stream.Length - textStart);

            return bin;
        }
    }
}
