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
using System.Reflection;

using System.Runtime.InteropServices;

namespace ShaderCreationTool
{
   

    public partial class MainWindow : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        ConnectionLine m_TestLine;
        ConnectionLine m_TempLine;


        MovableObject m_MovableKey;
        MovableObject m_MovableRenderObject;
        MovableObject m_MovablePreviewPanel;

        ShaderVectorVariable m_DiffuseColour;
        ShaderVectorVariable m_AmbientColour;

        SCTNode m_Node;

        public MainWindow()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            m_TestLine = new ConnectionLine(EditAreaPanel);
            m_TempLine = new ConnectionLine(EditAreaPanel);

            m_MovableKey = new MovableObject(button44);
            m_MovableKey.AddObjectMovedEventListener(UpdateOnObjectMoved);

            m_MovableRenderObject = new MovableObject(button48);
            m_MovableRenderObject.AddObjectMovedEventListener(UpdateOnObjectMoved);

            m_MovablePreviewPanel = new MovableObject(PreviewAreaPanel);
            m_MovablePreviewPanel.AddObjectMovedEventListener(UpdateOnObjectMoved);

            Bridge.TESTUJE = 100;

            m_DiffuseColour = new ShaderVectorVariable(1, 1, 0, 1, "diffuse");
            Bridge.SetVariable(m_DiffuseColour);

            m_AmbientColour = new ShaderVectorVariable(0.1f,0.1f, 0.1f, 1, "ambient");
            Bridge.SetVariable(m_AmbientColour); 
            SCTConsole.Instance.Show();

        }

        private async void StartRenderer(int delayMs)
        {
            //AllocConsole();
            await Task.Delay(delayMs);
            IntPtr pointer = pictureBox1.Handle;
            Bridge.StartRenderer(pictureBox1.Width, pictureBox1.Height, pointer);
            
        }
      

        // UTIL METHODS
        private void UpdateOnObjectMoved()
        {
            m_TestLine.Invalidate();
            m_TempLine.Invalidate();
            ConnectionManager.UpdateOnObjectMoved();
            EditAreaPanel.Update();
        }

     
        private void OnConnectionBegin(Connector sender)
        {
            SCTConsole.Instance.PrintLine("Connector called on click.");

        }

        //**************************************  UI EVENTS  ***********************************************//


        //  MAIN FORM
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Bridge.Terminate();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            SCTConsole.Instance.PrintLine("Main Window Loaded...");
           
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            StartRenderer(100);
            PreviewTextLabel.ForeColor = Color.White;

            m_Node = new SCTNode(SCTElement, new Point(200, 300));
            m_Node.RegisterListener_OnMoved(UpdateOnObjectMoved);
            m_Node.RegisterListener_OnBeginConnection(OnConnectionBegin);

        }


        // MAIN EDIT AREA PANEL
        private void EditAreaPanel_Paint(object sender, PaintEventArgs e)
        {
           Graphics formGraphics = e.Graphics;
            ConnectionManager.Draw(formGraphics);
           m_TestLine.DrawConnectionLine(formGraphics, button48, PreviewAreaPanel);
          
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
            //Bridge.ReloadScene();
            //List<CheckBox> buttons = ControlExtensions.GetAllChildreenControls<CheckBox>(exp).Cast<CheckBox>().ToList();

            //m_Connector = new Connector(buttons[0]);
            //m_Connector.RegisterListener_BeginConnection(OnConnectionBegin);

            SCTConsole.Instance.Show();
            SCTConsole.Instance.PrintLine("Console shown test..");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            DialogResult result = cd.ShowDialog();
            if (result == DialogResult.OK)
            {
                // Set form background to the selected color.
                //this.BackColor = cd.Color;
                m_DiffuseColour.Set((float)(cd.Color.R) / 255.0f,
                    (float)(cd.Color.G) / 255.0f,
                    (float)(cd.Color.B) / 255.0f,
                    (float)(cd.Color.A) / 255.0f
                    );
             
            }
        }

        private void EditAreaPanel_Scroll(object sender, ScrollEventArgs e)
        {
            EditAreaPanel.Invalidate(false);
        }
       
    }

   
}
