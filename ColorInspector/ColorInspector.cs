using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace ColorInspectorSpace
{
    public partial class ColorInspector : Form
    {
        // 100% / 900% images
        Bitmap bmpScan;
        Bitmap bmpZoom;
        Pen p = new Pen(Color.Black); // pen used to draw the grid lines on the panels
        bool scanning = false; // is the user scanning the desktop


        // form constructor
        public ColorInspector()
        {
            InitializeComponent();

            // install the hook
            Hooking.CreateLowLevelMouseHook(mouseMoved);
        }


        // update the 900% view
        private void drawImages()
        {
            // if the bmps have not yet been initialized, do not proceed
            // happens if user mouses over the zoom panel before actually inspecting
            if (bmpScan == null) { 
                return; 
            }

            bmpZoom = new Bitmap(81, 81);

            Graphics pnlScanGraphics = this.pnlScan.CreateGraphics();
            Graphics pnlZoomGraphics = this.pnlZoom.CreateGraphics();
            Graphics bmpScanGraphics = Graphics.FromImage(bmpScan);
            Graphics bmpZoomGraphics = Graphics.FromImage(bmpZoom);

            // set properties to create the pixelated effect
            pnlZoomGraphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            pnlZoomGraphics.PixelOffsetMode = PixelOffsetMode.Half;

            // rectangles of the source / destination to draw
            Rectangle srcRect = new Rectangle(36, 36, 9, 9);
            Rectangle dstRect = new Rectangle(0, 0, 81, 81);

            bmpZoomGraphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            bmpZoomGraphics.PixelOffsetMode = PixelOffsetMode.Half;

            // draw the scan bitmap
            pnlScanGraphics.DrawImage(bmpScan, 0, 0);

            // draw the zoomed bitmap, then draw it onto the form
            bmpZoomGraphics.DrawImage(bmpScan, dstRect, srcRect, GraphicsUnit.Pixel);
            pnlZoomGraphics.DrawImage(bmpZoom, 0, 0);

            // draw the crosses to divide each panel into 4 quadrants
            pnlScanGraphics.DrawLine(p, 0, 40, 81, 40);
            pnlScanGraphics.DrawLine(p, 40, 0, 40, 81);
            pnlZoomGraphics.DrawLine(p, 0, 41, 81, 41);
            pnlZoomGraphics.DrawLine(p, 41, 0, 41, 81);

            // free resources
            bmpScanGraphics.Dispose();
            bmpZoomGraphics.Dispose();
            pnlScanGraphics.Dispose();
            pnlZoomGraphics.Dispose();
        }


        // called by the hook procedure that was installed in the constructor
        private void mouseMoved(int x, int y)
        {
            if (scanning)
            {
                // get the 81x81 bitmap under the mouse
                bmpScan = Gdi32.CaptureDesktopWindow(x, y, 81);

                updateColorControls(bmpScan.GetPixel(40, 40));

                // draw the 900% view
                drawImages();
            }

            // update mouse coordinates
            this.lblMouseCoords.Text = "Mouse Location: " + x + ", " + y;
        }


        // update the color translations when the 900% view is moused over
        private void pnlZoom_MouseDown(object sender, MouseEventArgs e)
        {
            if (bmpZoom == null) { return; }

            int xTile = (int)Math.Floor((double)e.X / 9);
            int yTile = (int)Math.Floor((double)e.Y / 9);

            updateColorControls(bmpZoom.GetPixel(xTile * 9 + 5, yTile * 9 + 5));
        }


        // updates the rgb/hex values
        private void updateColorControls(Color c)
        {
            this.txtColorHex.Text = ColorTranslator.ToHtml(c).ToLower();
            this.txtRGB.Text = c.R + ", " + c.G + ", " + c.B;
            this.pnlColor.BackColor = c;
        }


        #region Event Handlers

        private void btnInspect_MouseUp(object sender, MouseEventArgs e)
        {
            scanning = false;
        }


        private void btnInspect_MouseDown(object sender, MouseEventArgs e)
        {
            scanning = true;
        }

        private void pnlPaint(object sender, PaintEventArgs e)
        {
            drawImages();
        }

        #endregion

        private void ColorInspector_Paint(object sender, PaintEventArgs e)
        {
            drawImages();
        }

    }
}
