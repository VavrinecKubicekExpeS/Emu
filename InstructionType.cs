using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emu
{
    internal class InstructionType
    {
        public int Opcode { get; private set; }
        public InstructionAction Action { get; private set; }
        public byte ArgCount { get; private set; }

        public InstructionType(int opcode, InstructionAction action, byte argCount)
        {
            Opcode = opcode;
            Action = action;
            ArgCount = argCount;
        }
    }
}
