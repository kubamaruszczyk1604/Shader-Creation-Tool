using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShaderCreationTool
{
    public partial class ImagePreviewForm : Form
    {

        public ImagePreviewForm(Image image, string varName, string path)
        {
            InitializeComponent();
            pictureBox1.Image = image;
            this.Text += ":  " + varName;
            label_fileName.Text = path;
        }

        private void ImagePreviewForm_Load(object sender, EventArgs e)
        {

        }
    }
}
