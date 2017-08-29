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
        //[DllImport("kernel32.dll", SetLastError = true)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //static extern bool AllocConsole();

        private const string NODES_PATH = @"..\Data\StdNodes\nodes.txt";
        private const string VERT_PATH = @"..\Data\Shaders\glVert.txt";
        private const string FRAG_PATH = @"..\Data\Shaders\glFrag.txt";

        private SimpleZLine m_TempLine;
        private MovableObject m_MovablePreviewPanel;

        private List<ISCTNode> m_Nodes;
        private List<Connector> m_HighlightedConnectorList;

        private bool m_IsConnecting;
        private Point m_TempLineOrgin;

        private ZoomController m_ZoomController;

        private ICodeParser m_CodeParser;

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

        private void OnMessage(ulong message, ulong wParam, ulong lParam)
        {
            label_ConnectionCount.Text = ConnectionManager.ConnectionCount.ToString();
            label_NodeCount.Text = m_Nodes.Count.ToString();
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
            m_MovablePreviewPanel = new MovableObject(PreviewAreaPanel);
            m_MovablePreviewPanel.AddObjectMovedEventListener(UpdateOnObjectMoved);      
            SCTConsole.Instance.Show();
            m_Nodes = new List<ISCTNode>();
            CreateAndSetupFrameNode(m_Nodes);
            m_HighlightedConnectorList = new List<Connector>();
            NodeInstantiator.SetupInstantiator(TransparentNodePanel);
            NodeInstantiator.AddOnObjectMovedCallback(UpdateOnObjectMoved);
            NodeInstantiator.AddOnPlaceListener(OnPlaceNode);
            Bridge.AddWndProcUpdateCallback(MainUpdate);
            Bridge.AddWndProcMessageCallback(OnMessage);

            m_ZoomController = new ZoomController(ZoomInButton, ZoomOutButton);

            m_CodeParser = new CodeParserGLSL();
            
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

    
        

        // USED METHODS

        private void Start_AddVariableNode(bool input)
        {
            var form = new SelectNodeForm(input);
            form.ShowDialog();
            if (form.DialogResult == DialogResult.OK)
            {
                SCTConsole.Instance.PrintDebugLine("Selection: " + form.RequestedInputNodeType.ToString());
                {
                    NodeInstantiator.StartPlacing(form.RequestedInputNodeType);
                    MovableObject.LockAllMovement();
                    LockableNodes.LockButtons();
                    Connector.LockAllConnectors();
                }
            }
            form.Dispose();
        }


        private void Start_AddFunctionNode()
        {
            var form = new SelectNodeForm(NODES_PATH);
            form.ShowDialog();
            if (form.DialogResult == DialogResult.OK)
            {
                SCTConsole.Instance.PrintDebugLine("Selection: " + form.RequestedFunctionNodeDescription.Name);
                {
                    NodeInstantiator.StartPlacing(form.RequestedFunctionNodeDescription);
                    MovableObject.LockAllMovement();
                    LockableNodes.LockButtons();
                    Connector.LockAllConnectors();
                }
            }
            form.Dispose();
        }

        

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


        ////////////////////////////  CALLBACKS   /////////////////////////////

        // Method Called when "not connected" connector is clicked
        private bool OnConnectionBegin(Connector sender)
        {
            // If the program is in "connecting state" already:
            // ..this click was on targer connector - make new connecion
            if (m_IsConnecting)
            {
                if (!Connection.CheckConnectionValidity(Connector.GetPreviouslyClickedConnector(), sender)) return false;
                var tempCon = new Connection(Connector.GetPreviouslyClickedConnector(), sender, EditAreaPanel);
                ConnectionManager.AddConnecion(tempCon);
                CancelIsConnecting();
                return false;
            }

            //Connection open request - first connector clicked
            
            SCTConsole.Instance.PrintDebugLine("Connector on connection begin.");
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
                        SCTConsole.Instance.PrintDebugLine("JEB");
                    }
                    if (c.ParentNode == sender.ParentNode) continue;
                    if (c.VariableType != sender.VariableType) continue;
                    if (c.Connected) continue;
                    m_HighlightedConnectorList.Add(c);
                    c.SetBackHighlighted();
                }
            }
            return true;
        }

        private void OnConnectionBreak(Connector sender)
        {
            SCTConsole.Instance.PrintDebugLine("Connector on connection end");
            ConnectionManager.RemoveConnection(sender.ParentConnection);
        }

        private void OnNodeClose(ISCTNode sender)
        {
            SCTConsole.Instance.PrintDebugLine("Node Close request..");
            DialogResult dialogResult =
                MessageBox.Show("Are you sure that you want do delete this node ?", 
                "Confirm node delete", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                SCTConsole.Instance.PrintDebugLine("Node Close request.. Confirmed");
                m_Nodes.Remove(sender);
                if (sender is IDisposable)
                {
                    IDisposable tmp = (IDisposable)sender;
                    tmp.Dispose();
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                SCTConsole.Instance.PrintDebugLine("Node Close request.. Aborted");
            }
        }

        private void OnInputError(string errorDescription,ISCTNode sender)
        {
            MessageBox.Show(errorDescription, "Invalid input",  MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private IAttribNode anode;
        private void OnPlaceNode(Point location, NodeType type)
        {
            SCTConsole.Instance.PrintDebugLine("OnPlaceNode()");
            MovableObject.UnlockAllMovement();
            LockableNodes.UnlockButtons();
            Connector.UnlockAllConnectors();
            ISCTNode temp = null;
            switch (type)
            {
                case NodeType.Input_Float: { temp = new InputNodeVector(FloatInputWindow, location); break; }
                case NodeType.Input_Float2: { temp = new InputNodeVector(Float2InputWindow, location); break; } 
                case NodeType.Input_Float3: { temp = new InputNodeVector(Float3InputWindow, location); break; }
                case NodeType.Input_Float4:{ temp = new InputNodeVector(Float4InputWindow, location); break; }
                case NodeType.Input_Colour: { temp = new InputNodeColour(ColourInputWindow, location); break; }
                case NodeType.Input_Texture2D: { temp = new InputNodeTexture2D(Texture2DInputWindow, location); break; }
                case NodeType.AttribPosition: { temp = new AttribNodeWithSelection(VertexPositionWindow, location); break; }
                case NodeType.AttribNormal: { temp = new AttribNodeWithSelection(NormalVectorWindow, location); break; }
                case NodeType.AttribInput_CameraPos: { temp = new AttribNodeWithSelection(CameraPositionWindow, location); break; }
                case NodeType.AttribUVs: { temp = new AttribNodeSimple(UVsWindow, location); break; }
                case NodeType.AttribInput_Time: { temp = new AttribNodeSimple(TimeWindow, location); break; }
                case NodeType.Funtion: { temp = new SCTFunctionNode(FunctionNodeWindow, location, NodeInstantiator.FunctionDescriptionStruct); break; }
                default:{ MessageBox.Show(type.ToString() + " NOT IMPLEMENTED"); return; }
            }

            temp.AddOnMovedCallback(UpdateOnObjectMoved);
            temp.AddOnBeginConnectionCallback(OnConnectionBegin);
            temp.AddOnBreakConnectionCallback(OnConnectionBreak);

            if (temp is IInputNode)
            {
                IInputNode inpN = (IInputNode)temp;
                inpN.AddOnCloseCallback(OnNodeClose);
                inpN.AddInputErrorCallback(OnInputError);
            }
            else if (temp is SCTFunctionNode)
            {
                SCTFunctionNode inpN = (SCTFunctionNode)temp;
                inpN.AddOnCloseCallback(OnNodeClose);
            }
            else if (temp is IAttribNode)
            {
                IAttribNode inpN = (IAttribNode)temp;
                inpN.AddOnCloseCallback(OnNodeClose);
                anode = inpN;
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
            SCTConsole.Instance.PrintDebugLine("Main Window Loaded...");
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            StartRenderer(100);
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
            m_ZoomController.RegisterLeftClick();
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

        int lastMouseX = 0;
        int lastMouseY = 0;
        private void PreviewWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseButtons == MouseButtons.Left)
            {
                SCTConsole.Instance.PrintDebugLine("Is pressed now!");
                int amountX = lastMouseX - e.X;
                int amountY = lastMouseY - e.Y;
                Bridge.RotateObject((float)DegreeToRadian(amountY), (float)DegreeToRadian(amountX), 0);

            }
            lastMouseX = e.X;
            lastMouseY = e.Y;
        }
      


        // BUTTONS 
        private void AddVariableButton_Click(object sender, EventArgs e)
        {
            Start_AddVariableNode(true);
        }

        private void AddNodeButton_Click(object sender, EventArgs e)
        {
            Start_AddFunctionNode();
        }


        private void AddAttribButton_Click(object sender, EventArgs e)
        {
            Start_AddVariableNode(false);
        }

  


        private void button44_Click(object sender, EventArgs e)
        {
            //for (int i = 0; i < ConnectionManager.ConnectionCount; ++i)
            //{
            //    ConnectionManager.GetConnection(i).TestKillLine();
            //}

            string fragStatus;
            string fragCode;
        

            string vertStat;
            string vertCode;
            m_CodeParser.TranslateNetworkVertex(null, null, out vertCode, out vertStat);
            m_CodeParser.TranslateNetworkFragment(m_Nodes, ConnectionManager.ConnectionList, out fragCode, out fragStatus);

            //SCTConsole.Instance.PrintLine("VERTEX SHADER: \r\n" + vertCode);
            //SCTConsole.Instance.PrintLine("FRAGMENT SHADER: \r\n" + fragCode);
            TextFileReaderWriter.Save(VERT_PATH, vertCode);
            TextFileReaderWriter.Save(FRAG_PATH, fragCode);
            SCTConsole.Instance.PrintDebugLine(TextFileReaderWriter.LastError);
            Thread.Sleep(100);
            Bridge.ReloadScene();
            //

        }

     

      
        ////////////////////////////////  MENU ITEMS ///////////////////
        private void AddUniformVariable_MenuItem_Click(object sender, EventArgs e)
        {
            Start_AddVariableNode(true);
        }

        private void FunctionNodeMenuItem_Click(object sender, EventArgs e)
        {
            Start_AddFunctionNode();
        }
        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void AttributeNodeMenuItem_Click(object sender, EventArgs e)
        {
            Start_AddVariableNode(false);
        }

        /////////////////////////////// UTIL  ////////////////////////
        void SetCursorRecursive(IEnumerable theControls, Cursor cursor)
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
        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }
        private double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //SCTConsole.Instance.PrintLine("\r\n\r\n");
            //List<Connection> inConnections = ConnectionManager.GetInputConnections();
            //foreach(Connection c in inConnections)
            //{
            //    SCTConsole.Instance.PrintLine("\r\n" + c.Info);
            //}

            SCTConsole.Instance.PrintLine("Name of the variable: " + anode.GetVariableName());
        }

      
    }




}
