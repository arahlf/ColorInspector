using System;
using System.Runtime.InteropServices;

namespace ColorInspector
{
    /// <summary>
    /// Wrapper for kernell32.dll function calls.
    /// </summary>
    public class Kernel32
    {
        [DllImport(KERNEL32)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport(KERNEL32)]
        public static extern int GetLastError();

        private const string KERNEL32 = "kernel32.dll";
    }
}
