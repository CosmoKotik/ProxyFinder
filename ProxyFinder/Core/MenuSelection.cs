using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ProxyFinder.Core
{
    public class MenuSelection
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public Vector2 Position { get; set; }
        public string Description { get; set; }
        public string Destination { get; set; }
        public bool CanClick { get; set; }
        public bool isDescription { get; set; }
        public bool isDestination { get; set; }
        public Menu SelectMenu { get; set; }
    }
}
