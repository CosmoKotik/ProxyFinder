using ProxyFinder.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyFinder.Core
{
    public class Proxy
    {
        public string IP { get; set; }
        public int Port { get; set; }
        public Proxies ProxyType { get; set; }
        public ProxySites Sites { get; set; }
    }
}
