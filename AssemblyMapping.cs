using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emu
{
    internal class AssemblyMapping
    {
        public ushort[] Addresses { get; set; }
        public Type Distributor { get; set; }
    }
}
