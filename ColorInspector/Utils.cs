using System;
using System.Drawing;

namespace ColorInspector
{
    public class Utils
    {
        public static Bitmap CaptureDesktopWindow(int x, int y, int width, int height) {
            // get the handle to the desktop window
            IntPtr handle = User32.GetDesktopWindow();

            // get the handle to the device context of the window
            IntPtr hdcSrc = User32.GetWindowDC(handle);

            // create a device context that can be copied to
            IntPtr hdcDest = Gdi32.CreateCompatibleDC(hdcSrc);

            // get a handle to a bitmap that can be copied to
            IntPtr hBitmap = Gdi32.CreateCompatibleBitmap(hdcSrc, width, height);

            // select the bitmap into the new device context
            IntPtr hOld = Gdi32.SelectObject(hdcDest, hBitmap);

            // we want to offset the x/y position by half the size for the snapshot
            int xTo = x - width / 2;
            int yTo = y - height / 2;

            // copy the pixels from the source handle to the destination handle
            Gdi32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, xTo, yTo, SRCCOPY);

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

        public static Rectangle GetScreenRectangle() {
            return new Rectangle(0, 0, User32.GetSystemMetrics(SM_CXVIRTUALSCREEN), User32.GetSystemMetrics(SM_CYVIRTUALSCREEN));
        }

        private const int SM_CXVIRTUALSCREEN = 78;
        private const int SM_CYVIRTUALSCREEN = 79;
        private const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter
    }
}
