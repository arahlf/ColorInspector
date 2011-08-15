using System;
using System.Runtime.InteropServices;

namespace ColorInspector
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MSLLHookStruct
    {
        public Point pt;
        public uint mouseData;
        public uint flags;
        public uint time;
        public IntPtr dwExtraInfo;
    }
}
