using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BlessBuddy.Core;
using Process.NET;
using Process.NET.Memory;
using Process.NET.Native;
using Process.NET.Native.Types;
using Process.NET.Windows;

namespace BlessBuddy
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            System.Diagnostics.Process selectedProcess = null;

            var processes = System.Diagnostics.Process.GetProcessesByName("Bless");
            if (processes.Length <= 1)
                selectedProcess = processes.FirstOrDefault();
            else
            {
                for (int i = 0; i < processes.Length; i++)
                {
                    Console.WriteLine($"{i + 1}) {processes[i].ProcessName} - {processes[i].Id}");
                }
                int input;
                if (int.TryParse(Console.ReadLine(), out input))
                    selectedProcess = processes[input];
                else
                    Console.WriteLine("Incorrect input value!");
            }

            if (selectedProcess != null)
            {
                BlessEngine.AttachToProcess(selectedProcess);
                BlessEngine.Run();
            }
            else
            {
                Console.WriteLine("No processes finded!");
            }
        }
    }

}
