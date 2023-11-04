
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CB.DIRegisterGenerator
{
    internal static class SyntaxHelper
    {
        internal static IEnumerable<RegisterType> GetRegisterTypes<TDeclationSyntaxType>(IEnumerable<TypeDeclarationSyntax> interfaceTypeDeclarationSyntaxes)
            where TDeclationSyntaxType : TypeDeclarationSyntax
        {
            var interfaces = new List<RegisterType>();
            foreach (var typeDeclarationSyntax in interfaceTypeDeclarationSyntaxes)
            {
                var declaration = (TDeclationSyntaxType)typeDeclarationSyntax;
                var namespaceIdentifier = GetNamespaceIdentifier(declaration);

                var registerType = new RegisterType
                {
                    Name = typeDeclarationSyntax.Identifier.ValueText,
                    Namespace = namespaceIdentifier?.Identifier.ValueText ?? string.Empty,
                    BaseTypeName = declaration?.BaseList?.Types.FirstOrDefault()?.Type.ToString() ?? string.Empty
                };
                interfaces.Add(registerType);
            }

            return interfaces.ToArray();
        }

        internal static IdentifierNameSyntax GetNamespaceIdentifier<TDeclationSyntaxType>(TDeclationSyntaxType interfaceDeclaration) where TDeclationSyntaxType : TypeDeclarationSyntax
        {
            var fileScopedNamespaceDeclarationSyntax = interfaceDeclaration.Ancestors().OfType<FileScopedNamespaceDeclarationSyntax>().FirstOrDefault()?.Name;
            var namespaceDeclarationSyntax = interfaceDeclaration.Ancestors().OfType<NamespaceDeclarationSyntax>().FirstOrDefault()?.Name;
            
            if(fileScopedNamespaceDeclarationSyntax!= null && fileScopedNamespaceDeclarationSyntax is IdentifierNameSyntax)
                return (IdentifierNameSyntax)fileScopedNamespaceDeclarationSyntax;

            if(namespaceDeclarationSyntax!= null && namespaceDeclarationSyntax is IdentifierNameSyntax)
                return (IdentifierNameSyntax)namespaceDeclarationSyntax;
            
            return null;
        }
    }
}