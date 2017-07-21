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
                //new Point(marginleft + width - arrowSize, marginTop + height - arrowSize),
                //new Point(marginleft + width - arrowSize, marginTop + height + arrowSize),
                //new Point(marginleft + width, marginTop + height)
             };
            if (mov)
            {
                EditAreaPanel.Invalidate(GetRegionByLine(a, mid1));
                EditAreaPanel.Invalidate(GetRegionByLine(mid1, mid2));
                EditAreaPanel.Invalidate(GetRegionByLine(mid2, end));
                // EditAreaPanel.Invalidate(new Rectangle(start.X, start.Y - 20, mid1.X - start.X, 30), false);

                mov = false;
            }
            g.DrawLines(m_Pen, points);


        }
        Region GetRegionByLine(Point a, Point b)
        {
            GraphicsPath gp = new GraphicsPath();
            //  gp.AddPolygon(new Point[] {a, new Point(b.X, a.Y), b, new Point(a.X, b.Y)});
            gp.AddPolygon(new Point[] { a, new Point(b.X, a.Y), b, new Point(a.X, b.Y) });
            RectangleF rf = gp.GetBounds();
            gp.Dispose();

            rf.Inflate(15f, 15f);

            return new Region(rf);
        }


        private void MoveControlMouseMove(Control control, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                mov = true;
                control.Left = e.X + control.Left - m_MouseDownLocation.X;
                control.Top = e.Y + control.Top - m_MouseDownLocation.Y;


                //   EditAreaPanel.Invalidate(false);


                control.Update();
                EditAreaPanel.Update();

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

            Point start = new Point(button48.Left + button48.Width, button48.Top + button48.Height / 2);
            Point end = new Point(PreviewAreaPanel.Left, PreviewAreaPanel.Top + PreviewAreaPanel.Height / 2);
            DrawConnectionLine(formGraphics, start, end);
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
