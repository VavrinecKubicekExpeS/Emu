using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Emu
{
    internal delegate void InstructionAction(short[] args);
    public class InstructionLoader
    {
        private static InstructionType[] InstructionTypes
        {
            get => new InstructionType[]{
                    new (0x01,
                        new (InstructionFunctions.NoOperation),
                        0),
                    new (0x08,
                        new (InstructionFunctions.Move),
                        2),
                    new (0x09,
                        new (InstructionFunctions.Load),
                        2),
                    new (0x10,
                        new (InstructionFunctions.Add),
                        0),
                    new (0x50,
                        new (InstructionFunctions.AddImmediate),
                        1),
                    new (0x11,
                        new (InstructionFunctions.Subtract),
                        0),
                    new (0x51,
                        new (InstructionFunctions.SubtractImmediate),
                        1),
                    new (0x18,
                        new (InstructionFunctions.Not),
                        0),
                    new (0x58,
                        new (InstructionFunctions.NotImmediate),
                        1),
                    new (0x19,
                        new (InstructionFunctions.Or),
                        0),
                    new (0x59,
                        new (InstructionFunctions.OrImmediate),
                        1),
                    new (0x1a,
                        new (InstructionFunctions.And),
                        0),
                    new (0x5a,
                        new (InstructionFunctions.AndImmediate),
                        1),
                    new (0x1b,
                        new (InstructionFunctions.Xor),
                        0),
                    new (0x5b,
                        new (InstructionFunctions.XorImmediate),
                        1),
                    new (0x20,
                        new (InstructionFunctions.ShiftLeft),
                        0),
                    new (0x21,
                        new (InstructionFunctions.ShiftRight),
                        0),
                    new (0x22,
                        new (InstructionFunctions.RotateLeft),
                        0),
                    new (0x23,
                        new (InstructionFunctions.RotateRight),
                        0),
                    new(0x28, 
                        new(InstructionFunctions.IncrementX),
                        0),
                    new(0x29,
                        new(InstructionFunctions.DecrementX),
                        0),
                    new (0x30,
                        new (InstructionFunctions.Jump),
                        1),
                    new (0x31,
                        new (InstructionFunctions.JumpIfOverflow),
                        1),
                    new (0x32,
                        new (InstructionFunctions.JumpIfZero),
                        1),
                    new (0x33,
                        new (InstructionFunctions.JumpIfNotOverflow),
                        1),
                    new (0x34,
                        new (InstructionFunctions.JumpIfNotZero),
                        1),
                    new (0x38,
                        new (InstructionFunctions.ResetOverflow),
                        1),
                    new (0x39,
                        new (InstructionFunctions.ResetZero),
                        1)
                };
        }
        internal ushort instructionPointer;
        public ushort InstructionPointer
        {
            get => instructionPointer;
        }
        public InstructionLoader() =>
            instructionPointer = 6;
        internal Instruction Load()
        {
            short instructionCell = Computer.Memory[instructionPointer++];
            bool immediateArgument = (instructionCell & 0x8000) == 0x8000;
            var type = InstructionTypes.SingleOrDefault(type => type.Opcode == ((instructionCell >> 8) & 0x7f), null);
            if (type == null) return Load();
            if (type.ArgCount == 0) return new Instruction(Array.Empty<short>(), type);
            var args = new short[type.ArgCount];
            int i = 0;
            if (immediateArgument) args[i++] = (short)(instructionCell & 0xff);
            for (; i < type.ArgCount; i++) args[i] = Computer.Memory[instructionPointer++];
            return new Instruction(args, type);
        }
    }
}
