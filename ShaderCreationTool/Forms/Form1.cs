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

      //  private ShaderVectorVariable m_DiffuseColour;
        //private ShaderVectorVariable m_AmbientColour;

        private List<ISCTNode> m_Nodes;
        private List<Connector> m_HighlightedConnectorList;

        private bool m_IsConnecting;
        private Point m_TempLineOrgin;

        //private OnWndProcUpdate m_OnWndProcUpdate;
        private void MainUpdate()
        {
            //canncel connection request
            if (MouseButtons == MouseButtons.Right)
            {
                OnMouseRightDown();
            }
            if(MouseButtons == MouseButtons.Left)
            {
                OnMouseLeftDown();
            }
        }
        static int i = 0;
        private void OnMessage(ulong message, ulong wParam, ulong lParam)
        {
           // i++;
            //SCTConsole.Instance.PrintLine("Refresh call from C++ " + i.ToString() );
            NodeInstantiator.Update(EditAreaPanel);
            if (m_IsConnecting)
            {
                UpdateOnObjectMoved();
                EditAreaPanel.Invalidate(false);
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            m_IsConnecting = false;

            m_MovableKey = new MovableObject(button44);
            m_MovableKey.AddObjectMovedEventListener(UpdateOnObjectMoved);
            m_MovablePreviewPanel = new MovableObject(PreviewAreaPanel);
            m_MovablePreviewPanel.AddObjectMovedEventListener(UpdateOnObjectMoved);

           // m_DiffuseColour = new ShaderVectorVariable(1, 1, 0, 1, "diffuse");
            //Bridge.SetVariable(m_DiffuseColour);
           // m_AmbientColour = new ShaderVectorVariable(0.1f, 0.1f, 0.1f, 1, "ambient");
           // Bridge.SetVariable(m_AmbientColour);
            SCTConsole.Instance.Show();
            m_Nodes = new List<ISCTNode>();
            CreateAndSetupFrameNode(m_Nodes);
            m_HighlightedConnectorList = new List<Connector>();
            NodeInstantiator.SetupInstantiator(TransparentNodePanel);
            NodeInstantiator.AddOnObjectMovedCallback(UpdateOnObjectMoved);
            NodeInstantiator.AddOnPlaceListener(OnPlaceNode);
            Bridge.AddWndProcUpdateCallback(MainUpdate);
            Bridge.AddWndProcMessageCallback(OnMessage);
        }

        private async void StartRenderer(int delayMs)
        {
           // AllocConsole();
            await Task.Delay(delayMs);
            IntPtr pointer = pictureBox1.Handle;
            Bridge.StartRenderer(pictureBox1.Width, pictureBox1.Height, pointer);
        }

        private void CreateAndSetupFrameNode(List<ISCTNode> nodes)
        {
            FrameBufferNode fbn = new FrameBufferNode(FrameBufferWindow);
            fbn.AddOnBeginConnectionCallback(OnConnectionBegin);
            fbn.AddOnBreakConnectionCallback(OnConnectionBreak);
            fbn.AddOnMovedCallback(UpdateOnObjectMoved);
            nodes.Add(fbn);
        }

        private async void AddExampleNodes()
        {
            await Task.Delay(1);
            ShaderVariableDescription inDesc1 = new ShaderVariableDescription("Kolor1", ShaderVariableType.Vector4, ConnectionDirection.In);
            ShaderVariableDescription inDesc2 = new ShaderVariableDescription("Kolor2", ShaderVariableType.Vector4, ConnectionDirection.In);
            ShaderVariableDescription inDesc3 = new ShaderVariableDescription("Kolor3", ShaderVariableType.Vector4, ConnectionDirection.In);
            ShaderVariableDescription inDesc4 = new ShaderVariableDescription("Kolorfff", ShaderVariableType.Vector4, ConnectionDirection.In);

            ShaderVariableDescription outDesc1 = new ShaderVariableDescription("Kolor4", ShaderVariableType.Vector4, ConnectionDirection.Out);
            ShaderVariableDescription outDesc2 = new ShaderVariableDescription("WYJSCIE", ShaderVariableType.Single, ConnectionDirection.Out);


            FunctionNodeDescription d = new FunctionNodeDescription("test of lngh");
            d.AddInputVariable(inDesc1);
            d.AddInputVariable(inDesc2);
            d.AddInputVariable(inDesc3);
            d.AddInputVariable(inDesc3);
            d.AddOutputVariable(outDesc1);
            d.AddOutputVariable(outDesc2);

            for (int i = 0; i < 1; ++i)
            {
                SCTFunctionNode temp = new SCTFunctionNode(FunctionNodeWindow, new Point(240 * i, 300), UpdateOnObjectMoved, d);
                temp.AddOnBeginConnectionCallback(OnConnectionBegin);
                temp.AddOnBreakConnectionCallback(OnConnectionBreak);
                temp.AddOnCloseCallback(OnNodeClose);
                m_Nodes.Add(temp);
            }

        }

        // UTIL METHODS
        private void UpdateOnObjectMoved()
        {
            if (m_IsConnecting) m_TempLine.Invalidate();
            ConnectionManager.UpdateOnObjectMoved();
            BringStaticObjectsToFront();
            EditAreaPanel.Update();
        }

        private void BringStaticObjectsToFront()
        {
            AddGroupBox.BringToFront();
            PreviewAreaPanel.BringToFront();
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
            LockableNodes.LockButtons();

            //Cursor Change
            EditAreaPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            SetCursorRecursive(EditAreaPanel.Controls,Cursors.Hand);

            //Node Highlighting
            List<Connector> allConnectors = new List<Connector>();
            foreach(ISCTNode n in m_Nodes)
            {
                ConnectionDirection dir =
                    (sender.DirectionType == ConnectionDirection.In)?
                    ConnectionDirection.Out:ConnectionDirection.In;
                List<Connector> tempLCon = n.GetAllConnectors(dir);
                if (tempLCon == null) continue;

                m_HighlightedConnectorList.Add(sender);
                sender.SetBackHighlighted();
                foreach(Connector c in tempLCon)
                {
                    if(c == null)
                    {
                        SCTConsole.Instance.PrintLine("JEB");
                    }
                    if (c.ParentNode == sender.ParentNode) continue;
                    if (c.VariableType != sender.VariableType) continue;
                    if (c.Connected) continue;
                    m_HighlightedConnectorList.Add(c);
                    c.SetBackHighlighted();
                }
            }
        }

        private void OnConnectionBreak(Connector sender)
        {
            SCTConsole.Instance.PrintLine("Connector on connection end");
            ConnectionManager.RemoveConnection(sender.ParentConnection);
        }

        private void OnNodeClose(ISCTNode sender)
        {
            SCTConsole.Instance.PrintLine("Node Close request..");
            DialogResult dialogResult =
                MessageBox.Show("Are you sure that you want do delete this node ?", 
                "Confirm node delete", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                SCTConsole.Instance.PrintLine("Node Close request.. Confirmed");
                m_Nodes.Remove(sender);
                if (sender is IDisposable)
                {
                    IDisposable tmp = (IDisposable)sender;
                    tmp.Dispose();
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                SCTConsole.Instance.PrintLine("Node Close request.. Aborted");
            }
        }

        private void OnInputError(string errorDescription,ISCTNode sender)
        {
            MessageBox.Show(errorDescription, "Invalid input",  MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void OnPlaceNode(Point location, NodeType type)
        {
            SCTConsole.Instance.PrintLine("OnPlaceNode()");
            MovableObject.UnlockAllMovement();
            LockableNodes.UnlockButtons();
            Connector.UnlockAllConnectors();


            ISCTNode temp = null;
            if (type == NodeType.Input_Float)
            {
                temp  =  new InputNodeVector(FloatInputWindow, location);
            }
            else if(type == NodeType.Input_Float2)
            {
                temp = new InputNodeVector(Float2InputWindow, location);
            }
            else if (type == NodeType.Input_Float3)
            {
                temp = new InputNodeVector(Float3InputWindow, location);
            }
            else if (type == NodeType.Input_Float4)
            {
                temp = new InputNodeVector(Float4InputWindow, location);
            }
            else if (type == NodeType.Input_Colour)
            {
                temp = new InputNodeColour(ColourInputWindow, location);
            }
            else if(type == NodeType.Input_Texture2D)
            {
                temp = new InputNodeTexture2D(Texture2DInputWindow, location);
            }
            else
            {
                MessageBox.Show(type.ToString() + " NOT IMPLEMENTED");
                return;

            }


            (temp).AddOnMovedCallback(UpdateOnObjectMoved);
            (temp).AddOnBeginConnectionCallback(OnConnectionBegin);
            temp.AddOnBreakConnectionCallback(OnConnectionBreak);

            if (temp is IInputNode)
            {
                IInputNode inpN = (IInputNode)temp;
                inpN.AddOnCloseCallback(OnNodeClose);
                inpN.AddInputErrorCallback(OnInputError);
            }
            m_Nodes.Add(temp);
           
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
            StartRenderer(50);
            PreviewTextLabel.ForeColor = Color.White;  
        }

        // MAIN FORM - UPDATE GENERATED

        private void OnMouseRightDown()
        {
            CancelIsConnecting();
            CancelNewNode();
        }

        private void OnMouseLeftDown()
        {
            NodeInstantiator.CaptureLeftMousePress();
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
            EditAreaPanel.Focus();
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

        private void EditAreaPanel_Scroll(object sender, ScrollEventArgs e)
        {
            EditAreaPanel.Invalidate(false);
        }

        // BUTTONS 
        private void AddVariableButton_Click(object sender, EventArgs e)
        {
            var form = new SelectNodeForm();
            form.ShowDialog();
            if (form.DialogResult == DialogResult.OK)
            {
                SCTConsole.Instance.PrintLine("Selection: " + form.RequestedNodeType.ToString());
                {
                    NodeInstantiator.StartPlacing(form.RequestedNodeType);
                    MovableObject.LockAllMovement();
                    LockableNodes.LockButtons();
                    Connector.LockAllConnectors();
                }
            }
            form.Dispose();
        }

        private void AddNodeButton_Click(object sender, EventArgs e)
        {
           
            var r = new SelectNodeForm();
            r.ShowDialog();
            if (r.DialogResult == DialogResult.OK)
            {
                SCTConsole.Instance.PrintLine("Selection: " + r.RequestedNodeType.ToString());
                AddExampleNodes();
            }
        }

        // TEMPORARY STUFF
        private void button1_Click(object sender, EventArgs e)
        {
            //Bridge.ReloadScene();
            SCTConsole.Instance.Show();
            SCTConsole.Instance.PrintLine("Console shown test..");
   
        }



        // USED METHODS
        private void CancelIsConnecting()
        {
            if (!m_IsConnecting) return;
            m_IsConnecting = false;
            MovableObject.UnlockAllMovement();
            LockableNodes.UnlockButtons();
            if (m_TempLine == null) return;
            m_TempLine.Invalidate();
            EditAreaPanel.Invalidate(false);
            EditAreaPanel.Update();
            m_TempLine = null;
            EditAreaPanel.Cursor = System.Windows.Forms.Cursors.Default;
            SetCursorRecursive(EditAreaPanel.Controls, Cursors.Default);
            foreach (Connector c in m_HighlightedConnectorList)
            {
                c.DisableBackHighlighted();
            }
            m_HighlightedConnectorList.Clear();
        }

        private void CancelNewNode()
        {
            NodeInstantiator.CancelInstantiate();
            MovableObject.UnlockAllMovement();
            LockableNodes.UnlockButtons();
            Connector.UnlockAllConnectors();
        }

        private void textBox_File_Textue2D_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void button44_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ConnectionManager.ConnectionCount; ++i)
            {
                ConnectionManager.GetConnection(i).TestKillLine();
            }
        }

        private void button31_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ConnectionManager.ConnectionCount; ++i)
            {
                ConnectionManager.GetConnection(i).TestRebuiltLine();
            }
        }
    }




}
