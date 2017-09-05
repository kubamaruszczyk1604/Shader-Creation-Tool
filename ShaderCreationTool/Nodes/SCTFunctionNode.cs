using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace ShaderCreationTool
{
 

    class SCTFunctionNode : IDisposable,ISCTNode
    {
        private Panel m_SctElement;
        private MovableObject m_Mover;
        private List<Connector> m_OutputConnectors;
        private List<Connector> m_InputConnectors;
        private string m_Label = string.Empty;
        private NodeCloseButtonCallback p_CloseCallback;
        private static bool s_ButtonsLocked = false;
        private FunctionNodeDescription m_FunctionNodeDescription;
        private string m_UniqueID;

        private static int s_InstanceCounter = 0;

        //////////////////////////////////////////  PUBLIC  ///////////////////////////////////////////////

        public NodeType GetNodeType() { return NodeType.Function; }
        public string GetNodeID() { return m_UniqueID; }
        public FunctionNodeDescription NodeDescription { get { return m_FunctionNodeDescription; } }
        public string FunctionCodeString { get { return m_FunctionNodeDescription.GetFunctionString(); } }
        public Point GetPosition() { return m_SctElement.Location; }
        public void ChangeUniqueID(string uniqueID) { m_UniqueID = uniqueID; }

        public SCTFunctionNode(Panel nodeTemplate, Point location, FunctionNodeDescription description)
        {
            m_UniqueID = NodeIDCreator.CreateID(description, s_InstanceCounter);
            m_FunctionNodeDescription = description;
            //Copy template (make local instance)
            m_SctElement = nodeTemplate.CopyAsSCTElement(true);
            m_SctElement.Visible = true;
            m_SctElement.Location = location;
            m_SctElement.MouseDown += Panel_MouseDown;

            //Make object movable
            m_Mover = new MovableObject(m_SctElement);
          
            //Find template tick boxes
            List<CheckBox> boxes = ControlExtensions.GetAllChildreenControls<CheckBox>(m_SctElement).Cast<CheckBox>().ToList();

            // Find input and output template boxes
            CheckBox inBox = null;
            CheckBox outBox = null;
            for (int i = 0; i < boxes.Count; ++i)
            {
                if(boxes[i].Name.Contains(Connector.s_InSlotSequenceID))
                {
                    inBox = boxes[i];
                }
                else if (boxes[i].Name.Contains(Connector.s_OutSlotSequenceID))
                {
                    outBox = boxes[i];
                }
            }
            boxes.Clear();

            //Adjust size
            const int connectorYOffset = 23;
            int biggestVarCount = (description.InputCount > description.OutputCount) ?
                             description.InputCount : description.OutputCount;

            List<Panel> panels = ControlExtensions.GetAllChildreenControls<Panel>(m_SctElement).Cast<Panel>().ToList();
            panels.Add(m_SctElement);
            foreach (Panel p in panels)
            {
                p.Size = new Size(p.Size.Width, p.Size.Height + connectorYOffset * (biggestVarCount-1));
                p.Click += AnyElement_Click;
            }
            int inConnectorCounter = 0;
            int outConnectorCounter = 0;
            //Create connectors
            m_OutputConnectors = new List<Connector>();
            m_InputConnectors = new List<Connector>();
            for (int i = 0; i < description.InputCount; ++i)
            {
                CheckBox cd = (i == 0) ? inBox : inBox.CopyAsSCTElement(true);
                cd.Location = new Point(cd.Location.X, cd.Location.Y + connectorYOffset * i);
               // cd.Text = "[" + description.GetInVariableDescription(i).Type.ToString() + "]\n";
                cd.Text = description.GetInVariableDescription(i).Name;
                cd.Click += AnyElement_Click;
                Connector tempCon = new Connector(cd, description.GetInVariableDescription(i).Type,this, "IN_" 
                    + cd. Text + inConnectorCounter.ToString());
                tempCon.SetShaderVaribaleDescription(description.GetInVariableDescription(i));
                m_InputConnectors.Add(tempCon);
                inConnectorCounter++;
            }

            for (int i = 0; i < description.OutputCount; ++i)
            {
                CheckBox cd = (i == 0) ? outBox : outBox.CopyAsSCTElement(true);
                cd.Location = new Point(cd.Location.X, cd.Location.Y + connectorYOffset * i);
                cd.Text = description.GetOutVariableDescription(i).Name;
                cd.Click += AnyElement_Click;
                Connector tempCon = new Connector(cd, description.GetOutVariableDescription(i).Type,this, 
                    "OUT_" + cd.Text + outConnectorCounter.ToString());
                tempCon.SetShaderVaribaleDescription(description.GetOutVariableDescription(i));
                m_OutputConnectors.Add(tempCon);
                outConnectorCounter++;
            }

            // Set node title
            List<Label> labels = ControlExtensions.GetAllChildreenControls<Label>(m_SctElement).Cast<Label>().ToList();
            labels[0].Text = description.Name;
            labels[0].MouseDown += TitleLabel_MouseDown;
            labels[0].MouseMove += TitleLabel_MouseMove;
            labels[0].Click += AnyElement_Click;

            //Close Button click setup
            List<Button> buttons = ControlExtensions.GetAllChildreenControls<Button>(m_SctElement).Cast<Button>().ToList();
            buttons[0].Click += CloseButton_Click;
            s_InstanceCounter++;
            m_SctElement.Update();
        }

        public SCTFunctionNode(Panel nodeTemplate, Point location, ObjectMovedCallback onObjectMoved, FunctionNodeDescription description) :
            this(nodeTemplate,location,description)
        {
            AddOnMovedCallback(onObjectMoved);
        }


        public void Serialize(XmlWriter target)
        {
            XmlNodeSerializer.SerializeFunctionNode(target, this);
        }



        /// <summary>
        /// Registered method will be called when node is moved
        /// </summary>
        /// <param name="onMovedCallback"></param>
        public void AddOnMovedCallback(ObjectMovedCallback onMovedCallback)
        {
            m_Mover.AddObjectMovedEventListener(onMovedCallback);
        }

        /// <summary>
        /// Registered callback method will be called when any of the node's connectors is clicked
        /// </summary>
        /// <param name="onBeginConnection"></param>
        public void AddOnBeginConnectionCallback(BeginConnectionCallback onBeginConnection)
        {
            foreach(Connector c in m_OutputConnectors)
            {
                c.AddCallback_BeginConnectionRequest(onBeginConnection);
            }
            foreach (Connector c in m_InputConnectors)
            {
                c.AddCallback_BeginConnectionRequest(onBeginConnection);
            }
        }

        /// <summary>
        /// Registered method will be called when user clicks on one of the connected connectors
        /// </summary>
        /// <param name="onBreakConnection"></param>
        public void AddOnBreakConnectionCallback(BreakConnectionCallback onBreakConnection)
        {
            foreach (Connector c in m_OutputConnectors)
            {
                c.AddCallback_BreakConnectionRequest(onBreakConnection);
            }
            foreach (Connector c in m_InputConnectors)
            {
                c.AddCallback_BreakConnectionRequest(onBreakConnection);
            }
        }

        /// <summary>
        /// Registered method will be called when close button of the node is clicked
        /// </summary>
        /// <param name="callback"></param>
        public void AddOnCloseCallback(NodeCloseButtonCallback callback)
        {
            p_CloseCallback += callback;
        }

        public Connector GetConnector(ConnectionDirection type, int index)
        {
            if (type == ConnectionDirection.Out) return m_OutputConnectors[index];
            else if (type == ConnectionDirection.In) return m_InputConnectors[index];
            else return null;
        }

        public List<Connector> GetAllConnectors(ConnectionDirection type)
        {
            if (type == ConnectionDirection.Out) return m_OutputConnectors;
            else if (type == ConnectionDirection.In) return m_InputConnectors;
            else return null;
        }

        public List<Connector> GetAllConnectors()
        {
            List<Connector> outList = new List<Connector>();
            outList.AddRange(m_OutputConnectors);
            outList.AddRange(m_InputConnectors);
            return outList;
        }

        public void Dispose()
        {
            List<Connector> connectors = GetAllConnectors();
            foreach(Connector c in connectors)
            {
                if (!c.Connected) continue;
                if (!ConnectionManager.ContainsConncetion(c.ParentConnection)) continue;
                ConnectionManager.RemoveConnection(c.ParentConnection);
            }
            m_SctElement.Parent.Controls.Remove(m_SctElement);
        }

        static public void LockButtons()
        {
            s_ButtonsLocked = true;
        }

        static public void UnlockButtons()
        {
            s_ButtonsLocked = false;
        }

        ////////////////// UI EVENTS ////////////////
        private void CloseButton_Click(object sender, EventArgs e)
        {
            if (s_ButtonsLocked) return;
            if (p_CloseCallback != null) p_CloseCallback(this);
       
        }

        private void AnyElement_Click(object sender, EventArgs e)
        {
            m_SctElement.SuspendLayout();
            m_SctElement.BringToFront();
            ((Control)sender).Focus();
            m_SctElement.Update();
        }

        private void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            m_SctElement.BringToFront();
        }


        private void TitleLabel_MouseDown(object sender, MouseEventArgs e)
        {
            m_Mover.MoveControlMouseCapture(m_SctElement, e);
        }

        private void TitleLabel_MouseMove(object sender, MouseEventArgs e)
        {
            m_Mover.MoveControlMouseMove(m_SctElement, e);
        }

    }
}
