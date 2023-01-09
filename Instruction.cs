using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emu
{
    internal class Instruction
    {
        public InstructionType Type { get; }
        public short[] Args { get; set; }
        public Instruction(short[] args, InstructionType type)
        {
            Args = args;
            Type = type;
        }
        public void Run()
        {
            Type.Action(Args);
        }
    }
}
