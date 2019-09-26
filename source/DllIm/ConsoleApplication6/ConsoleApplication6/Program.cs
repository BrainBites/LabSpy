using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
namespace ConsoleApplication3
{
    class Program
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetActiveWindow();
        public static void Main(string [] args)
        {
            Process[] processlist = Process.GetProcesses();
            int n=0;
            while (n < 8)
            {

                foreach (Process p in processlist)
                {
                    if (p.MainWindowHandle != IntPtr.Zero)
                    {
                        if (p.MainWindowHandle == GetActiveWindow())
                        {
                            Console.WriteLine(DateTime.Now + "   " + p.MainWindowTitle);
                        }
                    }
                    
                    n++;
                }
            }
            Console.ReadKey();
 
        }
    }
}