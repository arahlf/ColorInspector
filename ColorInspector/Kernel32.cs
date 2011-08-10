using System;
using System.Runtime.InteropServices;

namespace ColorInspector
{
    /// <summary>
    /// wrapper for kernell32.dll function calls
    /// </summary>
    public class Kernel32
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll")]
        public static extern int GetLastError();
    }
}
