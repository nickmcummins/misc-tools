using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static DecompiledAssemblySourcesTool.Extensions;

namespace DecompiledAssemblySourcesTool
{
    public interface ClassLikeNode
    {
        SyntaxToken Identifier { get; }
        IList<string> ModifierStrings { get; }
    }

    public class ClassNode : ClassLikeNode
    {
        public ClassDeclarationSyntax _classDecl;
        public IList<string> ModifierStrings => _classDecl.Modifiers.Select(modifier => modifier.Text).ToList();
        public SyntaxToken Identifier => _classDecl.Identifier;
        public ClassNode(ClassDeclarationSyntax classDecl) { _classDecl = classDecl; }
        public override string ToString() => $"{string.Join(Space, ModifierStrings)} class {Identifier.Text}";
    }

    public class InterfaceNode : ClassLikeNode
    {
        public InterfaceDeclarationSyntax _interfaceDecl;
        public IList<string> ModifierStrings => _interfaceDecl.Modifiers.Select(modifier => modifier.Text).ToList();
        public SyntaxToken Identifier => _interfaceDecl.Identifier;
        public InterfaceNode(InterfaceDeclarationSyntax interfaceDecl) {  _interfaceDecl = interfaceDecl; }
        public override string ToString() => $"{string.Join(Space, ModifierStrings)} interface {Identifier.Text}";
    }

    public class StructNode : ClassLikeNode
    {
        public StructDeclarationSyntax _structDecl;
        public IList<string> ModifierStrings => _structDecl.Modifiers.Select(modifier => modifier.Text).ToList();
        public SyntaxToken Identifier => _structDecl.Identifier;
        public StructNode(StructDeclarationSyntax structDecl) { _structDecl = structDecl; }
        public override string ToString() => $"{string.Join(Space, ModifierStrings)} struct {Identifier.Text}";
    }

    public class EnumNode : ClassLikeNode
    {
        public EnumDeclarationSyntax _enumDecl;
        public IList<string> ModifierStrings => _enumDecl.Modifiers.Select(modifier => modifier.Text).ToList();
        public SyntaxToken Identifier => _enumDecl.Identifier;
        public EnumNode(EnumDeclarationSyntax enumDecl) { _enumDecl = enumDecl; }
        public override string ToString() => $"{string.Join(Space, ModifierStrings)} enum {Identifier.Text}";
    }
}
