using System.Xml;

namespace DecompiledAssemblySourcesTool
{
    public class Csproj
    {
        private readonly XmlDocument _xmlDoc;
        public string ProjectDir { get; }

        public IList<string> CompileIncludes { get; set; }

        public Csproj(string filename)
        {
            _xmlDoc = new XmlDocument();
            _xmlDoc.Load(filename);
            ProjectDir = filename.StartsWith(@"C:\") ? Path.GetDirectoryName(filename) : Environment.CurrentDirectory;
            var elements = _xmlDoc.DocumentElement.Flatten(element => element.ChildNodes.Select().Where(node => node.NodeType == XmlNodeType.Element).Select(node => (XmlElement)node));
            CompileIncludes = elements.Where(element => element.Name == "Compile")
                .Select(compile => compile.Attributes["Include"].Value)
                .Select(f => f.StartsWith("\\") ? $"C:{f}" : @$"{ProjectDir}\{f}")
                .ToList();
        }

        public override string ToString()
        {
            return $"csproj(compileIncludes.Count={CompileIncludes.Count})";
        }
    }
}
