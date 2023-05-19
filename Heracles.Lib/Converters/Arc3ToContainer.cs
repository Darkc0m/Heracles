using Heracles.Lib.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileFormat;
using Yarhl.FileSystem;
using Yarhl.IO;
using TextReader = Yarhl.IO.TextReader;

namespace Heracles.Lib.Converters
{
    public class Arc3ToContainer : IConverter<Arc3, NodeContainerFormat>, IConverter<NodeContainerFormat, Arc3>
    {
        public NodeContainerFormat Convert(Arc3 arc) {
            var container = new NodeContainerFormat();
            var info = $"{arc.name}\n{arc.headerName}\n{arc.headerName2}\n{arc.numPointerFiles}\n";

            for (int i = 0; i < arc.numPointerFiles; i++) {
                if (arc.files[i].Length == 0) {
                    info += ",\n";
                }
                else {
                    info += $"file{i:000}.bin\n";
                    container.Root.Add(GenerateSingleNode($"file{i:0000}.bin", arc.files[i]));
                }
            }

            var fileInfo = NodeFactory.FromMemory("files.txt");
            var fileBytes = Encoding.UTF8.GetBytes(info);
            fileInfo.Stream.Write(fileBytes, 0, fileBytes.Length);
            container.Root.Add(fileInfo);

            return container;
        }

        public Arc3 Convert(NodeContainerFormat container) {
            var info = container.Root.Children["files.txt"];
            var arc = new Arc3();
            var reader = new TextReader(info.Stream) { NewLine = "\n" };
            reader.Stream.Position = 0x00;

            arc.name = reader.ReadLine();
            arc.headerName = reader.ReadLine();
            arc.headerName2 = reader.ReadLine();
            arc.numPointerFiles = ushort.Parse(reader.ReadLine());
            arc.headerSize = 0x20 + (uint)arc.numPointerFiles * 0x08;
            arc.numFiles = 0;

            for (int i = 0; i < arc.numPointerFiles; i++) {
                if (container.Root.Children[$"file{i:0000}.bin"] != null) {
                    var fileReader = new DataReader(container.Root.Children[$"file{i:0000}.bin"].Stream);
                    fileReader.Stream.Position = 0x00;
                    arc.files.Add(fileReader.ReadBytes((int)fileReader.Stream.Length));
                    arc.numFiles++;
                }
                else {
                    arc.files.Add(new byte[0]);
                }
            }

            return arc;
        }

        private Node GenerateSingleNode(string name, byte[] block) {
            Node child = NodeFactory.FromMemory(name);
            child.Stream.Write(block, 0, block.Length);
            return child;
        }
    }
}
