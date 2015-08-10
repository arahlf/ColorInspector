namespace ColorInspector
{
    partial class ColorInputDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorInputDialog));
            this.txtColor = new System.Windows.Forms.TextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.lblInvalidColor = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtColor
            // 
            this.txtColor.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtColor.Location = new System.Drawing.Point(10, 12);
            this.txtColor.Name = "txtColor";
            this.txtColor.Size = new System.Drawing.Size(157, 23);
            this.txtColor.TabIndex = 0;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(52, 61);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 1;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.OnSubmit);
            // 
            // lblInvalidColor
            // 
            this.lblInvalidColor.AutoSize = true;
            this.lblInvalidColor.ForeColor = System.Drawing.Color.Red;
            this.lblInvalidColor.Location = new System.Drawing.Point(56, 41);
            this.lblInvalidColor.Name = "lblInvalidColor";
            this.lblInvalidColor.Size = new System.Drawing.Size(67, 13);
            this.lblInvalidColor.TabIndex = 2;
            this.lblInvalidColor.Text = "Invalid color.";
            this.lblInvalidColor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblInvalidColor.Visible = false;
            // 
            // ColorInputDialog
            // 
            this.AcceptButton = this.btnSubmit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(179, 96);
            this.Controls.Add(this.lblInvalidColor);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.txtColor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColorInputDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Enter a color.";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtColor;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Label lblInvalidColor;
    }
}