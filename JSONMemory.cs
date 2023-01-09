using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emu
{
    internal class JSONMemory
    {
        public Cell[] memory { get; set; }
    }

    public class Cell
    {
        public int address { get; set; }
        public int data { get; set; }
    }
}
