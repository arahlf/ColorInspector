using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace ColorInspector
{
    class MouseMoveHook : IDisposable
    {
        public MouseMoveHook(IMouseMoveListener mouseMovelistener) {
            listener = mouseMovelistener;
            hookProc = HookCallback; // if an instance-level delegate is not used, windows will silently remove the hook...

            using (Process currentProcess = Process.GetCurrentProcess())
            using (ProcessModule currentModule = currentProcess.MainModule) {
                hook = User32.SetWindowsHookEx(WH_MOUSE_LL, hookProc, Kernel32.GetModuleHandle(currentModule.ModuleName), 0);
            }
        }

        public void Dispose() {
            User32.UnhookWindowsHookEx(hook);
            hook = IntPtr.Zero;
        }

        public IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
            if (nCode >= 0 && MouseMessages.WM_MOUSEMOVE == (MouseMessages)wParam) {
                MSLLHookStruct hookStruct = (MSLLHookStruct) Marshal.PtrToStructure(lParam, typeof(MSLLHookStruct));

                Utils.ConstrainToDesktop(ref hookStruct.pt);

                listener.OnMouseMove(hookStruct.pt.x, hookStruct.pt.y);
            }

            return User32.CallNextHookEx(hook, nCode, wParam, lParam);
        }

        private const int WH_MOUSE_LL = 14; // specifies a low-level mouse hook

        private User32.HookProc hookProc;
        private IntPtr hook;
        private IMouseMoveListener listener;
    }
}
