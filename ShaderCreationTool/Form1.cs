using System;
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

        private SimpleZLine m_TempLine;
        private MovableObject m_MovableKey;
        private MovableObject m_MovablePreviewPanel;

        private ShaderVectorVariable m_DiffuseColour;
        private ShaderVectorVariable m_AmbientColour;

        private List<SCTNode> m_Nodes;
        private List<Connector> m_HighlightedList;

        private bool m_IsConnecting;
        private Point m_TempLineOrgin;

        public MainWindow()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            m_IsConnecting = false;

            m_MovableKey = new MovableObject(button44);
            m_MovableKey.AddObjectMovedEventListener(UpdateOnObjectMoved);
            m_MovablePreviewPanel = new MovableObject(PreviewAreaPanel);
            m_MovablePreviewPanel.AddObjectMovedEventListener(UpdateOnObjectMoved);

            m_DiffuseColour = new ShaderVectorVariable(1, 1, 0, 1, "diffuse");
            Bridge.SetVariable(m_DiffuseColour);
            m_AmbientColour = new ShaderVectorVariable(0.1f, 0.1f, 0.1f, 1, "ambient");
            Bridge.SetVariable(m_AmbientColour);
            SCTConsole.Instance.Show();
            m_Nodes = new List<SCTNode>();
            m_HighlightedList = new List<Connector>();
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
            if (m_IsConnecting) m_TempLine.Invalidate();
            ConnectionManager.UpdateOnObjectMoved();
            EditAreaPanel.Update();
        }

        void SetCursorRecursive(IEnumerable theControls,Cursor cursor)
        {
            foreach (Control control in theControls)
            {
                if (control.HasChildren)
                {
                    SetCursorRecursive(control.Controls, cursor);
                }
                else
                {
                    control.Cursor = cursor;
                }
            }
        }

        ////////////////////////////  CALLBACKS   /////////////////////////////

        // Method Called when "no connected" connector is clicked
        private void OnConnectionBegin(Connector sender)
        {
            // If the program is in "connecting state" already:
            // ..this click was on targer connector - make new connecion
            if (m_IsConnecting)
            {
                if (!Connection.CheckConnectionValidity(Connector.GetPreviouslyClickedConnector(), sender)) return;
                var tempCon = new Connection(Connector.GetPreviouslyClickedConnector(), sender, EditAreaPanel);
                ConnectionManager.AddConnecion(tempCon);
                CancelIsConnecting();
                return;
            }


            //Connection open request - first connector clicked
            SCTConsole.Instance.PrintLine("Connector on connection begin.");
            m_TempLine = new SimpleZLine(EditAreaPanel);
            m_TempLineOrgin = EditAreaPanel.PointToClient(System.Windows.Forms.Cursor.Position);
            m_IsConnecting = true;
            MovableObject.LockAllMovement();

            //Cursor Change
            EditAreaPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            SetCursorRecursive(EditAreaPanel.Controls,Cursors.Hand);

            //Node Highlighting
            List<Connector> allConnectors = new List<Connector>();
            foreach(SCTNode n in m_Nodes)
            {

                ConnectionDirection dir =
                    (sender.DirectionType == ConnectionDirection.In)?
                    ConnectionDirection.Out:ConnectionDirection.In;
                List<Connector> tempLCon = n.GetAllConnectors(dir);

                m_HighlightedList.Add(sender);
                sender.SetBackHighlighted();
                foreach(Connector c in tempLCon)
                {
                    if (c.ParentNode == sender.ParentNode) continue;
                    if (c.VariableType != sender.VariableType) continue;
                    if (c.Connected) continue;
                    m_HighlightedList.Add(c);
                    c.SetBackHighlighted();
                }
            }
        }

        private void OnConnectionBreak(Connector sender)
        {
            SCTConsole.Instance.PrintLine("Connector on connection end");
            ConnectionManager.RemoveConnection(sender.ParentConnection);
        }

        private void OnNodeClose(SCTNode sender)
        {
            SCTConsole.Instance.PrintLine("Node Close request..");
            DialogResult dialogResult =
                MessageBox.Show("Are you sure that you want do delete this node ?", 
                "Confirm node delete", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                SCTConsole.Instance.PrintLine("Node Close request.. Confirmed");
                m_Nodes.Remove(sender);
                sender.Dispose();
            }
            else if (dialogResult == DialogResult.No)
            {
                SCTConsole.Instance.PrintLine("Node Close request.. Aborted");
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
            SCTConsole.Instance.PrintLine("Main Window Loaded...");
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            StartRenderer(100);
            PreviewTextLabel.ForeColor = Color.White;

            ShaderVariableDescription inDesc1 = new ShaderVariableDescription("Kolor1", ShaderVariableType.Vector4, ConnectionDirection.In);
            ShaderVariableDescription inDesc2 = new ShaderVariableDescription("Kolor2", ShaderVariableType.Vector4, ConnectionDirection.In);
            ShaderVariableDescription inDesc3 = new ShaderVariableDescription("Kolor3", ShaderVariableType.Vector4, ConnectionDirection.In);
            ShaderVariableDescription inDesc4 = new ShaderVariableDescription("Kolorfff", ShaderVariableType.Vector4, ConnectionDirection.In);

            ShaderVariableDescription outDesc1 = new ShaderVariableDescription("Kolor4", ShaderVariableType.Vector4, ConnectionDirection.Out);
            ShaderVariableDescription outDesc2 = new ShaderVariableDescription("WYJSCIE", ShaderVariableType.Vector4, ConnectionDirection.Out);


            NodeDescription d = new NodeDescription("SUKA");
            d.AddInputVariable(inDesc1);
            d.AddInputVariable(inDesc2);
            d.AddInputVariable(inDesc3);
            d.AddInputVariable(inDesc3);
            d.AddOutputVariable(outDesc1);
            d.AddOutputVariable(outDesc2);

            for (int i = 0; i < 2; ++i)
            {
                SCTNode temp = new SCTNode(MainPanel, new Point(240 * i, 300), UpdateOnObjectMoved, d);
                temp.AddOnBeginConnectionCallback(OnConnectionBegin);
                temp.AddOnBreakConnectionCallback(OnConnectionBreak);
                temp.AddOnCloseCallback(OnNodeClose);
               m_Nodes.Add(temp);
            }

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
            m_TempLine.Invalidate();
            EditAreaPanel.Invalidate(false);
            EditAreaPanel.Update();
            m_TempLine = null;
            EditAreaPanel.Cursor = System.Windows.Forms.Cursors.Default;
            SetCursorRecursive(EditAreaPanel.Controls, Cursors.Default);
            foreach (Connector c in m_HighlightedList)
            {
                c.DisableBackHighlighted();
            }
            m_HighlightedList.Clear();
        }
    }




}
