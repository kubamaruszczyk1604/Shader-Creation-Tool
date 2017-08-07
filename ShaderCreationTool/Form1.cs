﻿using System;
using System.Collections.Generic;
using System.Collections;
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

        ConnectionLine m_TempLine;


        MovableObject m_MovableKey;
        MovableObject m_MovablePreviewPanel;

        ShaderVectorVariable m_DiffuseColour;
        ShaderVectorVariable m_AmbientColour;

        List<SCTNode> m_Nodes;

        public MainWindow()
        {
            InitializeComponent();
            this.DoubleBuffered = true;



            m_MovableKey = new MovableObject(button44);
            m_MovableKey.AddObjectMovedEventListener(UpdateOnObjectMoved);


            m_MovablePreviewPanel = new MovableObject(PreviewAreaPanel);
            m_MovablePreviewPanel.AddObjectMovedEventListener(UpdateOnObjectMoved);

            Bridge.TESTUJE = 100;

            m_DiffuseColour = new ShaderVectorVariable(1, 1, 0, 1, "diffuse");
            Bridge.SetVariable(m_DiffuseColour);

            m_AmbientColour = new ShaderVectorVariable(0.1f,0.1f, 0.1f, 1, "ambient");
            Bridge.SetVariable(m_AmbientColour); 
            SCTConsole.Instance.Show();


            m_Nodes = new List<SCTNode>();
        }

        private async void StartRenderer(int delayMs)
        {
           // AllocConsole();
            await Task.Delay(delayMs);
            IntPtr pointer = pictureBox1.Handle;
            Bridge.StartRenderer(pictureBox1.Width, pictureBox1.Height, pointer); 
        }
      

        // UTIL METHODS
        private void UpdateOnObjectMoved()
        {
            if(m_IsConnecting) m_TempLine.Invalidate();
            ConnectionManager.UpdateOnObjectMoved();
            EditAreaPanel.Update();
        }


        bool m_IsConnecting = false;
        Point m_TempLineOrgin;

        private void OnConnectionBegin(Connector sender)
        {
            if (m_IsConnecting)
            {

                var tempCon = new Connection(Connector.GetPreviouslyClickedConnector(),
                    sender,
                     EditAreaPanel
                     );

                ConnectionManager.AddConnecion(tempCon);
                CancelIsConnecting();
                return;
            }
           SCTConsole.Instance.PrintLine("Connector on connection begin.");
           m_TempLine = new ConnectionLine(EditAreaPanel);
           m_TempLineOrgin = EditAreaPanel.PointToClient(System.Windows.Forms.Cursor.Position);
           m_IsConnecting = true;
            MovableObject.LockAllMovement();
        }

        private void OnConnectionBreak(Connector sender)
        {
            SCTConsole.Instance.PrintLine("Connector on connection end");
           ConnectionManager.RemoveConnection(sender.ParentConnection);
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

            for (int i = 0; i < 2; ++i)
            {
                SCTNode temp = new SCTNode(SCTElement, new Point(200*i, 300), UpdateOnObjectMoved);
                temp.RegisterListener_OnBeginConnection(OnConnectionBegin);
                temp.RegisterListener_OnBreakConnection(OnConnectionBreak);
                m_Nodes.Add(temp);
            }

           var tempCon = new Connection(m_Nodes[0].GetAllConnectors(ConnectorType.Source)[0],
                m_Nodes[1].GetAllConnectors(ConnectorType.Destination)[2],
                EditAreaPanel
                );
                   
            ConnectionManager.AddConnecion(tempCon);   
        }


        // MAIN EDIT AREA PANEL
        private void EditAreaPanel_Paint(object sender, PaintEventArgs e)
        {
           Graphics formGraphics = e.Graphics;
           ConnectionManager.Draw(formGraphics);

            if (m_IsConnecting)
            {
               m_TempLine.DrawConnectionLine(formGraphics, 
                   m_TempLineOrgin, 
                   EditAreaPanel.PointToClient(System.Windows.Forms.Cursor.Position));
            }
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

        private void mousedowntest(object sender, MouseEventArgs e)
        {

        }

        private void button29_Click(object sender, EventArgs e)
        {
        }

        private void EditAreaPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_IsConnecting)
            {
                UpdateOnObjectMoved();
                EditAreaPanel.Invalidate(false);
            }
        }

        private void EditAreaPanel_MouseClick(object sender, MouseEventArgs e)
        {
            //canncel connection request
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (!m_IsConnecting) return;
                CancelIsConnecting();
            }
        }

        private void CancelIsConnecting()
        {
            m_IsConnecting = false;
            MovableObject.UnlockAllMovement();
            if (m_TempLine == null) return;
            m_TempLine.Dispose();
            m_TempLine = null;

        }

        private void button48_Click(object sender, EventArgs e)
        {

        }
    }


 
   
}
