namespace ColorInspectorSpace
{
    partial class ColorInspector
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                // remove the hook when the application is closing
                Hooking.DestroyHook();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorInspector));
            this.txtColorHex = new System.Windows.Forms.TextBox();
            this.lblHex = new System.Windows.Forms.Label();
            this.btnInspect = new System.Windows.Forms.Button();
            this.lblMouseCoords = new System.Windows.Forms.Label();
            this.lblRGB = new System.Windows.Forms.Label();
            this.txtRGB = new System.Windows.Forms.TextBox();
            this.lbl900 = new System.Windows.Forms.Label();
            this.lbl100 = new System.Windows.Forms.Label();
            this.pnlColor = new System.Windows.Forms.Panel();
            this.pnlZoom = new ColorInspectorSpace.PicturePanel();
            this.pnlScan = new ColorInspectorSpace.PicturePanel();
            this.SuspendLayout();
            // 
            // txtColorHex
            // 
            this.txtColorHex.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtColorHex.Location = new System.Drawing.Point(210, 74);
            this.txtColorHex.Name = "txtColorHex";
            this.txtColorHex.Size = new System.Drawing.Size(100, 21);
            this.txtColorHex.TabIndex = 2;
            // 
            // lblHex
            // 
            this.lblHex.AutoSize = true;
            this.lblHex.Location = new System.Drawing.Point(211, 58);
            this.lblHex.Name = "lblHex";
            this.lblHex.Size = new System.Drawing.Size(29, 13);
            this.lblHex.TabIndex = 3;
            this.lblHex.Text = "Hex:";
            // 
            // btnInspect
            // 
            this.btnInspect.Location = new System.Drawing.Point(235, 106);
            this.btnInspect.Name = "btnInspect";
            this.btnInspect.Size = new System.Drawing.Size(75, 23);
            this.btnInspect.TabIndex = 4;
            this.btnInspect.Text = "Inspect";
            this.btnInspect.UseVisualStyleBackColor = true;
            this.btnInspect.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnInspect_MouseDown);
            this.btnInspect.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnInspect_MouseUp);
            // 
            // lblMouseCoords
            // 
            this.lblMouseCoords.AutoSize = true;
            this.lblMouseCoords.Location = new System.Drawing.Point(12, 119);
            this.lblMouseCoords.Name = "lblMouseCoords";
            this.lblMouseCoords.Size = new System.Drawing.Size(107, 13);
            this.lblMouseCoords.TabIndex = 6;
            this.lblMouseCoords.Text = "Mouse Location: 0, 0";
            // 
            // lblRGB
            // 
            this.lblRGB.AutoSize = true;
            this.lblRGB.Location = new System.Drawing.Point(207, 12);
            this.lblRGB.Name = "lblRGB";
            this.lblRGB.Size = new System.Drawing.Size(33, 13);
            this.lblRGB.TabIndex = 7;
            this.lblRGB.Text = "RGB:";
            // 
            // txtRGB
            // 
            this.txtRGB.Location = new System.Drawing.Point(210, 28);
            this.txtRGB.Name = "txtRGB";
            this.txtRGB.Size = new System.Drawing.Size(100, 20);
            this.txtRGB.TabIndex = 8;
            // 
            // lbl900
            // 
            this.lbl900.AutoSize = true;
            this.lbl900.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl900.Location = new System.Drawing.Point(136, 98);
            this.lbl900.Name = "lbl900";
            this.lbl900.Size = new System.Drawing.Size(28, 12);
            this.lbl900.TabIndex = 9;
            this.lbl900.Text = "900%";
            // 
            // lbl100
            // 
            this.lbl100.AutoSize = true;
            this.lbl100.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl100.Location = new System.Drawing.Point(38, 98);
            this.lbl100.Name = "lbl100";
            this.lbl100.Size = new System.Drawing.Size(28, 12);
            this.lbl100.TabIndex = 10;
            this.lbl100.Text = "100%";
            // 
            // pnlColor
            // 
            this.pnlColor.Location = new System.Drawing.Point(210, 108);
            this.pnlColor.Name = "pnlColor";
            this.pnlColor.Size = new System.Drawing.Size(19, 19);
            this.pnlColor.TabIndex = 13;
            // 
            // pnlZoom
            // 
            this.pnlZoom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlZoom.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pnlZoom.Location = new System.Drawing.Point(109, 12);
            this.pnlZoom.Name = "pnlZoom";
            this.pnlZoom.Size = new System.Drawing.Size(83, 83);
            this.pnlZoom.TabIndex = 12;
            this.pnlZoom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlZoom_MouseDown);
            // 
            // pnlScan
            // 
            this.pnlScan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlScan.Location = new System.Drawing.Point(12, 12);
            this.pnlScan.Name = "pnlScan";
            this.pnlScan.Size = new System.Drawing.Size(83, 83);
            this.pnlScan.TabIndex = 11;
            // 
            // ColorInspector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 141);
            this.Controls.Add(this.pnlColor);
            this.Controls.Add(this.pnlZoom);
            this.Controls.Add(this.pnlScan);
            this.Controls.Add(this.lbl100);
            this.Controls.Add(this.lbl900);
            this.Controls.Add(this.txtRGB);
            this.Controls.Add(this.lblRGB);
            this.Controls.Add(this.lblMouseCoords);
            this.Controls.Add(this.btnInspect);
            this.Controls.Add(this.lblHex);
            this.Controls.Add(this.txtColorHex);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ColorInspector";
            this.Text = "Color Inspector";
            this.TopMost = true;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ColorInspector_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtColorHex;
        private System.Windows.Forms.Label lblHex;
        private System.Windows.Forms.Button btnInspect;
        private System.Windows.Forms.Label lblMouseCoords;
        private System.Windows.Forms.Label lblRGB;
        private System.Windows.Forms.TextBox txtRGB;
        private System.Windows.Forms.Label lbl900;
        private System.Windows.Forms.Label lbl100;
        private PicturePanel pnlScan;
        private PicturePanel pnlZoom;
        private System.Windows.Forms.Panel pnlColor;
    }
}

