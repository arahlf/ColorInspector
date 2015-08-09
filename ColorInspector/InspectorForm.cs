using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace ColorInspector
{
    public partial class InspectorForm : Form, IMouseMoveListener
    {
        public InspectorForm(int size, int quadrantSize)
        {
            InitializeComponent();

            SIZE = size;
            QUAD_SIZE = quadrantSize;
            ZOOM_SIZE = QUAD_SIZE * 2 + 1;
            HALF = (size - 1) / 2;

            ZOOM_SRC = new Rectangle(HALF - QUAD_SIZE, HALF - QUAD_SIZE, ZOOM_SIZE, ZOOM_SIZE);
            ZOOM_DST = new Rectangle(0, 0, SIZE, SIZE);

            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(btnInspect, "Press and drag to begin inspecting.");
            toolTip.SetToolTip(pnlZoom, "Click a color to select it.");
            toolTip.SetToolTip(pnlColor, "Click to change colors.");

            hook = new MouseMoveHook(this);
        }

        public void OnMouseMove(int x, int y)
        {
            if (scanning) {
                UpdateImages(x, y);
                UpdateColorControls(bmpZoom.GetPixel(HALF - 1, HALF - 1)); // - 1 to avoid the pen line
                
                DrawImages();
            }

            this.lblMouseCoords.Text = "Mouse Location: " + x + ", " + y;
        }

        private void UpdateImages(int x, int y) {
            if (bmpScan != null) {
                bmpScan.Dispose();
            }
            if (bmpZoom != null) {
                bmpZoom.Dispose();
            }

            bmpScan = Utils.CaptureDesktopWindow(x, y, SIZE, SIZE);
            bmpZoom = new Bitmap(SIZE, SIZE);

            Graphics bmpScanGraphics = Graphics.FromImage(bmpScan);
            Graphics bmpZoomGraphics = Graphics.FromImage(bmpZoom);

            // set properties to create the pixelated effect
            bmpZoomGraphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            bmpZoomGraphics.PixelOffsetMode = PixelOffsetMode.Half;

            // create the zoomed image by transferring part of the scan
            bmpZoomGraphics.DrawImage(bmpScan, ZOOM_DST, ZOOM_SRC, GraphicsUnit.Pixel);

            // draw the crosses to divide each panel into 4 quadrants
            bmpScanGraphics.DrawLine(Pens.Black, 0, HALF, SIZE, HALF);
            bmpScanGraphics.DrawLine(Pens.Black, HALF, 0, HALF, SIZE);
            bmpZoomGraphics.DrawLine(Pens.Black, 0, HALF + 1, SIZE, HALF + 1); // + 1's needed for the pixeloffsetmode
            bmpZoomGraphics.DrawLine(Pens.Black, HALF + 1, 0, HALF + 1, SIZE);

            bmpScanGraphics.Dispose();
            bmpZoomGraphics.Dispose();
        }

        private void DrawImages() {
            if (bmpScan != null && bmpZoom != null) {
                Graphics pnlScanGraphics = this.pnlScan.CreateGraphics();
                Graphics pnlZoomGraphics = this.pnlZoom.CreateGraphics();

                pnlScanGraphics.DrawImage(bmpScan, 0, 0);
                pnlZoomGraphics.DrawImage(bmpZoom, 0, 0);

                pnlScanGraphics.Dispose();
                pnlZoomGraphics.Dispose();
            }
        }

        private void OnZoomClick(object sender, MouseEventArgs e) {
            if (bmpZoom != null) {
                int xTile = (int) Math.Floor((double)(e.X / ZOOM_SIZE));
                int yTile = (int) Math.Floor((double)(e.Y / ZOOM_SIZE));

                UpdateColorControls(bmpZoom.GetPixel(xTile * ZOOM_SIZE, yTile * ZOOM_SIZE));
            }
        }

        private void UpdateColorControls(Color color) {
            this.txtColorHex.Text = ColorTranslator.ToHtml(color).ToLower();
            this.txtRGB.Text = color.R + ", " + color.G + ", " + color.B;
            this.pnlColor.BackColor = color;
        }

        private void OnInspectMouseUp(object sender, MouseEventArgs e) {
            scanning = false;
        }

        private void OnInspectMouseDown(object sender, MouseEventArgs e) {
            scanning = true;
        }

        private void OnColorClick(object sender, EventArgs e) {
            ColorInputDialog inputDialog = new ColorInputDialog();

            inputDialog.ShowDialog();

            pnlColor.BackColor = inputDialog.GetColor();
        }

        private readonly int SIZE;
        private readonly int QUAD_SIZE;
        private readonly int ZOOM_SIZE;
        private readonly int HALF;
        private readonly Rectangle ZOOM_SRC;
        private readonly Rectangle ZOOM_DST;

        private Bitmap bmpScan;
        private Bitmap bmpZoom;
        private MouseMoveHook hook;
        private bool scanning;
    }
}
