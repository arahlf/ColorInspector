using System;

namespace ColorInspector
{
    class MouseMoveHook : IDisposable
    {
        public MouseMoveHook(IMouseMoveListener mouseMovelistener) {
            listener = mouseMovelistener;
            hook = Hooking.CreateLowLevelMouseHook(listener);
        }

        public void Dispose() {
            User32.UnhookWindowsHookEx(hook);
            hook = IntPtr.Zero;
        }

        private IntPtr hook;
        private IMouseMoveListener listener;
    }
}
