using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ColorInspector
{
    public interface IMouseMoveListener
    {
        void OnMouseMove(int x, int y);
    }
}
