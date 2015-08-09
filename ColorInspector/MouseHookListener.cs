using System;

namespace ColorInspector
{
    public interface MouseHookListener
    {
        void onMouseMove(int x, int y);

        void onMouseUp(int x, int y);
    }
}
