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


    class FrameBufferNode: ISCTNode
    {
        private Panel m_SctElement;
        private MovableObject m_Mover;
        private List<Connector> m_InputConnectors;
        private string m_Label = string.Empty;
        private string m_UniqueID = "FRAME_BUFFER";

        public NodeType GetNodeType() { return NodeType.Target; }
        public string GetNodeID() { return m_UniqueID; }

        public FrameBufferNode(Panel windowControl)
        {
            //Copy template (make local instance)
            m_SctElement = windowControl;
          //m_SctElement.MouseDown += Panel_MouseDown;

            //Make object movable
            m_Mover = new MovableObject(m_SctElement);


            // Set events to allow "click and drag" through the label
            List<Label> labels = ControlExtensions.GetAllChildreenControls<Label>(m_SctElement).Cast<Label>().ToList();
            labels[0].MouseDown += TitleLabel_MouseDown;
            labels[0].MouseMove += TitleLabel_MouseMove;

            //Find all tick boxes
            List<CheckBox> boxes = ControlExtensions.GetAllChildreenControls<CheckBox>(m_SctElement).Cast<CheckBox>().ToList();

            // Find input and output template boxes
            // and create connections
            m_InputConnectors = new List<Connector>();
            for (int i = 0; i < boxes.Count; ++i)
            {
                if (boxes[i].Name.Contains(Connector.s_InSlotSequenceID))
                {
                    CheckBox tempBox = boxes[i];
                    ShaderVariableType varType = ShaderVariableType.Single;
                    if (tempBox.Name.Contains("Colour"))
                    {
                        varType = ShaderVariableType.Vector4;
                    }
                    else if (tempBox.Name.Contains("Depth"))
                    {
                        varType = ShaderVariableType.Single;
                    }
                    else
                    {
                        SCTConsole.Instance.PrintLine("WRONG SEQUENCE IN CHECKBOX NAME: FRAME BUFFER NODE\n");
                        throw new Exception("WRONG SEQUENCE");
                    }

                    Connector tempCon = new Connector(tempBox, varType, this,"IN_" + i.ToString());
                    m_InputConnectors.Add(tempCon);
                }
                else
                {
                    SCTConsole.Instance.PrintLine("WRONG SEQUENCE IN CHECKBOX NAME: FRAME BUFFER NODE\n");
                    throw new Exception("WRONG SEQUENCE");
                }
            }

        }

        public void AddOnMovedCallback(ObjectMovedCallback onMovedCallback)
        {
            m_Mover.AddObjectMovedEventListener(onMovedCallback);
        }

        public void AddOnBeginConnectionCallback(BeginConnectionCallback onBeginConnection)
        {
            foreach (Connector c in m_InputConnectors)
            {
                c.AddCallback_BeginConnectionRequest(onBeginConnection);
            }
        }

        public void AddOnBreakConnectionCallback(BreakConnectionCallback onBreakConnection)
        {
            foreach (Connector c in m_InputConnectors)
            {
                c.AddCallback_BreakConnectionRequest(onBreakConnection);
            }
        }

        public Connector GetConnector(ConnectionDirection type, int index)
        {
            if (type == ConnectionDirection.Out) return null;
            else if (type == ConnectionDirection.In) return m_InputConnectors[index];
            else return null;
        }

        public List<Connector> GetAllConnectors(ConnectionDirection type)
        {
            if (type == ConnectionDirection.Out) return null;
            return m_InputConnectors;
        }
        public List<Connector> GetAllConnectors()
        {
            return m_InputConnectors;
        }

        ///////////////////////////  UI EVENTS  ///////////////////////////////

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
