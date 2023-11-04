using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CB.DIRegisterGenerator.Helper
{
    internal static class ResourceHelper
    {
        internal static string GetEmbeddedFile(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourcename = assembly.GetManifestResourceNames().FirstOrDefault(n => n.EndsWith(filename));
            
            if(resourcename == null)
                throw new Exception($"embeded file '{filename}' not found in assembly '{assembly.FullName}'");

            using( var resourceStream = assembly.GetManifestResourceStream(resourcename))
            using( var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                var template = reader.ReadToEnd();
                return template;
            }
        }
    }
}