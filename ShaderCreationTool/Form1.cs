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
        ConnectionLine m_TestLine;
        ConnectionLine m_TestLine2;
        MovableObject movableTestObject;

        public MainWindow()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            m_TestLine = new ConnectionLine(EditAreaPanel);
            m_TestLine2 = new ConnectionLine(EditAreaPanel);
            //  this.Controls.SetChildIndex(EditAreaPanel, 0);
            movableTestObject = new MovableObject(button44);
            movableTestObject.AddObjectMovedEventListener(UpdateOnMouseMove);
        }

        private async void StartRenderer(int delayMs)
        {
            await Task.Delay(delayMs);
            IntPtr pointer = pictureBox1.Handle;
            Bridge.StartRenderer(pictureBox1.Width, pictureBox1.Height, pointer);
        }
      

        // UTIL METHODS
        void UpdateOnMouseMove()
        {
            m_TestLine.Invalidate();
            m_TestLine2.Invalidate();
            EditAreaPanel.Update();
        }


        private void MoveControlMouseMove(Control control, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                


                control.Left = e.X + control.Left - m_MouseDownLocation.X;
                control.Top = e.Y + control.Top - m_MouseDownLocation.Y;
                control.Update();

                UpdateOnMouseMove();
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
           Graphics formGraphics = e.Graphics;
           m_TestLine.DrawConnectionLine(formGraphics, button48, PreviewAreaPanel);
          // m_TestLine2.DrawConnectionLine(formGraphics, button29.Location, PreviewAreaPanel.Location);

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

        private void EditAreaPanel_Scroll(object sender, ScrollEventArgs e)
        {
            EditAreaPanel.Invalidate(false);
        }

   
    }
}
