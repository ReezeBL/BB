using System;
using System.IO;
using System.Text;
using BlessBuddy.Core.Engine;
using BlessBuddy.Core.Engine.Factories;
using Process.NET;
using Process.NET.Memory;

namespace BlessBuddy.Core
{
    //Default__BLNpcBase
    //Default__BLPlayer
    public static class BlessEngine
    {
        public static ProcessSharp Process { get; private set; }
        public static IMemory Memory { get; private set; }
        private static bool _processIsRunning;

        public static IntPtr BlessBase;
        private const int GNamesOffset = 0x43E19A8;
        private const int GObjectsOffset = 0x43E19F0;

        public static UArray<FNameEntry>GNames;
        public static UArray<UObject> GObjects; 

        public static uint FrameCount { get; private set; }

        public static void AttachToProcess(System.Diagnostics.Process nativeProcess)
        {
            Process = new ProcessSharp(nativeProcess, MemoryType.Remote);
            Memory = Process.Memory;
            _processIsRunning = true;
            Process.ProcessExited += delegate { _processIsRunning = false; };

            BlessBase = Process.ModuleFactory.MainModule.BaseAddress;
            GNames = new UArray<FNameEntry>(BlessBase + GNamesOffset, MemoryObjectFactory.CreateDeterminedObject<FNameEntry>);
            GObjects = new UArray<UObject>(BlessBase + GObjectsOffset, MemoryObjectFactory.CreateDeterminedObject<UObject>);
        }

        public static void Run()
        {
            //DumpNamesTable();
            DumpObjectsTable();
            Console.WriteLine("Dumping finished!");
            
            while (_processIsRunning)
            {
                FrameCount += 1;
                ObjectReverser.ReverseClass(GObjects[100147].Class);

                //var nameId = 8885;
                //var objs =
                //GObjects.Where(obj => obj.IsValid && obj.Class.IsValid && obj.Class.NameId == nameId)
                //    .ToArray();

                //foreach (var obj in objs)
                //{
                //    Console.WriteLine($"{obj.Name, -35}\t{obj.BaseAddress.ToInt64(),-8:X}");
                //}

                //Thread.Sleep(1000);
                break;
            }
        }

        private static void DumpObjectsTable()
        {
            Console.WriteLine($"ObjectsTable contains {GObjects.ElementsCount} objects");
            var sb = new StringBuilder();
            for(int i =0;i < GObjects.ElementsCount; i++)
            {
                var obj = GObjects[i];
                if (!obj.IsValid)
                    continue;
               sb.AppendLine($"GObject[{i:000000}]\t{obj.Name, -35}\t{obj.BaseAddress.ToInt64():X}");


            }
            File.WriteAllText("ObjectsDump.txt", sb.ToString());
        }

        private static void DumpNamesTable(bool writeToFile = false)
        {
            Console.WriteLine($"NameTable contains: {GNames.ElementsCount} elements");
            var sb = new StringBuilder();
            for(int i = 0;i < GNames.ElementsCount; i++)
            {
                var nameEntry = GNames[i];
                if (nameEntry.IsValid)
                {
                    FNameEntry.Table[nameEntry.Name] = i;
                    if(writeToFile)
                        sb.AppendLine($"Name[{i:000000}]\t{nameEntry.Name}");
                }
            }
            if(writeToFile)
                File.WriteAllText("NamesDump.txt", sb.ToString());
        }
    }
}
