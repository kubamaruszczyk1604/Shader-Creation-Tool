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
    class FrameBufferNode
    {
        private Panel m_SctElement;
        private MovableObject m_Mover;
        private List<Connector> m_InputConnectors;
        private string m_Label = string.Empty;
        private NodeCloseButtonCallback p_CloseCallback;

        public FrameBufferNode(Panel windowControl, Point location)
        {
            //Copy template (make local instance)
            m_SctElement = windowControl;
          //m_SctElement.MouseDown += Panel_MouseDown;

            //Make object movable
            m_Mover = new MovableObject(m_SctElement);

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
                    ShaderVariableType varType;
                    if (tempBox.Name.Contains("Colour"))
                    {
                        varType = ShaderVariableType.Vector4;
                    }
                    else if (tempBox.Name.Contains("Depth"))
                    {
                        varType = ShaderVariableType.Single;
                    }

                   // Connector tempCon = new Connector(tempBox, var, this);
                   // m_InputConnectors.Add(tempCon);
                }
                else
                {
                    SCTConsole.Instance.PrintLine("WRONG SEQUENCE IN CHECKBOX NAME: FRAME BUFFER NODE\n");
                    throw new Exception("WRONG SEQUENCE");
                }
            }

        }

    }
}
