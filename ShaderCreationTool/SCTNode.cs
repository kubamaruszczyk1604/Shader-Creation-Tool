using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShaderCreationTool
{
    delegate void NodeCloseButtonCallback(SCTNode sender);

    class SCTNode : IDisposable
    {
        private Panel m_SctElement;
        private MovableObject m_Mover;
        private List<Connector> m_OutputConnectors;
        private List<Connector> m_InputConnectors;
        private string m_Label = string.Empty;
        private NodeCloseButtonCallback p_CloseCallback;


        //////////////////////////////////////////  PUBLIC  ///////////////////////////////////////////////
        
        public SCTNode(Panel nodeTemplate, Point location, NodeDescription description)
        {
            //Copy template (make local instance)
            m_SctElement = nodeTemplate.CopyAsSCTElement(true);
            m_SctElement.Visible = true;
            m_SctElement.Location = location;

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
            }

            //Create connectors
            m_OutputConnectors = new List<Connector>();
            m_InputConnectors = new List<Connector>();
            for (int i = 0; i < description.InputCount; ++i)
            {
                CheckBox cd = (i == 0) ? inBox : inBox.CopyAsSCTElement(true);
                cd.Location = new Point(cd.Location.X, cd.Location.Y + connectorYOffset * i);
               // cd.Text = "[" + description.GetInVariableDescription(i).Type.ToString() + "]\n";
                cd.Text = description.GetInVariableDescription(i).Name;
                
                Connector tempCon = new Connector(cd, description.GetInVariableDescription(i).Type,this);
                m_InputConnectors.Add(tempCon);
            }

            for (int i = 0; i < description.OutputCount; ++i)
            {
                CheckBox cd = (i == 0) ? outBox : outBox.CopyAsSCTElement(true);
                cd.Location = new Point(cd.Location.X, cd.Location.Y + connectorYOffset * i);
                cd.Text = description.GetOutVariableDescription(i).Name;
                Connector tempCon = new Connector(cd, description.GetOutVariableDescription(i).Type,this);
                m_OutputConnectors.Add(tempCon);
            }


            // Set node title
            List<Label> labels = ControlExtensions.GetAllChildreenControls<Label>(m_SctElement).Cast<Label>().ToList();
            labels[0].Text = description.Name;

            //Close Button click setup
            List<Button> buttons = ControlExtensions.GetAllChildreenControls<Button>(m_SctElement).Cast<Button>().ToList();
            buttons[0].Click += CloseButton_Click;

        }

        public SCTNode(Panel nodeTemplate, Point location, ObjectMovedCallback onObjectMoved, NodeDescription description) :
            this(nodeTemplate,location,description)
        {
            AddOnMovedCallback(onObjectMoved);
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

        ////////////////// UI EVENTS ////////////////
        private void CloseButton_Click(object sender, EventArgs e)
        {
            if (p_CloseCallback != null) p_CloseCallback(this);
       
        }


    }
}
