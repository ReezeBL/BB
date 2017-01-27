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
    public static class BlessEngine
    {
        public static ProcessSharp Memory { get; private set; }
        private static IProcessModule _mainModule;
        private static bool _processIsRunning;

        private const int GNamesOffset = 0x43E19A8;
        private const int GObjectsOffset = 0x43E19F0;

        private static readonly Dictionary<int, string> NamesTable = new Dictionary<int, string>();

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
            var objectsArray = _mainModule.Read<UArray>(GObjectsOffset);
            Console.WriteLine($"ObjectsTable contains {objectsArray.ElementsCount} objects");
            var sb = new StringBuilder();
            for (int i = 0; i < objectsArray.ElementsCount; i++)
            {
                var objectPtr = Memory[objectsArray.ArrayPtr].Read<IntPtr>(i * 8);
                if (objectPtr == IntPtr.Zero)
                    continue;
                var nameOffset = Memory[objectPtr].Read<int>(0x48);
                sb.AppendLine($"GObject[{objectPtr.ToInt64():X}]\t{NamesTable[nameOffset]}\t{i.ToString("000000")}");
            }
            File.WriteAllText("ObjectsDump.txt", sb.ToString());
        }

        private static void DumpNamesTable(bool writeToFile = false)
        {
            var namesArray = _mainModule.Read<UArray>(GNamesOffset);
            Console.WriteLine($"NameTable contains: {namesArray.ElementsCount} elements");
            var sb = new StringBuilder();
            for (int i = 0; i < namesArray.ElementsCount; i++)
            {
                var fnamePtr = Memory[namesArray.ArrayPtr].Read<IntPtr>(i * 8);
                if (fnamePtr != IntPtr.Zero)
                {
                    var name = Memory[fnamePtr].Read(0x14, Encoding.Default, 100);
                    NamesTable[i] = name;
                    if(writeToFile)
                        sb.AppendLine($"Name[{i.ToString("000000")}]\t{name}");
                }
            }
            if(writeToFile)
                File.WriteAllText("NamesDump.txt", sb.ToString());
        }
    }
}
