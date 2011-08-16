using System;
using System.Drawing;
using System.Windows.Forms;

namespace ColorInspector
{
    public partial class ColorInputDialog : Form
    {
        public ColorInputDialog() {
            InitializeComponent();
        }

        private void OnSubmit(object sender, EventArgs e) {
            if (this.GetColor() != Color.Empty) {
                this.Hide();
            }
            else {
                this.lblInvalidColor.Show();
            }
        }

        public Color GetColor() {
            String input = this.txtColor.Text;
            Color color = Color.Empty;

            try {
                color = ColorTranslator.FromHtml(input);
            }
            catch (Exception) {
                // shortcut for hex w/o hash
                try {
                    color = ColorTranslator.FromHtml("#" + input);
                }
                catch (Exception) {
                    // ignored
                }
            }

            return color;
        }
    }
}
