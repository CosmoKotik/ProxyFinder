using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyFinder.Geonode
{
    public class Geonode
    {
        public GeonodeData[] data { get; set; }
        public int total { get; set; }
        public int page { get; set; }
        public int limit { get; set; }
    }
}
