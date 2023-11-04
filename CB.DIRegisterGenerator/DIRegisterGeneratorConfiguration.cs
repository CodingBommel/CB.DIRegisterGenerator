using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace CB.DIRegisterGenerator
{
    public class DIRegisterGeneratorConfiguration
    {
        public DIRegisterGeneratorConfiguration(IConfiguration config)
        {
            var section = config.GetSection(nameof(DIRegisterGenerator));
            section.Bind(this);
        }

        public List<string> IgnoredInterfaces { get; set; }
    }
}