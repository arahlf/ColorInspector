using System;
using System.Windows.Forms;

namespace ColorInspector
{
    /// <summary>
    /// A Panel that is optimized for drawing on.
    /// </summary>
    public class DrawingPanel : Panel
    {
        public DrawingPanel()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }
    }
}
