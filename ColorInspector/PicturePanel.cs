using System;
using System.Drawing;
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

        public void setBackgroundBitmap(Bitmap backgroundBitmap) {
            _backgroundBitmap = backgroundBitmap;

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);

            if (_backgroundBitmap != null) {
                e.Graphics.DrawImage(_backgroundBitmap, 0, 0);

                int widthWithoutBorder = this.Width - 2;
                int half = widthWithoutBorder / 2;

                e.Graphics.DrawLine(Pens.Black, 0, half, widthWithoutBorder, half);
                e.Graphics.DrawLine(Pens.Black, half, 0, half, widthWithoutBorder);
            }
        }

        private Bitmap _backgroundBitmap;
    }
}
