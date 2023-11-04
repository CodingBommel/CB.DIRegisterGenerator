using Microsoft.CodeAnalysis;
using NUnit.Framework;

public static class VerifyGeneratorDriverHelper
{
    public static void Verify(GeneratorDriver driver, string expectedSoruce)
    {
        var generatedResults = driver.GetRunResult().Results;
        Assert.That(generatedResults.Length, Is.EqualTo(1));
        foreach (var generatedResult in generatedResults)
        {
            Assert.That(generatedResult.Exception, Is.Null);
            Assert.That(generatedResult.GeneratedSources.Length, Is.EqualTo(1));
            foreach (var generatedSource in generatedResult.GeneratedSources)
            {
                Assert.That(generatedSource.SourceText.ToString(), Is.EqualTo(expectedSoruce));
            }
        }
    }
}