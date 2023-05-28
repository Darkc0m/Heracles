using Heracles.Lib.Converters;
using Heracles.Lib.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileSystem;
using Yarhl.IO;
using Yarhl.Media.Text;

namespace Heracles.CLI
{
    public static class ContainerCommands
    {
        public static void exportArc3(string srcPath, string dirPath) {
            Node n = NodeFactory.FromFile(srcPath);

            var conv = new Binary2Arc3(Path.GetFileNameWithoutExtension(srcPath));

            NodeContainerFormat container = n.TransformWith<BinaryFormat, Arc3>(conv)
                                                .TransformWith<Arc3ToContainer>()
                                                .GetFormatAs<NodeContainerFormat>();

            foreach(Node child in container.Root.Children) {
                child.Stream.WriteTo($"{dirPath}/{child.Name}");
            }
        }

        public static void importArc3(string srcPath, string dirPath) {
            Node n = NodeFactory.FromDirectory(srcPath);

            var arc = n.TransformWith<Arc3ToContainer>().GetFormatAs<Arc3>();

            new Binary2Arc3().Convert(arc).Stream.WriteTo($"{dirPath}/{arc.name}.arc");
        }
    }
}
