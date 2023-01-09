using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Emu
{
    public static class Computer
    {
        public static void Initialize(string initFolder)
        {
            JSONMemory jsonMemory;
            try
            {
                jsonMemory = JsonSerializer.Deserialize<JSONMemory>(File.ReadAllText(initFolder+"\\imem.json"));
            }
            catch(FileNotFoundException e)
            {
                throw new ArgumentException("Initializer folder not found", e);
            }
            memory = new(jsonMemory);
            loader = new();

            Port.Initialize(initFolder);
        }
        public static void Step()
        {
            loader.Load().Run();
        }
        internal static bool OverflowFlag { get; set; } = false;
        internal static bool ZeroFlag { get; set; } = false;
        public static InstructionLoader loader;
        internal static Memory memory;
        public static Memory Memory
        {
            get => memory;
        }
        public static Dictionary<ushort, short> InitializedMemory
        {
            get => memory.memory;
        }
    }
}
