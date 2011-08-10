using System;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ColorInspector
{
    /// <summary>
    /// provides an api for installing a low level mouse hook to monitor mouse activity
    /// </summary>
    public class Hooking
    {
        // delegates are used as an alternative to function pointers
        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        public delegate void MouseHandler(int x, int y);

        private static Rectangle screenRect = User32.GetScreenRectangle(); // width/height of the user's desktop
        private static IntPtr hook = IntPtr.Zero;                          // handle to the hook
        private static HookProc hookProc;                                  // callback function for the hook
        private static MouseHandler mouseHandler;                          // user specified callback function
        private const int WH_MOUSE_LL = 14;                                // specifies a low level mouse hook, as defined in WinUser.h


        // installs a low level mouse hook to monitor mouse movement
        public static void CreateLowLevelMouseHook(MouseHandler mh)
        {
            hookProc = HookCallback;
            mouseHandler = mh;

            hook = User32.SetWindowsHookEx(WH_MOUSE_LL, hookProc, Kernel32.GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);
        }

        // hook callback function
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            // check if the message can be processed and if the mouse event was a movement
            if (nCode >= 0 && MouseMessages.WM_MOUSEMOVE == (MouseMessages) wParam)
            {
                // convert the unmanged pointer to a managed instance of the hook structure
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT) Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));

                ConstrainToDesktop(ref hookStruct.pt);

                // call the user-specified function with the x/y coordinates of the event
                mouseHandler(hookStruct.pt.x, hookStruct.pt.y);
            }

            // pass the hook information on to the next in the chain
            return User32.CallNextHookEx(hook, nCode, wParam, lParam);
        }


        // remove the hook procedure
        public static bool DestroyHook()
        {
            return User32.UnhookWindowsHookEx(hook);
        }


        /// <summary>
        /// ensures that the x,y coordinates returned by the hook do not exceed that of the actual desktop
        /// </summary>
        /// <param name="p">reference to the point structure passed into the hook callback</param>
        private static void ConstrainToDesktop(ref POINT p)
        {
            // validate x coordinate
            if (p.x < 0) {
                p.x = 0;
            }

            if (p.x > screenRect.Width - 1) {
                p.x = screenRect.Width - 1;
            }

            // validate y coordinate
            if (p.y < 0) {
                p.y = 0;
            }

            if (p.y > screenRect.Height - 1) {
                p.y = screenRect.Height - 1;
            }
        }


        public enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
    }
}