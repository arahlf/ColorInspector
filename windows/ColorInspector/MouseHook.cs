using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace ColorInspector
{
    class MouseHook : IDisposable
    {
        public MouseHook(MouseHookListener listener) {
            _listener = listener;
            _hookProc = HookCallback; // if an instance-level delegate is not used, windows will silently remove the hook...

            using (Process currentProcess = Process.GetCurrentProcess())
            using (ProcessModule currentModule = currentProcess.MainModule) {
                _hook = User32.SetWindowsHookEx(WH_MOUSE_LL, _hookProc, Kernel32.GetModuleHandle(currentModule.ModuleName), 0);
            }
        }

        public void Dispose() {
            User32.UnhookWindowsHookEx(_hook);
            _hook = IntPtr.Zero;
        }

        public IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
            if (nCode < 0) {
                return User32.CallNextHookEx(_hook, nCode, wParam, lParam);
            }

            if (MouseMessages.WM_MOUSEMOVE == (MouseMessages)wParam) {
                MSLLHookStruct hookStruct = (MSLLHookStruct) Marshal.PtrToStructure(lParam, typeof(MSLLHookStruct));

                Utils.ConstrainToDesktop(ref hookStruct.pt);

                _listener.onMouseMove(hookStruct.pt.x, hookStruct.pt.y);
            }
            else if (MouseMessages.WM_LBUTTONUP == (MouseMessages)wParam) {
                MSLLHookStruct hookStruct = (MSLLHookStruct)Marshal.PtrToStructure(lParam, typeof(MSLLHookStruct));

                Utils.ConstrainToDesktop(ref hookStruct.pt);

                _listener.onMouseUp(hookStruct.pt.x, hookStruct.pt.y);
            }

            return User32.CallNextHookEx(_hook, nCode, wParam, lParam);
        }

        private const int WH_MOUSE_LL = 14; // specifies a low-level mouse hook

        private readonly MouseHookListener _listener;
        private User32.HookProc _hookProc;
        private IntPtr _hook;
    }
}
