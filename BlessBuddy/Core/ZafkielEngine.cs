using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using BlessBuddy.Core.Engine;
using Process.NET;
using Process.NET.Memory;
using Process.NET.Modules;

namespace BlessBuddy.Core
{
    public static class ZafkielEngine
    {
        public static ProcessSharp Memory { get; private set; }
        private static IProcessModule _mainModule;
        private static bool _processIsRunning;

        private const int GNames = 0x43E19A8;
        private const int GObjects = 0x43E19F0;
        private static readonly Dictionary<int, string> namesTable = new Dictionary<int, string>();

        public static uint FrameCount { get; private set; }

        public static void AttachToProcess(System.Diagnostics.Process nativeProcess)
        {
            Memory = new ProcessSharp(nativeProcess, MemoryType.Remote);
            _processIsRunning = true;

            Memory.ProcessExited += delegate { _processIsRunning = false; };

            _mainModule = Memory.ModuleFactory.MainModule;
        }

        public static T Read<T>(int offset)
        {
            return _mainModule.Read<T>(offset);
        }

        public static void Run()
        {
            DumpNamesTable();
            DumpObjectsTable();
            Console.WriteLine("Dumping finished!");
            
            while (_processIsRunning)
            {
                FrameCount += 1;

                Thread.Sleep(50);
            }
        }

        private static void DumpObjectsTable()
        {
            var array = _mainModule.Read<UArray>(GObjects);
            Console.WriteLine($"ObjectsTable contains {array.ElementsCount} objects");
            var sb = new StringBuilder();
            for (int i = 0; i < array.ElementsCount; i++)
            {
                var adr = Memory[array.ArrayPtr].Read<IntPtr>(i * 8);
                if (adr == IntPtr.Zero)
                    continue;
                var nameOffset = Memory[adr].Read<int>(0x48);
                sb.AppendLine($"GObject [{adr.ToInt64():X}]\t{namesTable[nameOffset]}\t{i.ToString("000000")}");
            }
            File.WriteAllText("ObjectsDump.txt", sb.ToString());
        }

        private static void DumpNamesTable(bool writeToFile = false)
        {
            var nameTable = _mainModule.Read<UArray>(GNames);
            Console.WriteLine($"NameTable contains: {nameTable.ElementsCount} elements");
            var sb = new StringBuilder();
            for (int i = 0; i < nameTable.ElementsCount; i++)
            {
                var addr = Memory[nameTable.ArrayPtr].Read<IntPtr>(i * 8);
                if (addr != IntPtr.Zero)
                {
                    var name = Memory[addr].Read(0x14, Encoding.Default, 100);
                    namesTable[i] = name;
                    if(writeToFile)
                        sb.AppendLine($"Name [{i.ToString("000000")}] {name}");
                }
            }
            if(writeToFile)
                File.WriteAllText("NamesDump.txt", sb.ToString());
        }
    }
}
