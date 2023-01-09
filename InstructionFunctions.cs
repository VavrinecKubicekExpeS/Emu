using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emu
{
    internal static class InstructionFunctions
    {
        private static void RunWithFlags(Func<short> checkedAction, Func<short> uncheckedAction)
        {
            var overflow = false;
            try { checkedAction(); }
            catch(OverflowException) { overflow = true; }
            finally {
                AdjustZero(uncheckedAction());
                Computer.OverflowFlag = overflow;
            }
        }
        private static void AdjustZero(short result) =>
            Computer.ZeroFlag = result == 0;
        #region Instruction functions
        public static void NoOperation(short[] args) { }
        public static void Move(short[] args) =>
            Computer.Memory[(ushort)args[0]] = Computer.Memory[(ushort)args[1]];
        public static void Load(short[] args) =>
            Computer.Memory[(ushort)args[0]] = args[1];
        public static void Add(short[] args) =>
            RunWithFlags(() =>   checked(Computer.Memory[2] = (short)(Computer.Memory[0] + Computer.Memory[1])),
                         () => unchecked(Computer.Memory[2] = (short)(Computer.Memory[0] + Computer.Memory[1])));
        public static void AddImmediate(short[] args) =>
            RunWithFlags(() =>   checked(Computer.Memory[2] = (short)(Computer.Memory[0] + args[1])),
                         () => unchecked(Computer.Memory[2] = (short)(Computer.Memory[0] + args[1])));
        public static void Subtract(short[] args) =>
            RunWithFlags(() =>   checked(Computer.Memory[2] = (short)(Computer.Memory[0] - Computer.Memory[1])),
                         () => unchecked(Computer.Memory[2] = (short)(Computer.Memory[0] - Computer.Memory[1])));
        public static void SubtractImmediate(short[] args) =>
            RunWithFlags(() =>   checked(Computer.Memory[2] = (short)(Computer.Memory[0] - args[1])),
                         () => unchecked(Computer.Memory[2] = (short)(Computer.Memory[0] - args[1])));
        public static void Not(short[] args) =>
            RunWithFlags(() =>   checked(Computer.Memory[2] = (short)~Computer.memory[0]),
                         () => unchecked(Computer.Memory[2] = (short)~Computer.memory[0]));
        public static void NotImmediate(short[] args) =>
            RunWithFlags(() =>   checked(Computer.Memory[2] = (short)~args[0]),
                         () => unchecked(Computer.Memory[2] = (short)~args[0]));
        public static void Or(short[] args) =>
            RunWithFlags(() =>   checked(Computer.Memory[2] = (short)(Computer.Memory[0] | Computer.Memory[1])),
                         () => unchecked(Computer.Memory[2] = (short)(Computer.Memory[0] | Computer.Memory[1])));
        public static void OrImmediate(short[] args) =>
            RunWithFlags(() =>   checked(Computer.Memory[2] = (short)(Computer.Memory[0] | args[1])),
                         () => unchecked(Computer.Memory[2] = (short)(Computer.Memory[0] | args[1])));
        public static void And(short[] args) =>
            RunWithFlags(() =>   checked(Computer.Memory[2] = (short)(Computer.Memory[0] & Computer.Memory[1])),
                         () => unchecked(Computer.Memory[2] = (short)(Computer.Memory[0] & Computer.Memory[1])));
        public static void AndImmediate(short[] args) =>
            RunWithFlags(() =>   checked(Computer.Memory[2] = (short)(Computer.Memory[0] & args[1])),
                         () => unchecked(Computer.Memory[2] = (short)(Computer.Memory[0] & args[1])));
        public static void Xor(short[] args) =>
            RunWithFlags(() =>   checked(Computer.Memory[2] = (short)(Computer.Memory[0] ^ Computer.Memory[1])),
                         () => unchecked(Computer.Memory[2] = (short)(Computer.Memory[0] ^ Computer.Memory[1])));
        public static void XorImmediate(short[] args) =>
            RunWithFlags(() =>   checked(Computer.Memory[2] = (short)(Computer.Memory[0] ^ args[1])),
                         () => unchecked(Computer.Memory[2] = (short)(Computer.Memory[0] ^ args[1])));
        public static void ShiftLeft(short[] args) =>
            RunWithFlags(() =>   checked(Computer.Memory[2] = (short)(Computer.Memory[0] << 1)),
                         () => unchecked(Computer.Memory[2] = (short)(Computer.Memory[0] << 1)));
        public static void ShiftRight(short[] args) =>
            RunWithFlags(() =>   checked(Computer.Memory[2] = (short)(Computer.Memory[0] >> 1)),
                         () => unchecked(Computer.Memory[2] = (short)(Computer.Memory[0] >> 1)));
        public static void RotateLeft(short[] args) =>
            RunWithFlags(() =>   checked(Computer.Memory[2] = (short)((Computer.Memory[0] << 1) | Computer.Memory[0] >> 16)),
                         () => unchecked(Computer.Memory[2] = (short)((Computer.Memory[0] << 1) | Computer.Memory[0] >> 16)));
        public static void RotateRight(short[] args) =>
            RunWithFlags(() =>   checked(Computer.Memory[2] = (short)((Computer.Memory[0] >> 1) | Computer.Memory[0] << 16)),
                         () => unchecked(Computer.Memory[2] = (short)((Computer.Memory[0] >> 1) | Computer.Memory[0] << 16)));
        public static void IncrementX(short[] args) =>
            Computer.Memory[5]++;
        public static void DecrementX(short[] args) =>
            Computer.Memory[5]--;
        public static void Jump(short[] args) =>
            Computer.loader.instructionPointer = (ushort)args[0];
        public static void JumpIfOverflow(short[] args)
        {
            if (Computer.OverflowFlag)
            {
                Jump(args);
                Computer.OverflowFlag = false;
            }
        }
        public static void JumpIfZero(short[] args)
        {
            if (Computer.ZeroFlag)
            {
                Jump(args);
                Computer.ZeroFlag = false;
            }
        }
        public static void JumpIfNotOverflow(short[] args)
        {
            if(!Computer.OverflowFlag) Jump(args);
        }
        public static void JumpIfNotZero(short[] args)
        {
            if(!Computer.ZeroFlag) Jump(args);
        }
        public static void ResetOverflow(short[] args) =>
            Computer.OverflowFlag = false;
        public static void ResetZero(short[] args) =>
            Computer.ZeroFlag = false;
        #endregion
    }
}
