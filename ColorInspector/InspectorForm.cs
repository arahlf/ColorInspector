using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace ColorInspector
{
    public partial class InspectorForm : Form, IMouseMoveListener
    {
        public InspectorForm()
        {
            InitializeComponent();

            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(btnInspect, "Press and drag to begin inspecting.");
            toolTip.SetToolTip(pnlZoom, "Click a color to select it.");
            toolTip.SetToolTip(pnlColor, "Click to change colors.");

            hook = new MouseMoveHook(this);
        }

        private void updateImages(int x, int y) {
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
            bmpZoomGraphics.DrawImage(bmpScan, DST_RECT, SRC_RECT, GraphicsUnit.Pixel);

            bmpScanGraphics.Dispose();
            bmpZoomGraphics.Dispose();
        }

        private void drawImages()
        {
            if (bmpScan != null && bmpZoom != null) {
                Graphics pnlScanGraphics = this.pnlScan.CreateGraphics();
                Graphics pnlZoomGraphics = this.pnlZoom.CreateGraphics();

                pnlScanGraphics.DrawImage(bmpScan, 0, 0);
                pnlZoomGraphics.DrawImage(bmpZoom, 0, 0);

                // draw the crosses to divide each panel into 4 quadrants
                pnlScanGraphics.DrawLine(Pens.Black, 0, HALF, SIZE, HALF);
                pnlScanGraphics.DrawLine(Pens.Black, HALF, 0, HALF, SIZE);
                pnlZoomGraphics.DrawLine(Pens.Black, 0, HALF, SIZE, HALF);
                pnlZoomGraphics.DrawLine(Pens.Black, HALF, 0, HALF, SIZE);

                pnlScanGraphics.Dispose();
                pnlZoomGraphics.Dispose();
            }
        }


        // called by the hook procedure that was installed in the constructor
        public void OnMouseMove(int x, int y)
        {
            if (scanning) {
                updateImages(x, y);
                updateColorControls(bmpScan.GetPixel(HALF, HALF));
                
                drawImages();
            }

            this.lblMouseCoords.Text = "Mouse Location: " + x + ", " + y;
        }

        // update the color translations when the 900% view is clicked
        private void pnlZoomClick(object sender, MouseEventArgs e) {
            if (bmpZoom != null) {
                int xTile = (int) Math.Floor((double)(e.X / 9));
                int yTile = (int) Math.Floor((double)(e.Y / 9));

                updateColorControls(bmpZoom.GetPixel(xTile * 9 + 5, yTile * 9 + 5));
            }
        }

        // updates the rgb/hex values
        private void updateColorControls(Color c) {
            this.txtColorHex.Text = ColorTranslator.ToHtml(c).ToLower();
            this.txtRGB.Text = c.R + ", " + c.G + ", " + c.B;
            this.pnlColor.BackColor = c;
        }

        private void _btnInspectMouseUp(object sender, MouseEventArgs e) {
            scanning = false;
        }

        private void _btnInspectMouseDown(object sender, MouseEventArgs e) {
            scanning = true;
        }

        private void ColorInspector_Paint(object sender, PaintEventArgs e) {
            drawImages();
        }

        private void pnlColor_Click(object sender, EventArgs e) {
            ColorInputDialog inputDialog = new ColorInputDialog();

            inputDialog.ShowDialog();

            pnlColor.BackColor = inputDialog.getColor();
        }

        private void OnFormResize(object sender, EventArgs e) {
            if (WindowState == FormWindowState.Minimized) {
                hook.Suspend();
            }
            else if (WindowState == FormWindowState.Normal) {
                hook.Resume();
            }
        }

        private const int SIZE = 81;
        private const int HALF = SIZE / 2; // expecting the truncation
        private readonly Rectangle SRC_RECT = new Rectangle(36, 36, 9, 9);
        private readonly Rectangle DST_RECT = new Rectangle(0, 0, SIZE, SIZE);
        private Bitmap bmpScan;
        private Bitmap bmpZoom;
        private MouseMoveHook hook;
        private bool scanning;
    }
}
