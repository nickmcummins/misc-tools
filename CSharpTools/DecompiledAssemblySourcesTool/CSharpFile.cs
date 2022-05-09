using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace DecompiledAssemblySourcesTool
{
    public class CSharpFile
    {
        public string Filename { get; }
        public IEnumerable<string> Lines { get; }
        public SyntaxTree AST { get; }
        public IDictionary<SyntaxKind, IList<SyntaxNode>> Nodes { get; }
        public string Namespace => Nodes.TryGetValue(NamespaceDeclaration, out var namespaceDeclarations) ? (namespaceDeclarations.First() as BaseNamespaceDeclarationSyntax).Name.ToString() : Lines.First(line => line.StartsWith("namespace ")).Split(" ")[1];
        public IEnumerable<ClassDeclarationSyntax> Classes => Nodes.TryGetValue(ClassDeclaration, out var classes) ? classes.Select(c => c as ClassDeclarationSyntax).Where(c => !c.Modifiers.Any(m => m.Text == "private")) : Enumerable.Empty<ClassDeclarationSyntax>();
        public IEnumerable<InterfaceDeclarationSyntax> Interfaces => Nodes.TryGetValue(InterfaceDeclaration, out var interfaces) ? interfaces.Select(i => i as InterfaceDeclarationSyntax) : Enumerable.Empty<InterfaceDeclarationSyntax>();
        public IEnumerable<StructDeclarationSyntax> Structs => Nodes.TryGetValue(StructDeclaration, out var structs) ? structs.Select(i => i as StructDeclarationSyntax) : Enumerable.Empty<StructDeclarationSyntax>();
        public IEnumerable<EnumDeclarationSyntax> Enums => Nodes.TryGetValue(EnumDeclaration, out var enums) ? enums.Select(i => i as EnumDeclarationSyntax) : Enumerable.Empty<EnumDeclarationSyntax>();
        public IEnumerable<string> TopLevelIdentifiers => new List<string>().Add(Classes.Select(c => c.Identifier.Text)).Add(Interfaces.Select(i => i.Identifier.Text)).Add(Structs.Select(structNode => structNode.Identifier.Text)).Add(Enums.Select(enumNode => enumNode.Identifier.Text));

        public CSharpFile(string filename)
        {
            Filename = filename;
            Lines = File.ReadAllText(filename).Split("\r\n");
            AST = CSharpSyntaxTree.ParseText(File.ReadAllText(filename));
            Nodes = AST.GetRoot().Flatten(node => node.ChildNodes()).ToGroupedDictionary(node => node.Kind());

        }

        public override string ToString()
        {
            return $"csharpClass(filename={Filename},namespace={Namespace},classes={Classes.ToString(classNode => classNode.Identifier.Text)},interfaces={Interfaces.ToString(interfaceNode => interfaceNode.Identifier.Text)},structs={Structs.ToString(structNode => structNode.Identifier.Text)},enums={Enums.ToString(enumNode => enumNode.Identifier.Text)})";
        }
    }
}
