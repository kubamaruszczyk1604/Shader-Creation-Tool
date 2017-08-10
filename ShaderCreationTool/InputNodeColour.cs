using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ShaderCreationTool
{
 

    class InputNodeColour: SCTNode, IDisposable
    {
        private Panel m_SctElement;
        private MovableObject m_Mover;
        private List<Connector> m_OutputConnectors;
        private string m_Label = string.Empty;
        private NodeCloseButtonCallback p_CloseCallback;
        private NodeInputError p_ErrorCallback;
        private TextBox m_NameTextbox;
        private string m_Name; 

        public InputNodeColour(Panel nodeTemplate, Point location)
        {
            //Copy template (make local instance)
            m_SctElement = nodeTemplate.CopyAsSCTElement(true);
            m_SctElement.Visible = true;
            m_SctElement.Location = location;
            m_SctElement.MouseDown += Panel_MouseDown;


            //Make object movable
            m_Mover = new MovableObject(m_SctElement);

            //Create connectors
            List<CheckBox> boxes = ControlExtensions.GetAllChildreenControls<CheckBox>(m_SctElement).Cast<CheckBox>().ToList();
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

            //SetUp text box events
            List<TextBox> textBoxes = ControlExtensions.GetAllChildreenControls<TextBox>(m_SctElement).Cast<TextBox>().ToList();
            if(textBoxes.Count == 0)
            {
                SCTConsole.Instance.PrintLine("ERROR: No Texboxes in Input Colour Node");
                throw new Exception("ERROR: No Texboxes in Input Colour Node");
            }
            else
            {
                textBoxes[0].LostFocus += TextBoxLostFocus_TextChanged;
                textBoxes[0].KeyPress += TextBox_KeyPress;
                m_NameTextbox = textBoxes[0];
            }

            List<Panel> panels = ControlExtensions.GetAllChildreenControls<Panel>(m_SctElement).Cast<Panel>().ToList();
            foreach(Panel p in panels)
            {
                p.Click += AnyPanel_Click;
            }

            // Set node title on click events
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

            m_Name = string.Empty;

        }

        ////////////////////////  ADD CALLBACK METHODS ///////////////

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

        public void AddInputErrorCallback(NodeInputError callback)
        {
            p_ErrorCallback += callback;
        }

        public Connector GetConnector(ConnectionDirection type, int index)
        {
            if (type == ConnectionDirection.Out) return m_OutputConnectors[index];
            else return null;
        }



        //////////////////// PUBLIC UTIL METHODS /////////////////////////

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

        private void AnyPanel_Click(object sender, EventArgs e)
        {

            ((Control)sender).Focus();

        }

        private void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            m_SctElement.BringToFront();
            m_SctElement.Focus();
        }


        private void TitleLabel_MouseDown(object sender, MouseEventArgs e)
        {
            m_Mover.MoveControlMouseCapture(m_SctElement, e);
        }

        private void TitleLabel_MouseMove(object sender, MouseEventArgs e)
        {
            m_Mover.MoveControlMouseMove(m_SctElement, e);
        }

        private void TextBoxLostFocus_TextChanged(object sender, EventArgs e)
        {
            TextChanged(((TextBox)sender).Text);
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {

            if(e.KeyChar == 13)
            {
                TextChanged(((TextBox)sender).Text);
            }
        }

        private void TextChanged(string newText)
        {
            if (m_Name == newText) return;
         
            // Check for white characters error
            if(newText.Contains(" "))
            {
                // error - spaces
                SCTConsole.Instance.PrintLine("Error: White spaces not allowed in varable name!");
                m_NameTextbox.Text = m_Name; // use old name
                m_NameTextbox.Invalidate();
                if (p_ErrorCallback != null)
                {
                    p_ErrorCallback("Invalid Input: White Spaces not allowed!", this);    
                }
                return;
            }

            if (newText == string.Empty)
            {
                
                SCTConsole.Instance.PrintLine("Error: Variable name must contain characters!");
                if (m_Name == string.Empty) { m_Name = "Variable_Number"; }
                m_NameTextbox.Text = m_Name;
                m_NameTextbox.Invalidate();
                if (p_ErrorCallback != null)
                {
                    p_ErrorCallback("Invalid Input: Empty varaiable names are not allowed!", this);
                }
                return;
            }

            var regexItem = new Regex("^[a-zA-Z0-9_]*$");
            if (!regexItem.IsMatch(newText))
            {
                // error - illegal signs
                SCTConsole.Instance.PrintLine("Error: Symbols not allowed in varable name!");
                if (p_ErrorCallback != null)
                {
                    m_NameTextbox.Text = m_Name;
                    m_NameTextbox.Invalidate();
                    p_ErrorCallback("Invalid Input: Symbols other than '_' are not allowed!", this);
                }
                return;
            }

            m_Name = newText;
            SCTConsole.Instance.PrintLine("TEXT CHANGED to: " + newText);
        }
    }
}
