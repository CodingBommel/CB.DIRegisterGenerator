using System.Collections.Immutable;
using System.Linq;
using CB.DIRegisterGenerator.Helper;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CB.DIRegisterGenerator
{
    [Generator(LanguageNames.CSharp)]
    public sealed partial class DIRegisterGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var typeDeclarationSyntaxProvider  = context.SyntaxProvider.CreateSyntaxProvider(
                predicate: (s, _) => s is TypeDeclarationSyntax,
                transform: (ctx, _) => (TypeDeclarationSyntax)ctx.Node);

            var compilationAndClasses = context.CompilationProvider.Combine(typeDeclarationSyntaxProvider.Collect());

            context.RegisterSourceOutput(compilationAndClasses, (sourceProductionContext, compilationAndClass) => 
            {
                Execute(compilationAndClass.Left, compilationAndClass.Right, sourceProductionContext);
            });
        }

        private void Execute(Compilation compilation,
            ImmutableArray<TypeDeclarationSyntax> typeDeclarationSyntaxProvider,
            SourceProductionContext sourceProductionContext)
        {
            var interfaceTypeDeclarationSyntaxes = typeDeclarationSyntaxProvider
                            .Where(typeDeclaration => typeDeclaration.Kind() == SyntaxKind.InterfaceDeclaration)
                            .Where(typeDeclaration => !typeDeclaration.AttributeLists
                                .Any(att => !att.Attributes
                                    .Any(a => ((IdentifierNameSyntax)a.Name)?.Identifier.ValueText == nameof(IgnoreForDIRegisterAttribute))));

            var interfaces = SyntaxHelper.GetRegisterTypes<InterfaceDeclarationSyntax>(interfaceTypeDeclarationSyntaxes);

            var typeDeclarationSyntax = typeDeclarationSyntaxProvider
                    .Where(td => td.Kind() == SyntaxKind.ClassDeclaration)
                    .Where(td => td.BaseList?.Types != null &&
                                 td.BaseList.Types.Any(type => 
                                    interfaces.Any(@interface => @interface.Name == type?.Type?.ToString())));

            var typesForInterfaces = SyntaxHelper.GetRegisterTypes<TypeDeclarationSyntax>(typeDeclarationSyntax);

            var template = ResourceHelper.GetEmbeddedFile("DependecyRegisterTemplateFile.txt");
            template = TemplateHelper.FillTemplate(interfaces, typesForInterfaces, template);
            sourceProductionContext.AddSource($"DependencyInjection.g.cs", template);
        }
        
    }

    public struct RegisterType
    {
        public string Name { get; set; }
        public string Namespace { get; set; }

        public string BaseTypeName { get; set; }
    }
}