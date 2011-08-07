using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ColorInspectorSpace
{
    /// <summary>
    /// wrapper for gdi32.dll function calls
    /// </summary>
    class Gdi32
    {
        public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter


        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjectSource, int nXSrc, int nYSrc, int dwRop);
        
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);


        public static Bitmap CaptureDesktopWindow(int x, int y, int size)
        {
            // get the handle to the desktop window
            IntPtr handle = User32.GetDesktopWindow();

            // get the handle to the device context of the window
            IntPtr hdcSrc = User32.GetWindowDC(handle);

            // create a device context that can be copied to
            IntPtr hdcDest = Gdi32.CreateCompatibleDC(hdcSrc);

            // get a handle to a bitmap that can be copied to
            IntPtr hBitmap = Gdi32.CreateCompatibleBitmap(hdcSrc, size, size);

            // select the bitmap into the new device context
            IntPtr hOld = Gdi32.SelectObject(hdcDest, hBitmap);

            // we want to offset the x/y position by half the size for the snapshot
            int xTo = x - size / 2;
            int yTo = y - size / 2;

            // copy the pixels from the source handle to the destination handle
            Gdi32.BitBlt(hdcDest, 0, 0, size, size, hdcSrc, xTo, yTo, Gdi32.SRCCOPY);

            // restore the old selection
            Gdi32.SelectObject(hdcDest, hOld);

            // clean up memory allocations
            Gdi32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);

            // create a managed bitmap from the unmanaged bitmap
            Bitmap bmp = Bitmap.FromHbitmap(hBitmap);

            // free the unmanaged bitmap handle
            Gdi32.DeleteObject(hBitmap);

            return bmp;
        }
    }
}
