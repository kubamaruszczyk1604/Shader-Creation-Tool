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
    class InputNodeColour: SCTNode, IDisposable
    {
        private Panel m_SctElement;
        private MovableObject m_Mover;
        private List<Connector> m_OutputConnectors;
        private string m_Label = string.Empty;
        private NodeCloseButtonCallback p_CloseCallback;

        public InputNodeColour(Panel nodeTemplate, Point location)
        {
            //Copy template (make local instance)
            m_SctElement = nodeTemplate.CopyAsSCTElement(true);
            m_SctElement.Visible = true;
            m_SctElement.Location = location;
            m_SctElement.MouseDown += Panel_MouseDown;


            //Make object movable
            m_Mover = new MovableObject(m_SctElement);

            //Find template tick boxes
            List<CheckBox> boxes = ControlExtensions.GetAllChildreenControls<CheckBox>(m_SctElement).Cast<CheckBox>().ToList();

            //Create connectors
            m_OutputConnectors = new List<Connector>();
            for (int i = 0; i < boxes.Count; ++i)
            {
                if (boxes[i].Name.Contains(Connector.s_OutSlotSequenceID))
                {
                   
                    CheckBox cd = boxes[i];
                    Connector tempCon = new Connector(cd, ShaderVariableType.Vector4, this);
                    m_OutputConnectors.Add(tempCon);

                }
                else
                {
                    SCTConsole.Instance.PrintLine("Checkbox name seqence error in Input Colour Node");
                }
            }
            

            // Set node title
            List<Label> labels = ControlExtensions.GetAllChildreenControls<Label>(m_SctElement).Cast<Label>().ToList();
            foreach (Label l in labels)
            {
                if (l.Name.Contains("Title"))
                {
                    l.MouseDown += TitleLabel_MouseDown;
                    l.MouseMove += TitleLabel_MouseMove;
                    break;
                }
            }

            //Close Button click setup
            List<Button> buttons = ControlExtensions.GetAllChildreenControls<Button>(m_SctElement).Cast<Button>().ToList();
            buttons[0].Click += CloseButton_Click;

        }



        public void AddOnMovedCallback(ObjectMovedCallback onMovedCallback)
        {
            m_Mover.AddObjectMovedEventListener(onMovedCallback);
        }

        public void AddOnBeginConnectionCallback(BeginConnectionCallback onBeginConnection)
        {
            foreach (Connector c in m_OutputConnectors)
            {
                c.AddCallback_BeginConnectionRequest(onBeginConnection);
            }
        }

        public void AddOnBreakConnectionCallback(BreakConnectionCallback onBreakConnection)
        {
            foreach (Connector c in m_OutputConnectors)
            {
                c.AddCallback_BreakConnectionRequest(onBreakConnection);
            }
        }
    
        public void AddOnCloseCallback(NodeCloseButtonCallback callback)
        {
            p_CloseCallback += callback;
        }

        public Connector GetConnector(ConnectionDirection type, int index)
        {
            if (type == ConnectionDirection.Out) return m_OutputConnectors[index];
            else return null;
        }

        public List<Connector> GetAllConnectors(ConnectionDirection type)
        {
            if (type == ConnectionDirection.Out) return m_OutputConnectors;
            else return null;
        }
        public List<Connector> GetAllConnectors()
        {
            return m_OutputConnectors;
        }

        public void Dispose()
        {
            List<Connector> connectors = GetAllConnectors();
            foreach (Connector c in connectors)
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
