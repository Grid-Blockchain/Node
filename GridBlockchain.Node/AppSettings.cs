using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GridBlockchain.Node
{
    public class AppSettings
    {
        public string ThisNodeName { get; set; }
        public List<string> KnownNodes  { get; set; }
    }
}
