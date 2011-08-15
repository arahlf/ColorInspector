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

        private static IntPtr hook = IntPtr.Zero;                          // handle to the hook
        private static HookProc hookProc;                                  // callback function for the hook
        private static IMouseMoveListener mouseHandler;                          // user specified callback function
        private const int WH_MOUSE_LL = 14;                                // specifies a low level mouse hook, as defined in WinUser.h


        // installs a low level mouse hook to monitor mouse movement
        public static IntPtr CreateLowLevelMouseHook(IMouseMoveListener mouseCallback)
        {
            hookProc = HookCallback;
            mouseHandler = mouseCallback;

            hook = User32.SetWindowsHookEx(WH_MOUSE_LL, hookProc, Kernel32.GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);

            return hook;
        }

        // hook callback function
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            // check if the message can be processed and if the mouse event was a movement
            if (nCode >= 0 && MouseMessages.WM_MOUSEMOVE == (MouseMessages) wParam)
            {
                // convert the unmanged pointer to a managed instance of the hook structure
                MSLLHookStruct hookStruct = (MSLLHookStruct) Marshal.PtrToStructure(lParam, typeof(MSLLHookStruct));

                Utils.ConstrainToDesktop(ref hookStruct.pt);

                // call the user-specified function with the x/y coordinates of the event
                mouseHandler.OnMouseMove(hookStruct.pt.x, hookStruct.pt.y);
            }

            // pass the hook information on to the next in the chain
            return User32.CallNextHookEx(hook, nCode, wParam, lParam);
        }


        // remove the hook procedure
        public static bool DestroyHook()
        {
            return User32.UnhookWindowsHookEx(hook);
        }
    }
}