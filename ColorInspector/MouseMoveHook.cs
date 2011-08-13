using System;

namespace ColorInspector
{
    class MouseMoveHook : IDisposable
    {
        public MouseMoveHook(IMouseMoveListener mouseMovelistener) {
            listener = mouseMovelistener;
            hook = Hooking.CreateLowLevelMouseHook(listener);
        }

        public void Resume() {
            if (!IsListening()) {
                hook = Hooking.CreateLowLevelMouseHook(listener);
            }
        }

        public void Suspend() {
            if (IsListening()) {
                User32.UnhookWindowsHookEx(hook);
                hook = IntPtr.Zero;
            }
        }

        public bool IsListening() {
            return hook != IntPtr.Zero;
        }

        public void Dispose() {
            Suspend(); // rename
            hook = IntPtr.Zero;
        }

        private void CreateHook() {

        }

        private void RemoveHook() {

        }

        private IntPtr hook;
        private IMouseMoveListener listener;
    }
}
