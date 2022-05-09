using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DecompiledAssemblySourcesTool
{
    interface ClassLikeNode
    {
        SyntaxToken Identifier { get; }
        IList<string> ModifierStrings { get; }
    }

    public class ClassNode : ClassLikeNode
    {
        public ClassDeclarationSyntax _classDecl;
        public IList<string> ModifierStrings => _classDecl.Modifiers.Select(modifier => modifier.Text).ToList();
        public ClassNode(ClassDeclarationSyntax classDecl) { _classDecl = classDecl; }
        public SyntaxToken Identifier => _classDecl.Identifier;
    }

    public class InterfaceNode : ClassLikeNode
    {
        public InterfaceDeclarationSyntax _interfaceDecl;
        public IList<string> ModifierStrings => _interfaceDecl.Modifiers.Select(modifier => modifier.Text).ToList();
        public InterfaceNode(InterfaceDeclarationSyntax interfaceDecl) {  _interfaceDecl = interfaceDecl; }
        public SyntaxToken Identifier => _interfaceDecl.Identifier;
    }

    public class StructNode : ClassLikeNode
    {
        public StructDeclarationSyntax _structDecl;
        public IList<string> ModifierStrings => _structDecl.Modifiers.Select(modifier => modifier.Text).ToList();
        public StructNode(StructDeclarationSyntax structDecl) { _structDecl = structDecl; }
        public SyntaxToken Identifier => _structDecl.Identifier;
    }

    public class EnumNode : ClassLikeNode
    {
        public EnumDeclarationSyntax _enumDecl;
        public IList<string> ModifierStrings => _enumDecl.Modifiers.Select(modifier => modifier.Text).ToList();
        public EnumNode(EnumDeclarationSyntax enumDecl) { _enumDecl = enumDecl; }
        public SyntaxToken Identifier => _enumDecl.Identifier;
    }
}
