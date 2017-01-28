using System;
using System.Linq;
using System.Threading;
using BlessBuddy.Core;

namespace BlessBuddy
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            System.Diagnostics.Process selectedProcess = null;

            while (true)
            {
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
                    Thread.Sleep(2000);
                    BlessEngine.AttachToProcess(selectedProcess);
                    Console.WriteLine("Succesfully attached!");
                    try
                    {
                        BlessEngine.Run();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;
                }

                Thread.Sleep(100);
            }
        }
    }

}
