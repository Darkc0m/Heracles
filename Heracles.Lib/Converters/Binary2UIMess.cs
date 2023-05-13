using Heracles.Lib.Formats;
using Heracles.Lib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileFormat;
using Yarhl.IO;

namespace Heracles.Lib.Converters
{
    public class Binary2UIMess : IConverter<BinaryFormat, UIMess>, IConverter<UIMess, BinaryFormat>
    {
        public UIMess Convert(BinaryFormat bin) {
            var reader = new HeraclesReader(bin.Stream);
            var ui = new UIMess();

            ushort offset;
            ushort textStart = 0;
            for(int i = 0; i < 16; i++) {
                offset = reader.ReadUInt16();
                textStart = (ushort)(textStart == 0 ? offset * 2 : textStart);
                if (offset != 0) {
                    reader.Stream.PushToPosition(offset * 2);
                    ui.idText.Add(i, reader.ReadString());
                    reader.Stream.PopPosition();
                }
            }
            ui.dataOffset1 = reader.ReadUInt16();
            ui.dataOffset2 = reader.ReadUInt16();
            ui.dataOffset3 = reader.ReadUInt16();

            int j = 16;
            while(reader.Stream.Position < ui.dataOffset1 * 2) {
                offset = reader.ReadUInt16();
                if (offset != 0) {
                    reader.Stream.PushToPosition(offset * 2);
                    ui.idText.Add(j, reader.ReadString());
                    reader.Stream.PopPosition();
                }
                j++;
            }
            ui.textStart = textStart;
            ui.code = reader.ReadBytes((int)(textStart - reader.Stream.Position));

            return ui;
        }

        public BinaryFormat Convert(UIMess ui) {
            var bin = new BinaryFormat();
            var writer = new HeraclesWriter(bin.Stream);

            ushort offset = (ushort)(ui.textStart / 2);
            for(int i = 0; i < 16; i++) {
                if (ui.idText.ContainsKey(i)) {
                    writer.Write(offset);
                    offset += (ushort)(writer.GetByteCount(ui.idText[i]) / 2 + 1);
                }
                else {
                    writer.Write((ushort)0);
                }
            }
            writer.Write(ui.dataOffset1);
            writer.Write(ui.dataOffset2);
            writer.Write(ui.dataOffset3);

            int j = 16;
            while(writer.Stream.Position < ui.dataOffset1 * 2) {
                if (ui.idText.ContainsKey(j)) {
                    writer.Write(offset);
                    offset += (ushort)(writer.GetByteCount(ui.idText[j]) / 2 + 1);
                }
                else {
                    writer.Write((ushort)0);
                }
                j++;
            }

            writer.Write(ui.code);
            foreach(KeyValuePair<int, string> kv in ui.idText) {
                writer.Write(kv.Value);
                writer.WritePadding(0x00, 0x02);
            }

            return bin;
        }
    }
}
