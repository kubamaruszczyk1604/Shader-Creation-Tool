using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;


using System.Runtime.InteropServices; 

namespace ShaderCreationTool
{

    public partial class MainWindow : Form
    {

        private Point MouseDownLocation;

        public MainWindow()
        {
            InitializeComponent();
          
        }

        private void MoveControlMouseMove(Control control, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                control.Left = e.X + control.Left - MouseDownLocation.X;
                control.Top = e.Y + control.Top - MouseDownLocation.Y;
                control.Refresh();
            }
        }

        private void MoveControlMouseCapture(Control control, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }

        //Events
        private void button1_Click(object sender, EventArgs e)
        {


            Bridge.ReloadScene();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
           // label1.Parent = pictureBox1;
            StartRenderer(100);
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Bridge.Terminate();
          
        }

        private async void StartRenderer(int delayMs)
        {
            await Task.Delay(delayMs);
            IntPtr pointer = pictureBox1.Handle;
            Bridge.StartRenderer(pictureBox1.Width, pictureBox1.Height, pointer);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
            //ColorDialog cd = new ColorDialog();
            //DialogResult result = cd.ShowDialog();
            //if (result == DialogResult.OK)
            //{
            //    // Set form background to the selected color.
            //    this.BackColor = cd.Color;
            //}
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.Pen myPen;
            myPen = new System.Drawing.Pen(System.Drawing.Color.Red);
            System.Drawing.Graphics formGraphics = e.Graphics;
            formGraphics.DrawLine(myPen, 0, 0, 200, 200);
            myPen.Dispose();
            formGraphics.Dispose();
        }

      

        private void button48_MouseMove(object sender, MouseEventArgs e)
        {

            Control control = (Control)sender;
            MoveControlMouseMove(control, e);
        }

        private void button48_MouseDown(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            MoveControlMouseCapture(control, e);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            MoveControlMouseCapture(control, e);
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            MoveControlMouseMove(control, e);
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Click(object sender, EventArgs e)
        {
            fileToolStripMenuItem.HideDropDown();
        }
    }
}
