using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using CB.DIRegisterGenerator;
using CB.EmbeddedResourceHelper;
using System.Reflection;

namespace Tests;

public class DIRegisterGeneratorTests
{
    private Assembly currentAssembly;
    private const string EnvironmentRetrun = "\r";

    [SetUp]
    public void Setup()
    {
        currentAssembly = Assembly.GetExecutingAssembly();
    }

    [Test]
    public void TestForInterfacesWithoutAttributes()
    {
        var source = currentAssembly.ReadAllTextFromEmbeddedFile("SyntaxSource2ClassesAnd2Namespaces.txt");
        
        var syntaxTree = CSharpSyntaxTree.ParseText(source, path: "MyClass.cs", encoding: Encoding.UTF8);
        var compilation = CSharpCompilation.Create("TestFiles", new[] { syntaxTree });

        var generator = new DIRegisterGenerator();
        var driver = CSharpGeneratorDriver.Create(generator);
        var runedDriver = driver.RunGenerators(compilation);
        
        var expectedSource = currentAssembly.ReadAllTextFromEmbeddedFile("DependencyRegisterFileWith2Classes.expected.txt");
        expectedSource = expectedSource.Replace(EnvironmentRetrun, string.Empty);
        VerifyGeneratorDriverHelper.Verify(runedDriver, expectedSource);
    }

    [Test]
    public void TestForInterfacesWithIgnoreAttribute()
    {
        var source = currentAssembly.ReadAllTextFromEmbeddedFile("SyntaxSource2ClassesAnd3Namespaces.txt");
        var syntaxTree = CSharpSyntaxTree.ParseText(source, path: "MyClass.cs", encoding: Encoding.UTF8);
        var compilation = CSharpCompilation.Create("TestFiles", new[] { syntaxTree });

        var generator = new DIRegisterGenerator();
        var driver = CSharpGeneratorDriver.Create(generator);
        var runedDriver = driver.RunGenerators(compilation);
        
        var expectedSource = currentAssembly.ReadAllTextFromEmbeddedFile("DependencyRegisterFileWith1Class.expected.txt");
        expectedSource = expectedSource.Replace(EnvironmentRetrun, string.Empty);
        VerifyGeneratorDriverHelper.Verify(runedDriver, expectedSource);
    }
}