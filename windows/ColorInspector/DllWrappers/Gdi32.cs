using System;
using System.Runtime.InteropServices;

namespace ColorInspector
{
    /// <summary>
    /// Wrapper for gdi32.dll function calls.
    /// </summary>
    class Gdi32
    {
        [DllImport(GDI32)]
        public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjectSource, int nXSrc, int nYSrc, int dwRop);

        [DllImport(GDI32)]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);

        [DllImport(GDI32)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport(GDI32)]
        public static extern bool DeleteDC(IntPtr hDC);

        [DllImport(GDI32)]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport(GDI32)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        private const string GDI32 = "gdi32.dll";
    }
}