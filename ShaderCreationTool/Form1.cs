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

        private Point m_MouseDownLocation;
        private readonly Pen m_Pen = new Pen(Color.Red) { Width = 3 };

        private bool m_PanelRedrawn = false;

        public MainWindow()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
          
        }

        private async void StartRenderer(int delayMs)
        {
            await Task.Delay(delayMs);
            IntPtr pointer = pictureBox1.Handle;
            Bridge.StartRenderer(pictureBox1.Width, pictureBox1.Height, pointer);
        }


        // UTIL METHODS
        public void DrawConnectionLine(Graphics g, Point a, Point b)
        {
           // Pen myPen = new Pen(Color.Red);
           // myPen.Width = 2;
            // Create array of points that define lines to draw.
           


            int arrowSize = 3;
            Point[] points =
             {
                a,
              //  new Point(marginleft, height + marginTop),
                b,
                // Arrow
                //new Point(marginleft + width - arrowSize, marginTop + height - arrowSize),
                //new Point(marginleft + width - arrowSize, marginTop + height + arrowSize),
                //new Point(marginleft + width, marginTop + height)
             };

            g.DrawLines(m_Pen, points);

         
        }

        public void DrawLShapeLine(System.Drawing.Graphics g, int intMarginLeft, int intMarginTop, int intWidth, int intHeight)
        {
            Pen myPen = new Pen(Color.Red);
            myPen.Width = 2;
            // Create array of points that define lines to draw.
            int marginleft = intMarginLeft;
            int marginTop = intMarginTop;
            int width = intWidth;
            int height = intHeight;
            int arrowSize = 3;
            Point[] points =
             {
                new Point(marginleft, marginTop),
                new Point(marginleft, height + marginTop),
                new Point(marginleft + width, marginTop + height),
                // Arrow
                new Point(marginleft + width - arrowSize, marginTop + height - arrowSize),
                new Point(marginleft + width - arrowSize, marginTop + height + arrowSize),
                new Point(marginleft + width, marginTop + height)
             };

            g.DrawLines(myPen, points);
        }

        private void MoveControlMouseMove(Control control, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                control.Left = e.X + control.Left - m_MouseDownLocation.X;
                control.Top = e.Y + control.Top - m_MouseDownLocation.Y;

                if (m_PanelRedrawn)
                {
                    m_PanelRedrawn = false;
                }
                EditAreaPanel.Update();
                control.Update();
                //pictureBox1.Update();
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
            //formGraphics.DrawLine(myPen, 0, 0, 200, 200);

            // DrawLShapeLine(formGraphics, 50, 0, 300, 400);
           DrawConnectionLine(formGraphics, new Point(0, 0), new Point(400, 100));
            // myPen.Dispose();
            //formGraphics.Dispose();
            m_PanelRedrawn = true;
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

      
    }
}
