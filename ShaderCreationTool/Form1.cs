using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        private Point m_MouseDownLocation;
        private readonly Pen m_Pen = new Pen(Color.White, 3);

        ConnectionLine m_TestLine;

        public MainWindow()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            m_TestLine = new ConnectionLine(EditAreaPanel);
          //  this.Controls.SetChildIndex(EditAreaPanel, 0);
                
        }

        private async void StartRenderer(int delayMs)
        {
            await Task.Delay(delayMs);
            IntPtr pointer = pictureBox1.Handle;
            Bridge.StartRenderer(pictureBox1.Width, pictureBox1.Height, pointer);
        }
      

        // UTIL METHODS
        

        private void MoveControlMouseMove(Control control, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                m_TestLine.Invalidate();
                control.Left = e.X + control.Left - m_MouseDownLocation.X;
                control.Top = e.Y + control.Top - m_MouseDownLocation.Y;
                control.Update();
                EditAreaPanel.Update();
               
            }

        }


        private void MoveControlMouseCapture(Control control, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                m_MouseDownLocation = e.Location;
            }
        }

        //**************************************  UI EVENTS  ***********************************************//


        //  MAIN FORM
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Bridge.Terminate();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            StartRenderer(100);
            PreviewTextLabel.ForeColor = Color.White;
            //EditAreaPanel.Do
        }


        // MAIN EDIT AREA PANEL

        private void EditAreaPanel_Paint(object sender, PaintEventArgs e)
        {
            //Pen myPen;
            //myPen = new Pen(System.Drawing.Color.Red);
            Graphics formGraphics = e.Graphics;

            Point start = new Point(button48.Left + button48.Width, button48.Top + button48.Height / 2);
            Point end = new Point(PreviewAreaPanel.Left, PreviewAreaPanel.Top + PreviewAreaPanel.Height / 2);
           m_TestLine.DrawConnectionLine(formGraphics, start, end);

        }

        private void EditAreaPanel_Click(object sender, EventArgs e)
        {
            fileToolStripMenuItem.HideDropDown();
        }


        // PREVIEW AREA PANEL

        private void PreviewAreaPanel_MouseDown(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            MoveControlMouseCapture(control, e);
        }


        private void PreviewAreaPanel_MouseMove(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            MoveControlMouseMove(control, e);
        }

        private void PreviewTextLabel_MouseDown(object sender, MouseEventArgs e)
        {
            PreviewAreaPanel_MouseDown(PreviewAreaPanel, e);
        }

        private void PreviewTextLabel_MouseMove(object sender, MouseEventArgs e)
        {
            PreviewAreaPanel_MouseMove(PreviewAreaPanel, e);
        }







        // TEMPORARY STUFF

        private void button1_Click(object sender, EventArgs e)
        {


            Bridge.ReloadScene();
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

        private void panel1Paint(object sender, PaintEventArgs e)
        {
            

        }
    }
}
