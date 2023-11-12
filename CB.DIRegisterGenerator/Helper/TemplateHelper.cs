
using System.Collections.Generic;
using System.Linq;

namespace CB.DIRegisterGenerator.Helper
{
    internal static class TemplateHelper
    {
        private const string EnvironmentReturnNewLine = "\n";
        private const string UsingTemplateString = "using {0};";
        private const string RegisterTemplateString = "            services.AddTransient<{0}, {1}>();";

        internal static string FillTemplate(IEnumerable<RegisterType> interfaces, IEnumerable<RegisterType> typesForInterfaces, string template)
        {
            var usingNamespaces = interfaces.ToList();
            usingNamespaces.AddRange(typesForInterfaces);

            var registerInterfaceUsings = usingNamespaces
                .Where(t => !string.IsNullOrEmpty(t.Namespace))
                .Select(t => string.Format(UsingTemplateString, t.Namespace));
            
            var registerStrings = typesForInterfaces.Select(t => string.Format(RegisterTemplateString, t.BaseTypeName, t.Name));

            var usingsString = string.Join(EnvironmentReturnNewLine, registerInterfaceUsings.Distinct());
            var registerString = string.Join(EnvironmentReturnNewLine, registerStrings.Distinct());

            var filledTemplate = string.Format(template, usingsString, registerString);

            return filledTemplate;
        }
    }
}