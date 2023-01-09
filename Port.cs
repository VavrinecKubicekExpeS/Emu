using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text.Json;

namespace Emu
{
    internal class Port
    {
        private static AssemblyMapping[] mappings;

        internal static void Initialize(string initFolder)
        {
            // Read the JSON file
            string json = File.ReadAllText(initFolder + "\\ext.json");

            // Deserialize the JSON into a root object
            RootObject root = JsonSerializer.Deserialize<RootObject>(json);

            // Load the DLL dependencies
            foreach (string dll in root.dependencies)
            {
                Assembly.LoadFrom(dll);
            }

            // Create an array of AddressMapping objects
            AddressMapping[] addressMappings = new AddressMapping[root.map.Length];

            // Iterate through the array of map objects
            for (int i = 0; i < root.map.Length; i++)
            {
                // Get the map object
                Map map = root.map[i];

                // Get the assembly from the path
                Assembly assembly = Assembly.LoadFile(map.path);
                Type[] types = assembly.GetTypes();

                // Get the MemoryDistributor type from the assembly
                Type distributorType = types.Single((type) => type.Name == "MemoryDistributor");

                // Create a new AddressMapping object
                addressMappings[i] = new AddressMapping
                {
                    Address = (ushort)map.address,
                    Distributor = distributorType
                };
            }

            // Group the AddressMapping objects by their Distributor
            mappings = addressMappings.GroupBy(m => m.Distributor)
                .Select(g => new AssemblyMapping
                {
                    Addresses = g.Select(m => m.Address).ToArray(),
                    Distributor = g.Key
                }).ToArray();

            // Invoke the Initialize method of each Distributor type
            foreach (AssemblyMapping mapping in mappings)
            {
                mapping.Distributor.GetMethod("Initialize", BindingFlags.Static | BindingFlags.Public).
                    Invoke(null, new object[] { mapping.Addresses });
            }
        }

        
        internal static short Read(ushort index)
        {
            // Find the AssemblyMapping that contains the index
            AssemblyMapping mapping = mappings.FirstOrDefault(m => m.Addresses.Contains(index));
            if (mapping == null)
            {
                // Index not found
                throw new IndexOutOfRangeException();
            }

            // Invoke the static Read method of the Distributor type
            return (short)mapping.Distributor.InvokeMember("Read",
                BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public,
                null, null, new object[] { index });
        }
        internal static void Write(ushort index, short value)
        {
            // Find the AssemblyMapping that contains the index
            AssemblyMapping mapping = mappings.FirstOrDefault(m => m.Addresses.Contains(index));
            if (mapping == null)
            {
                // Index not found
                throw new IndexOutOfRangeException();
            }

            // Invoke the static Write method of the Distributor type
            mapping.Distributor.InvokeMember("Write",
                BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public,
                null, null, new object[] { index, value });
        }

    }
}
