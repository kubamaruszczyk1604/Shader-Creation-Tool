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
using System.IO;
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
        string m_CurrentFilePath;

        public MainWindow()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            m_IsConnecting = false;
            m_MovablePreviewPanel = new MovableObject(PreviewAreaPanel);
            m_MovablePreviewPanel.AddObjectMovedEventListener(UpdateOnObjectMoved);
            SCTConsole.Instance.Show();
            m_Nodes = new List<ISCTNode>();
            CreateTargetNode(m_Nodes);
            m_HighlightedConnectorList = new List<Connector>();
            NodeInstantiator.SetupInstantiator(TransparentNodePanel);
            NodeInstantiator.AddOnObjectMovedCallback(UpdateOnObjectMoved);
            NodeInstantiator.AddOnPlaceListener(OnPlaceNode);
            Bridge.AddWndProcUpdateCallback(WinformUpdate);
            Bridge.AddWndProcMessageCallback(OnMessage);
            Bridge.AddOnSceneReloadCallback(OnSceneReloaded);

            m_ZoomController = new ZoomController(ZoomInButton, ZoomOutButton);

            m_CodeParser = new CodeParserGLSL();
            m_CurrentFilePath = string.Empty;
            XmlNodeSerializer.PlaceNodeCalback += OnPlaceNode;

        }

        //////////////////////////// WINAPI CALLBACKS ///////////////////

       /// <summary>
       /// Form Update() method called once per frame.
       /// </summary>
        private void WinformUpdate()
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

        /// <summary>
        /// Form OnMessage() method called on any user input (mouse, keyboard) related to the form
        /// and its childreen 
        /// </summary>
        /// <param name="message">Winapi message</param>
        /// <param name="wParam">Winapi wParam</param>
        /// <param name="lParam">Winapi lParam</param>
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

        /// <summary>
        /// Initializes and starts C++ renderer.
        /// </summary>
        /// <param name="delayMs">Delay in miliseconds.</param>
        private async void StartRenderer(int delayMs)
        {
           // AllocConsole();
            await Task.Delay(delayMs);
            IntPtr pointer = pictureBox1.Handle;
            Bridge.StartRenderer(pictureBox1.Width, pictureBox1.Height, pointer);
        }

        /// <summary>
        /// Creates target "FrameBuffer" node. This node is "non closable" and will be always present.
        /// </summary>
        /// <param name="nodes">Main list of nodes.</param>
        private void CreateTargetNode(List<ISCTNode> nodes)
        {
            FrameBufferNode fbn = new FrameBufferNode(FrameBufferWindow);
            fbn.AddOnBeginConnectionCallback(OnConnectionBegin);
            fbn.AddOnBreakConnectionCallback(OnConnectionBreak);
            fbn.AddOnMovedCallback(UpdateOnObjectMoved);
            nodes.Add(fbn);
        }

  
        ////////// SCT METHODS /////////

        /// <summary>
        /// Creates and builds shaders.
        /// Reloads renderer.
        /// </summary>
        private void BuildShaders()
        {
            SCTConsole.Instance.Show();
            SCTConsole.Instance.BringToFront();
            SCTConsole.Instance.PrintLine("\r\n\r\n***** CREATE AND BUILD SHADERS  *****\r\n");
            string vertStat;
            string vertCode;
            SCTConsole.Instance.Print("Parsing vertex shader..");
            m_CodeParser.TranslateNetworkVertex(null, null, out vertCode, out vertStat);
            SCTConsole.Instance.PrintLine(vertStat);

            string fragStatus;
            string fragCode;
            SCTConsole.Instance.Print("Parsing fragment shader..");
            m_CodeParser.TranslateNetworkFragment(m_Nodes, ConnectionManager.ConnectionList, out fragCode, out fragStatus);
            SCTConsole.Instance.PrintLine(fragStatus);


            SCTConsole.Instance.Print("Saving temp shaders..");
            TextFileReaderWriter.Save(VERT_PATH, vertCode);
            TextFileReaderWriter.Save(FRAG_PATH, fragCode);

          
            Thread.Sleep(50);
            SCTConsole.Instance.PrintLine((TextFileReaderWriter.LastError == string.Empty)?
                "OK": "FAILED\r\n" + TextFileReaderWriter.LastError);

      
            Bridge.ReloadScene();
           
            

        }

        /// <summary>
        /// Starts the process of variable node addition
        /// </summary>
        /// <param name="input">true - user uniform node, false - attrib node</param>
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

        /// <summary>
        /// Starts proces of function node addition.
        /// </summary>
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
   
        /// <summary>
        /// Causes application to exit "Is Connecting" mode and return to normal mode. 
        /// </summary>
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

        // Method cancel new node placement causes return to "normal" mode.
        private void CancelNewNode()
        {
            NodeInstantiator.CancelInstantiate();
            MovableObject.UnlockAllMovement();
            LockableNodes.UnlockButtons();
            Connector.UnlockAllConnectors();
        }

        /// <summary>
        /// Method passed as a callback to all movable objects.
        /// Causes EditAreaPanel refresh.
        /// </summary>
        private void UpdateOnObjectMoved()
        {
            if (m_IsConnecting) m_TempLine.Invalidate();
            ConnectionManager.UpdateOnObjectMoved();
            BringStaticObjectsToFront();
            EditAreaPanel.Update();
        }

        /// <summary>
        /// Brings all non-movable buttons and panels to front (so the nodes can never be on top of them)
        /// </summary>
        private void BringStaticObjectsToFront()
        {
            AddGroupBox.BringToFront();
            PreviewAreaPanel.BringToFront();
        }

        private void RemoveAllNodes()
        {
            FrameBufferNode fbNode = null;
            foreach (ISCTNode node in m_Nodes)
            {
                if (node is IDisposable)
                {
                    IDisposable tmp = (IDisposable)node;
                    tmp.Dispose();
                }
                else if (node is FrameBufferNode)
                {
                    fbNode = (FrameBufferNode)node;
                }
            }
            m_Nodes.Clear();
            m_Nodes.Add(fbNode);
        }

        private void ResetCounters()
        {
           InputNodeTexture2D.SetCounter(0);
           InputNodeColour.SetCounter(0);
           InputNodeVector.SetCounter(0);
           AttribNodeSimple.SetCounter(0);
           AttribNodeWithSelection.SetCounter(0);
           SCTFunctionNode.SetCounter(0);
        }

        private bool SaveAs()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();

            saveDialog.Filter = "SCT Network Files (*.net)|*.net|All files (*.*)|*.*";
            saveDialog.FilterIndex = 1;
            saveDialog.RestoreDirectory = true;

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                string path = saveDialog.FileName;
                XmlNodeSerializer.Save(path, m_Nodes, ConnectionManager.ConnectionList);
                m_CurrentFilePath = path;
                return true;
            }
            return false;
        }

        public void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "SCT Network Files (*.net)|*.net|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                RemoveAllNodes();
                ResetCounters();
                if (!XmlNodeSerializer.ReadNodes(openFileDialog.FileName, ref m_Nodes, OnPlaceNode, EditAreaPanel))
                {
                    MessageBox.Show("READING FAILED!");
                }
                else
                {
                    m_CurrentFilePath = openFileDialog.FileName;
                }

            }
            EditAreaPanel.Update();
        }
        ////////////////////////////  CALLBACKS   /////////////////////////////

        private void OnSceneReloaded()
        {
            SCTConsole.Instance.PrintLine(Bridge.GetLastCompilerMessage());
        }


        /// <summary>
        ///  Method Called when "not connected" connector is clicked
        /// </summary>
        /// <param name="sender">Connector on which user clicked.</param>
        /// <returns>true - connector is the "source connector"(first one clicked),
        ///  false - connector is the "target connector" )</returns>
        private bool OnConnectionBegin(Connector sender)
        {
            
            // If the program is in "connecting state" already:
            // ..this click was on targer connector - make new connecion
            if (m_IsConnecting)
            {
                // FINALIZE CONNECTION CREATION REQUEST
                if (!Connection.CheckConnectionValidity(Connector.GetPreviouslyClickedConnector(), sender)) return false;
                var tempCon = new Connection(Connector.GetPreviouslyClickedConnector(), sender, EditAreaPanel);
                ConnectionManager.AddConnecion(tempCon);
                CancelIsConnecting();
                return false;
            }

            //CONNECTION OPEN REQUEST - first connector clicked
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


        /// <summary>
        /// Called on disconnect request.
        /// </summary>
        /// <param name="sender">Connector requesting "dissconnect" action.</param>
        private void OnConnectionBreak(Connector sender)
        {
            SCTConsole.Instance.PrintDebugLine("Connector on connection end");
            ConnectionManager.RemoveConnection(sender.ParentConnection);
        }

        /// <summary>
        /// Called when node button "X"(close) is pressed.
        /// </summary>
        /// <param name="sender">Node to close.</param>
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

        /// <summary>
        /// Callback method called when user is is incorrect.
        /// </summary>
        /// <param name="errorDescription">string description of an error</param>
        /// <param name="sender">Input node with incorrect input from user</param>
        private void OnInputError(string errorDescription,ISCTNode sender)
        {
            MessageBox.Show(errorDescription, "Invalid input",  MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private IAttribNode anode;
        /// <summary>
        /// Method finalizes nodes placement. 
        /// Called wyhen user conform positon of the new node.
        /// </summary>
        /// <param name="location">Where to place the node on EditAreaPanel.</param>
        /// <param name="type">Type of node to be created.</param>
        private ISCTNode OnPlaceNode(Point location, NodeType type)
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
                case NodeType.Function: { temp = new SCTFunctionNode(FunctionNodeWindow, location, NodeInstantiator.FunctionDescriptionStruct); break; }
                default:{ MessageBox.Show(type.ToString() + " NOT IMPLEMENTED"); return null; }
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
            return temp;
        }


        //**************************************  UI EVENTS  ***********************************************//


        //  MAIN FORM
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_Nodes.Count < 2)
            {
                Bridge.Terminate();
                return;
            }
            DialogResult dialogResult = MessageBox.Show("Would you like to save your current work?", "Save", MessageBoxButtons.YesNoCancel);
            if (dialogResult == DialogResult.Yes)
            {
                if (File.Exists(m_CurrentFilePath))
                {
                    XmlNodeSerializer.Save(m_CurrentFilePath, m_Nodes, ConnectionManager.ConnectionList);
                }
                else
                {
                    if (!SaveAs()) { e.Cancel = true; }
                }
                Bridge.Terminate();
            }
            else if (dialogResult == DialogResult.No)
            {
                Bridge.Terminate();
            }
            else if(dialogResult == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
            
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
            editToolStripMenuItem.HideDropDown();
            buildToolStripMenuItem.HideDropDown();
            consoleToolStripMenuItem.HideDropDown();
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
            EditAreaPanel.Update();
        }

        int lastMouseX = 0;
        int lastMouseY = 0;
        private void PreviewWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseButtons == MouseButtons.Left)
            {
                int amountX = lastMouseX - e.X;
                int amountY = lastMouseY - e.Y;
                Bridge.RotateObject((float)DegToRad(amountY), (float)DegToRad(amountX), 0);

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
            BuildShaders();
        }

        ////////////////////////////////  MENU ITEMS ///////////////////

        private void OpenFileMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void SaveAsMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }
        
        private void SaveMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(m_CurrentFilePath))
            {
                XmlNodeSerializer.Save(m_CurrentFilePath, m_Nodes, ConnectionManager.ConnectionList);
            }
            else SaveAs();
        }
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

        private void CompileAndRunMenuItem_Click(object sender, EventArgs e)
        {
            BuildShaders();
        }

        private void RunMenuItem1_Click(object sender, EventArgs e)
        {
            Bridge.ReloadScene();
        }

        private void CleanProjectMenuItem_Click(object sender, EventArgs e)
        {
           // TextFileReaderWriter.ClearTxtFile(VERT_PATH);
            TextFileReaderWriter.ClearTxtFile(FRAG_PATH);
            Bridge.ReloadScene();
        }

        private void ShowConsoleMenuItem_Click(object sender, EventArgs e)
        {
            SCTConsole.Instance.Show();
            SCTConsole.Instance.BringToFront();
        }

        private void HideConsoleMenuItem_Click(object sender, EventArgs e)
        {
            SCTConsole.Instance.Hide();
        }


        /////////////////////////////// UTIL  ////////////////////////

        /// <summary>
        /// Sets cursor for a control group and all childreen.
        /// </summary>
        /// <param name="theControls">Controls list.</param>
        /// <param name="cursor">Cursor to set.</param>
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

        private double DegToRad(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        private double RadToDeg(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

 
        
    }




}
