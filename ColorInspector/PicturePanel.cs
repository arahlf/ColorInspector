using System;
using System.Windows.Forms;

namespace ColorInspectorSpace
{
    /// <summary>
    /// class with a panel used to draw images on
    /// </summary>
    public class PicturePanel : Panel
    {
        public PicturePanel()
        {
            // styles to optimize drawing on a panel
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }
    }
}
