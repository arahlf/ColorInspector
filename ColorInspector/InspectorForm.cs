using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace ColorInspector
{
    public partial class InspectorForm : Form, MouseHookListener
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

            hook = new MouseHook(this);
        }

        public void onMouseMove(int x, int y)
        {
            this.lblMouseCoords.Text = "Mouse Location: " + x + ", " + y;

            if (scanning) {
                DateTime currentTime = DateTime.Now;

                if (currentTime < nextAllowableCaptureTime) {
                    return;
                }

                nextAllowableCaptureTime = currentTime.AddMilliseconds(SCAN_UPDATE_THROTTLE_MILLIS);

                UpdateImages(x, y);
                UpdateColorControls(bmpZoom.GetPixel(HALF - 1, HALF - 1)); // - 1 to avoid the pen line
            }
        }

        public void onMouseUp(int x, int y) {
            if (scanning) {
                scanning = false;           
            }
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

            bmpScanGraphics.Dispose();
            bmpZoomGraphics.Dispose();

            pnlScan.setBackgroundBitmap(bmpScan);
            pnlZoom.setBackgroundBitmap(bmpZoom);
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

        private void OnInspectMouseDown(object sender, MouseEventArgs e) {
            scanning = true;
        }

        private void OnColorClick(object sender, EventArgs e) {
            ColorInputDialog inputDialog = new ColorInputDialog();

            inputDialog.ShowDialog();

            pnlColor.BackColor = inputDialog.GetColor();
        }

        private const int SCAN_UPDATE_THROTTLE_MILLIS = 50;

        private readonly int SIZE;
        private readonly int QUAD_SIZE;
        private readonly int ZOOM_SIZE;
        private readonly int HALF;
        private readonly Rectangle ZOOM_SRC;
        private readonly Rectangle ZOOM_DST;

        private Bitmap bmpScan;
        private Bitmap bmpZoom;
        private MouseHook hook;
        private bool scanning;
        private DateTime nextAllowableCaptureTime = DateTime.Now;
    }
}
