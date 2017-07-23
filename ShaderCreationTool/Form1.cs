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

        ConnectionLine m_TestLine;
        ConnectionLine m_TestLine2;

        MovableObject m_MovableKey;
        MovableObject m_MovableRenderObject;
        MovableObject m_MovablePreviewPanel;
   
        
        public MainWindow()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            m_TestLine = new ConnectionLine(EditAreaPanel);
            m_TestLine2 = new ConnectionLine(EditAreaPanel);

            m_MovableKey = new MovableObject(button44);
            m_MovableKey.AddObjectMovedEventListener(UpdateOnMouseMove);

            m_MovableRenderObject = new MovableObject(button48);
            m_MovableRenderObject.AddObjectMovedEventListener(UpdateOnMouseMove);

            m_MovablePreviewPanel = new MovableObject(PreviewAreaPanel);
            m_MovablePreviewPanel.AddObjectMovedEventListener(UpdateOnMouseMove);

            Bridge.TESTUJE = 100;
           
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
        private void PreviewTextLabel_MouseDown(object sender, MouseEventArgs e)
        {
            m_MovablePreviewPanel.MoveControlMouseCapture(PreviewAreaPanel, e);
        }

        private void PreviewTextLabel_MouseMove(object sender, MouseEventArgs e)
        {
            m_MovablePreviewPanel.MoveControlMouseMove(PreviewAreaPanel, e);
        }




        // TEMPORARY STUFF
        private void button1_Click(object sender, EventArgs e)
        {
            Bridge.ReloadScene();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            DialogResult result = cd.ShowDialog();
            if (result == DialogResult.OK)
            {
                // Set form background to the selected color.
                this.BackColor = cd.Color;
            }
        }

        private void EditAreaPanel_Scroll(object sender, ScrollEventArgs e)
        {
            EditAreaPanel.Invalidate(false);
        }

   
    }
}
