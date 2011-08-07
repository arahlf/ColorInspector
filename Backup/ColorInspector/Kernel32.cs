using System;
using System.Runtime.InteropServices;

namespace ColorInspectorSpace
{
    /// <summary>
    /// wrapper for kernell32.dll function calls
    /// </summary>
    class Kernel32
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll")]
        public static extern int GetLastError();
    }
}
