using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ColorInspector
{
    public partial class ColorInputDialog : Form
    {
        public ColorInputDialog() {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e) {
            this.Hide();
        }

        public Color getColor() {
            try {
                return ColorTranslator.FromHtml(this.txtColor.Text);
            }
            catch (Exception) {
                return Color.Empty;
            }
        }
    }
}
