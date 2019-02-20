using System;
using System.Collections.Generic;
using System.Text;

namespace Egnyte.CoreApi.Metadata
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
