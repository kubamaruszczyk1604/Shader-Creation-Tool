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

        public MainWindow()
        {
            InitializeComponent();
          
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
          

            // CppApplicationInterface.ReloadScene();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void Form1_Shown(object sender, EventArgs e)
        {

            StartRenderer(50);
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


     
    }
}
