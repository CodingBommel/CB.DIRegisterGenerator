
using System.Collections.Generic;
using System.Linq;

namespace CB.DIRegisterGenerator.Helper
{
    internal static class TemplateHelper
    {
        private const string EnvironmentNewLine = "\r\n";
        private const string TemplateReplaceStringForUsings = "@@@Usings@@@";
        private const string TemplateReplaceStringForRegisterTypes = "@@@Registers@@@";


        internal static string FillTemplate(IEnumerable<RegisterType> interfaces, IEnumerable<RegisterType> typesForInterfaces, string template)
        {
            var registerInterfaceUsings = new List<string>();
            registerInterfaceUsings.AddRange(interfaces.Where(t => !string.IsNullOrEmpty(t.Namespace)).Select(t => $@"using {t.Namespace};"));
            registerInterfaceUsings.AddRange(typesForInterfaces.Where(t => !string.IsNullOrEmpty(t.Namespace)).Select(t => $@"using {t.Namespace};"));

            var registerStrings = typesForInterfaces.Select(t => $@"services.AddTransient<{t.BaseTypeName}, {t.Name}>();");

            var usingsString = string.Join(EnvironmentNewLine, registerInterfaceUsings.Distinct());
            var registerString = string.Join(EnvironmentNewLine + "            ", registerStrings.Distinct());

            template = template.Replace(TemplateReplaceStringForUsings, usingsString);
            template = template.Replace(TemplateReplaceStringForRegisterTypes, registerString);
            template = template.Replace("\n", EnvironmentNewLine);
            template = template.Replace("\r\r", "\r");
            
            return template;
        }
    }
}