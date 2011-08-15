using System;
using System.Runtime.InteropServices;

namespace ColorInspector
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public int x;
        public int y;
    }
}
