using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emu
{
    // Root object class
    public class RootObject
    {
        public Map[] map { get; set; }
        public string[] dependencies { get; set; }
    }

    // Map class
    public class Map
    {
        public int address { get; set; }
        public string path { get; set; }
    }
}
