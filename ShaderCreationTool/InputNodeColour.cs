using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ShaderCreationTool
{


    class InputNodeColour : SCTNode, IDisposable
    {
        private Panel m_SctElement;
        private MovableObject m_Mover;
        private List<Connector> m_OutputConnectors;
        private string m_Label = string.Empty;
        private NodeCloseButtonCallback p_CloseCallback;
        private NodeInputError p_ErrorCallback;
        private TextBox m_NameTextbox;
        private NumericUpDown[] m_Numeric = new NumericUpDown[4];
        private string m_Name;
      
        ShaderVectorVariable m_ShaderVariable;
       
        private static int s_InstanceCounter = 0;


        public InputNodeColour(Panel nodeTemplate, Point location)
        {

            //Copy template (make local instance)
            m_SctElement = nodeTemplate.CopyAsSCTElement(true);
            m_SctElement.Location = location;
            m_SctElement.MouseDown += Panel_MouseDown;

            //Make object movable
            m_Mover = new MovableObject(m_SctElement);

            m_OutputConnectors = new List<Connector>();
            int tbCounter = 0;
            List<Control> allControlls = ControlExtensions.GetAllChildreenControls<Control>(m_SctElement).Cast<Control>().ToList();
            SCTConsole.Instance.PrintLine("TYCH CONTROLSOW JEST: " + allControlls.Count.ToString());
            //allControlls.Add(m_SctElement);
            foreach (Control control in allControlls)
            {
                if (control.Name.Equals("")) continue;

                if (control is CheckBox)
                {
                    CheckBox checkBox = (CheckBox)control;
                    if (checkBox.Name.Contains(Connector.s_OutSlotSequenceID))
                    {
                        Connector tempCon = new Connector(checkBox, ShaderVariableType.Vector4, this);
                        m_OutputConnectors.Add(tempCon);
                    }
                    else
                    {
                        SCTConsole.Instance.PrintLine("Checkbox name seqence error in Input Colour Node");
                    }
                }
                else if (control is NumericUpDown)
                {
                    NumericUpDown num = (NumericUpDown)control;
                    SCTConsole.Instance.PrintLine("Name is:" + num.Name);
                    num.Validated += Numeric_LostFocus;
                    num.KeyPress += Numeric_KeyPress;


                }
                else if (control is TextBox)
                {
                    TextBox textBox = (TextBox)control;
                    SCTConsole.Instance.PrintLine("NAme is:" + textBox.Name);
                    textBox.Validated += TextBox_LostFocus;
                    textBox.KeyPress += TextBox_KeyPress;
                    m_NameTextbox = textBox;
                    tbCounter++;
                    m_Name = "SCT_UNIFORM_ColourIn_" + s_InstanceCounter.ToString();
                    m_NameTextbox.Text = m_Name;

                }
                else if (control is Panel)
                {
                    Panel p = (Panel)control;
                    p.Click += AnyPanel_Click;

                }
                else if (control is Label)
                {

                    Label l = (Label)control;
                    if (l.Name.Contains("Title"))
                    {
                        l.MouseDown += TitleLabel_MouseDown;
                        l.MouseMove += TitleLabel_MouseMove;
                    }
                }
                else if (control is Button)
                {
                    Button button = (Button)control;
                    button.Click += CloseButton_Click;

                }
            }

            if (tbCounter == 0)
            {
                SCTConsole.Instance.PrintLine("ERROR: No Texboxes in Input Colour Node");
                throw new Exception("ERROR: No Texboxes in Input Colour Node");
            }

            s_InstanceCounter++;
            ShowNode(10);
            m_ShaderVariable = new ShaderVectorVariable(0, 0, 0, 1,m_Name);
            Bridge.SetVariable(m_ShaderVariable);

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

        ////// UTIL FOR ASYNC
        private async void ShowNode(int delay)
        {
            await Task.Delay(delay);
            m_SctElement.Visible = true;
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


        //////////////////// VARIABLE NAME HANDLING  ////////////////////
        private void TextBox_LostFocus(object sender, EventArgs e)
        {
            TextChanged(((TextBox)sender).Text);
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == 13)
            {
                TextChanged(((TextBox)sender).Text);
            }
        }

        private void TextChanged(string newText)
        {
            if (m_Name == newText) return;

            // Check for white characters error
            if (newText.Contains(" "))
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

            //Check for empty string error
            if (newText == string.Empty)
            {
                SCTConsole.Instance.PrintLine("Error: Variable name must contain characters!");
                m_NameTextbox.Text = m_Name;
                m_NameTextbox.Invalidate();
                if (p_ErrorCallback != null)
                {
                    p_ErrorCallback("Invalid Input: Empty varaiable names are not allowed!", this);
                }
                return;
            }

            // Check for illegal symbols error
            var regexItem = new Regex("^[a-zA-Z0-9_]*$");
            if (!regexItem.IsMatch(newText))
            {
                // error - illegal signs
                SCTConsole.Instance.PrintLine("Error: Symbols not allowed in varable name!");
                if (p_ErrorCallback != null)
                {
                    m_NameTextbox.Text = m_Name;
                    m_NameTextbox.Invalidate();
                    p_ErrorCallback("Invalid Input: Symbols other than underscore ('_') are not allowed!", this);
                }
                return;
            }

            m_Name = newText;
            m_ShaderVariable.SetName(m_Name);
            SCTConsole.Instance.PrintLine("TEXT CHANGED to: " + newText);
        }

        //////////////////// NUMERIC COMPONENTS HANDLING  ////////////////////
        private void Numeric_LostFocus(object sender, EventArgs e)
        {
            ProcessOnNumericEvent(sender);

        }
        private void Numeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                ProcessOnNumericEvent(sender);
            }
        }

        private void ProcessOnNumericEvent(object sender)
        {
            NumericUpDown num = (NumericUpDown)sender;
            int index = -1;
            if (num.Name.Contains("Red")) index = 0;
            else if (num.Name.Contains("Green")) index = 1;
            else if (num.Name.Contains("Blue")) index = 2;
            else if (num.Name.Contains("Alpha")) index = 3;
            else index = -1;

            NumericChanged(index, (float)num.Value);
        }
        private void NumericChanged(int instanceIndex, float newVal)
        {
            SCTConsole.Instance.PrintLine("numeric: " + instanceIndex.ToString() + " changed to: " + newVal.ToString());
            m_ShaderVariable.SetAtIndex(instanceIndex, newVal);
        }
    }
}
