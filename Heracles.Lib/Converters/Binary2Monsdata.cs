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
    public class Binary2Monsdata : IConverter<BinaryFormat, Monsdata>, IConverter<Monsdata, BinaryFormat>
    {
        public Monsdata Convert(BinaryFormat bin) {
            var reader = new HeraclesReader(bin.Stream);
            var mons = new Monsdata();

            mons.numEntries = reader.ReadUInt32();
            reader.SkipPadding(0x08);

            for(int i = 0; i < mons.numEntries; i++) {
                mons.metadata.Add(reader.ReadBytes(0x124));
                if(!reader.Stream.EndOfStream) {
                    mons.names.Add(reader.ReadString(0x18));
                }
            }
            mons.names.Add("<empty!>");

            return mons;
        }

        public BinaryFormat Convert(Monsdata mons) {
            var bin = new BinaryFormat();
            var writer = new HeraclesWriter(bin.Stream);

            writer.Write(mons.numEntries);
            writer.WritePadding(0x00, 0x08);

            for(int i = 0; i < mons.numEntries; i++) {
                writer.Write(mons.metadata[i]);
                if(i + 1 != mons.numEntries) { 
                    writer.Write(mons.names[i], 0x18, false);
                }
            }

            return bin;
        }
    }
}
