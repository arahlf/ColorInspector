using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ColorInspector
{
    /// <summary>
    /// Wrapper for user32.dll function calls.
    /// </summary>
    public class User32
    {
        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport(USER32)]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport(USER32)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport(USER32)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport(USER32)]
        public static extern int GetSystemMetrics(int nIndex);

        [DllImport(USER32)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport(USER32)]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport(USER32)]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

        private const string USER32 = "user32.dll";
    }
}
