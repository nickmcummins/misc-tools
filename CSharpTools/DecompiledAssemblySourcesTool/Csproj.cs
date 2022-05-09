using System.Xml;
using System.Xml.XPath;

namespace DecompiledAssemblySourcesTool
{
    public class Csproj
    {
        private readonly XmlDocument _xmlDoc;

        public IList<string> CompileIncludes { get; set; }

        public Csproj(string filename)
        {
            _xmlDoc = new XmlDocument();
            _xmlDoc.Load(filename);
            var elements = _xmlDoc.DocumentElement.Flatten(element => element.ChildNodes.Select().Where(node => node.NodeType == XmlNodeType.Element).Select(node => (XmlElement)node));
            CompileIncludes = elements.Where(element => element.Name == "Compile")
                .Select(compile => compile.Attributes["Include"].Value)
                .Select(filename => filename.StartsWith("\\") ? $"C:{filename}" : filename)
                .ToList();
        }

        public override string ToString()
        {
            return $"csproj(compileIncludes.Count={CompileIncludes.Count})";
        }
    }
}
