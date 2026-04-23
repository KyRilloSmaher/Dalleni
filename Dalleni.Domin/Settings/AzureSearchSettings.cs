using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Domin.Settings
{
    public class AzureSearchSettings
    {
        public string ApiKey { get; set; } = default!;
        public string IndexName { get; set; } = default!;
        public string EndPoint { get; set; } = default!;
        public string SemanticConfigName { get; set; } = default!;
    }
}
