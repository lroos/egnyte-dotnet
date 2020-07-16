using System.Collections.Generic;

namespace Egnyte.Api.Metadata
{
    public class NamespaceValues
    {
        public EgnyteWorkspace[] Results { get; set; }
    }

    public class EgnyteWorkspace
    {
        private Dictionary<string, string> Workspace { get; set; }
    }
}