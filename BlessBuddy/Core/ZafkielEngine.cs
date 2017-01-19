using System;
using System.Linq;
using System.Text;
using System.Threading;
using Process.NET;
using Process.NET.Memory;
using Process.NET.Modules;

namespace BlessBuddy.Core
{
    public static class ZafkielEngine
    {
        private static ProcessSharp _process;
        private static IProcessModule _mainModule;
        private static bool _processIsRunning;


        public static uint FrameCount { get; private set; }

        public static void AttachToProcess(System.Diagnostics.Process nativeProcess)
        {
            _process = new ProcessSharp(nativeProcess, MemoryType.Remote);
            _processIsRunning = true;

            _process.ProcessExited += delegate { _processIsRunning = false; };

            _mainModule = _process.ModuleFactory.MainModule;
        }

        public static T Read<T>(int offset)
        {
            return _mainModule.Read<T>(offset);
        }

        public static void Run()
        {
            var addr = _mainModule.Read<IntPtr>(0x24528);
            addr = _process[addr].Read<IntPtr>(0);
            var @string = _process[addr].Read(0, Encoding.Unicode, 128);
            Console.WriteLine(@string);
            //Array.ForEach(memories, a => {Console.WriteLine($"{a.BaseAddress} {a.IsValid}");});
            while (_processIsRunning)
            {
                FrameCount += 1;
                

                Thread.Sleep(50);
            }
        }
    }
}
