using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Emu
{
    public class Memory
    {
        internal Memory(JSONMemory source)
        {
            foreach (var (address, data) in
                from cell 
                in source.memory select ((ushort)cell.address, (short)cell.data))
                    this[address] = data;
        }

        internal Dictionary<ushort, short> memory = new();
        public short this[ushort addr]
        {
            get =>
                addr < 0x8000 ?
                    memory.TryGetValue(addr, out var value)
                      ? value
                      : (short)new Random().Next(short.MaxValue)
                  : Port.Read(addr);
            internal set
            {
                if(addr >= 0x8000) Port.Write(addr, value);
                if(memory.ContainsKey(addr)) memory[addr] = value;
                else memory.Add(addr, value);
            }
        }
    }
}
