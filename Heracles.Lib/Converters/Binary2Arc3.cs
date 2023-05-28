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
    public class Binary2Arc3 : IConverter<BinaryFormat, Arc3>, IConverter<Arc3, BinaryFormat>
    {
        private string name;
        public Binary2Arc3(string name) {
            this.name = name;
        }
        public Binary2Arc3() { }
        public Arc3 Convert(BinaryFormat bin) {
            var reader = new HeraclesReader(bin.Stream);
            var arc = new Arc3();

            arc.name = name;
            arc.headerName = reader.ReadString(4);
            arc.headerSize = reader.ReadUInt32();
            if (reader.ReadUInt16() != Arc3.VERSION)
                throw new Exception("Wrong arc file version");
            arc.numFiles = reader.ReadUInt16();
            arc.numPointerFiles = reader.ReadUInt16();
            reader.SkipPadding(0x04);
            arc.headerName2 = reader.ReadString(16);

            for (int i = 0; i < arc.numPointerFiles; i++) {
                uint adddress = reader.ReadUInt32();
                uint size = reader.ReadUInt32();
                if(adddress < reader.Stream.Length) {
                    reader.Stream.PushToPosition(adddress);
                    arc.files.Add(reader.ReadBytes((int)size));
                    reader.Stream.PopPosition();
                }
                else {
                    arc.files.Add(new byte[0]);
                }
            }

            return arc;
        }

        public BinaryFormat Convert(Arc3 arc) {
            var bin = new BinaryFormat();
            var writer = new HeraclesWriter(bin.Stream);

            writer.Write(arc.headerName, 4, false);
            writer.Write(arc.headerSize);
            writer.Write(Arc3.VERSION);
            writer.Write(arc.numFiles);
            writer.Write(arc.numPointerFiles);
            writer.WritePadding(0x00, 0x04);
            writer.Write(arc.headerName2, 16, false);

            uint fileOffset = arc.headerSize;

            for(int i = 0; i < arc.numPointerFiles; i++) {
                if (arc.files[i].Length == 0) {
                    writer.Write(0);    //Offset
                    writer.Write(0);    //Size
                }
                else {
                    writer.Write(fileOffset);
                    writer.Write(arc.files[i].Length);
                    fileOffset += (uint)arc.files[i].Length;
                    if (fileOffset % 4 != 0)
                        fileOffset += 4 - (uint)(arc.files[i].Length % 4);
                }
            }
            for(int i = 0; i < arc.numPointerFiles; i++) {
                writer.Write(arc.files[i]);
                writer.WritePadding(0x00, 0x04);
            }

            return bin;
        }
    }
}
