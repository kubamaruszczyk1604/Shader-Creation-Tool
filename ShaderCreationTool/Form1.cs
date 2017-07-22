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
        private readonly Pen m_Pen = new Pen(Color.Snow) { Width = 3 };


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
        bool mov = false;

        // UTIL METHODS
        public void DrawConnectionLine(Graphics g, Point a, Point b)
        {
            Point start = (a.X <= b.X) ? a : b;
            Point end = (start.Equals(a)) ? b : a;
            int halfXDist = (int)(((float)end.X - (float)start.X) * 0.8f);

            Point mid1 = new Point(start.X + halfXDist, start.Y);
            Point mid2 = new Point(mid1.X, end.Y);

            int arrowSize = 3;
            Point[] points =
             {
                start,
                mid1,
                mid2,
                end
                // Arrow
                //new Point(end.X- arrowSize, end.Y - arrowSize),
                //new Point(end.X- arrowSize, end.Y + arrowSize),
                //new Point(end.X, end.Y)
             };
            if (mov)
            {
              //  g.DrawRectangle(m_Pen,GetRectangleByLine(start, mid1));
               EditAreaPanel.Invalidate(GetRectangleByLine(start, mid1));
                if (mid1.Y < mid2.Y)
                {
                    //  g.DrawRectangle(m_Pen, GetRectangleByLine(mid1, mid2));
                    EditAreaPanel.Invalidate(GetRectangleByLine(mid1, mid2));
                }
                else
                {
                    //   g.DrawRectangle(m_Pen, GetRectangleByLine(mid2, mid1));
                    EditAreaPanel.Invalidate(GetRectangleByLine(mid2, mid1));
                }
                // g.DrawRectangle(m_Pen, GetRectangleByLine(mid2, end));
                EditAreaPanel.Invalidate(GetRectangleByLine(mid2, end));
                mov = false;
            }
            g.DrawLines(m_Pen, points);


        }
     

        Rectangle GetRectangleByLine(Point p1, Point p2)
        {
            return new Rectangle(p1.X-20,p1.Y-20,
                Math.Abs(p1.X-p2.X)+40, Math.Abs(p1.Y - p2.Y) + 40);
        }

        private void MoveControlMouseMove(Control control, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                mov = true;
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
            DrawConnectionLine(formGraphics, start, end);

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
