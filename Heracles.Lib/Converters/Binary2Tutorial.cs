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
    public class Binary2Tutorial : IConverter<BinaryFormat, Tutorial>, IConverter<Tutorial, BinaryFormat>
    {
        public Tutorial Convert(BinaryFormat bin) {
            var reader = new HeraclesReader(bin.Stream);
            var tut = new Tutorial();
            
            ushort value;
            ushort lowestOffset = (ushort)bin.Stream.Length;
            ushort highestOffset = 0;
            do {
                string s = "";
                var metadata = new List<byte[]>();
                do {
                    value = reader.ReadUInt16();
                    metadata.Add(BitConverter.GetBytes(value));
                } while (value != 0x1100 && value != 0xFFFF);

                byte[] fullData = new byte[metadata.Count * 2];
                for (int i = 0; i < metadata.Count; i++) {
                    fullData[i * 2] = metadata[i][0];
                    fullData[i * 2 + 1] = metadata[i][1];
                }
                
                if(value != 0xFFFF) {
                    tut.metadata.Add(fullData);
                    ushort offset = (ushort)(reader.ReadUInt16() * 2);
                    lowestOffset = offset < lowestOffset ? offset : lowestOffset;
                    reader.Stream.PushToPosition(offset);
                    tut.text.Add(reader.ReadString());
                    highestOffset = (ushort)(reader.Stream.Position > highestOffset ? reader.Stream.Position : highestOffset);
                    reader.Stream.PopPosition();
                }
                else {
                    tut.lastMetadata = fullData;
                }
            } while (value != 0xFFFF);

            tut.textStart = lowestOffset;
            tut.code = reader.ReadBytes((int)(lowestOffset - reader.Stream.Position));
            reader.Stream.Position = highestOffset;
            reader.SkipPadding(0x02);
            tut.trailingText = reader.ReadBytes((int)(reader.Stream.Length - reader.Stream.Position));

            return tut;
        }

        public BinaryFormat Convert(Tutorial tut) {
            var bin = new BinaryFormat();
            var writer = new HeraclesWriter(bin.Stream);
            ushort offset = (ushort)(tut.textStart / 2);
            
            for(int i = 0; i < tut.text.Count; i++) {
                writer.Write(tut.metadata[i]);
                writer.Write(offset);
                offset += (ushort)(writer.GetByteCount(tut.text[i]) / 2 + 1);
            }
            writer.Write(tut.lastMetadata);
            writer.Write(tut.code);
            for(int i = 0; i < tut.text.Count; i++) {
                writer.Write(tut.text[i]);
                writer.WritePadding(0x00, 0x02);
            }
            writer.Write(tut.trailingText);

            return bin;
        }
    }
}
