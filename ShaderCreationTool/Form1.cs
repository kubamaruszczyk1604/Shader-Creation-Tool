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

namespace ShaderCreationTool
{
    
    public partial class MainWindow : Form
    {
        private Thread m_Thread;
        private bool m_Running = false;

        public MainWindow()
        {
            InitializeComponent();
          
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
            CppApplicationInterface.ReloadScene();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            IntPtr pointer = pictureBox1.Handle;
            CppApplicationInterface.StartRenderer(pictureBox1.Width,pictureBox1.Height,pointer);
            m_Running = true;
            m_Thread = new Thread(RefreshThreadMethod);
            m_Thread.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_Running = false;
          
        }


        private void RefreshThreadMethod()
        {

            while (m_Running)
            {
                CppApplicationInterface.Update();
            }
            CppApplicationInterface.Terminate();
        }
    }
}
