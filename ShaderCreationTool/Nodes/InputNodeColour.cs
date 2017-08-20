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


    class InputNodeColour : ISCTNode,IInputNode, IDisposable
    {
        private Panel m_SctElement;
        private MovableObject m_Mover;
        private List<Connector> m_OutputConnectors;
        private string m_Label = string.Empty;
        private NodeCloseButtonCallback p_CloseCallback;
        private NodeInputError p_ErrorCallback;
        private TextBox m_NameTextbox;
        private NumericUpDown[] m_Numeric = new NumericUpDown[4];
        private Panel m_ColourPanel;
        private string m_Name;
        ShaderVectorVariable m_ShaderVariable;
        private string m_UniqueID;
        private static bool s_ButtonsLocked = false;
        private static int s_InstanceCounter = 0;

        public NodeType GetNodeType() { return NodeType.Input_Colour; }
        public string GetNodeID() { return m_UniqueID; }
        public ShaderVariableType GetShaderVariableType() { return ShaderVariableType.Vector4; }
        public InputNodeColour(Panel nodeTemplate, Point location)
        {
            m_UniqueID = NodeIDCreator.CreateID(GetNodeType(), s_InstanceCounter);
            //Copy template (make local instance)
            m_SctElement = nodeTemplate.CopyAsSCTElement(true);
            m_SctElement.Location = location;
            m_SctElement.MouseDown += Panel_MouseDown;
   

            //Make object movable
            m_Mover = new MovableObject(m_SctElement);

            m_OutputConnectors = new List<Connector>();
            int tbCounter = 0;
            List<Control> allControlls = ControlExtensions.GetAllChildreenControls<Control>(m_SctElement).Cast<Control>().ToList();
            int connectorCounter = 0;
            foreach (Control control in allControlls)
            {
                if (control.Name.Equals("")) continue;

                control.Click += AnyElement_Click;
               // control.BackColor = Color.FromArgb(255, 100, 100, 100);
                if (control is CheckBox)
                {
                    CheckBox checkBox = (CheckBox)control;
                    if (checkBox.Name.Contains(Connector.s_OutSlotSequenceID))
                    {
                        Connector tempCon = new Connector(checkBox, ShaderVariableType.Vector4, this, "OUT_" + checkBox.Text + connectorCounter.ToString());
                        connectorCounter++;
                        m_OutputConnectors.Add(tempCon);
                    }
                    else
                    {
                        SCTConsole.Instance.PrintDebugLine("Checkbox name seqence error in Input Colour Node");
                    }
                }
                else if (control is NumericUpDown)
                {
                    NumericUpDown num = (NumericUpDown)control;
                    SCTConsole.Instance.PrintDebugLine("Name is:" + num.Name);
                    num.Validated += Numeric_LostFocus;
                    num.KeyPress += Numeric_KeyPress;

                    // Slot sorting
                    int index = -1;
                    if (num.Name.Contains("Red")) index = 0;
                    else if (num.Name.Contains("Green")) index = 1;
                    else if (num.Name.Contains("Blue")) index = 2;
                    else if (num.Name.Contains("Alpha")) index = 3;

                    m_Numeric[index] = num;
                }
                else if (control is TextBox)
                {
                    TextBox textBox = (TextBox)control;
                    SCTConsole.Instance.PrintDebugLine("NAme is:" + textBox.Name);
                    textBox.Validated += TextBox_LostFocus;
                    textBox.KeyPress += TextBox_KeyPress;
                    m_NameTextbox = textBox;
                    tbCounter++;
                    m_Name = "SCT_UNIFORM_ColourIn_" + s_InstanceCounter.ToString();
                    m_NameTextbox.Text = m_Name;
                    VaribaleNameGuard.ChangeName("", m_Name);

                }
                else if (control is Panel)
                {
                    Panel p = (Panel)control;
                    
                    if (p.Name.Contains("ColInd"))
                    {
                        m_ColourPanel = p;
                        m_ColourPanel.Click += ColourPanel_Click;
                    }

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
                SCTConsole.Instance.PrintDebugLine("ERROR: No Texboxes in Input Colour Node");
                throw new Exception("ERROR: No Texboxes in Input Colour Node");
            }

            s_InstanceCounter++;
            ShowNode(1);
           
            m_ShaderVariable = new ShaderVectorVariable(0, 0, 0, 1,m_Name);
            Bridge.SetVariable(m_ShaderVariable);
            SetShaderVariableFromNumeric();
            
        }
        public string GetVariableName()
        {
            return m_Name;
        }
        static public void LockButtons()
        {
            s_ButtonsLocked = true;
        }

        static public void UnlockButtons()
        {
            s_ButtonsLocked = false;
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
            //new code
            foreach(Control c in m_SctElement.Controls)
            {
                c.Dispose();
            }
        }

        ////// UTIL FOR ASYNC
        private async void ShowNode(int delay)
        {
            await Task.Delay(delay);
            m_SctElement.Visible = true;
            m_SctElement.Update();
        }

        ////////////////// UI EVENTS ////////////////
        private void CloseButton_Click(object sender, EventArgs e)
        {
            if (s_ButtonsLocked) return;
            if (p_CloseCallback != null)
            {
                ResetNumeric();
                SetShaderVariableFromNumeric();
                p_CloseCallback(this);
            }

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



        private void ColourPanel_Click(object sender, EventArgs e)
        {

            ColorDialog cd = new ColorDialog();
            cd.Color = Color.FromArgb(
                (int)(m_Numeric[3].Value * 255),
                (int)(m_Numeric[0].Value * 255),
                (int)(m_Numeric[1].Value * 255),
                (int)(m_Numeric[2].Value * 255));

            DialogResult result = cd.ShowDialog();
            if (result == DialogResult.OK)
            {
               
                m_Numeric[0].Value = (decimal) ((float)(cd.Color.R) / 255.0f);
                m_Numeric[1].Value = (decimal)((float)(cd.Color.G) / 255.0f);
                m_Numeric[2].Value = (decimal)((float)(cd.Color.B) / 255.0f);
                m_Numeric[3].Value = (decimal)((float)(cd.Color.A) / 255.0f);
                SetShaderVariableFromNumeric();

            }
           
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
                SCTConsole.Instance.PrintDebugLine("Error: White spaces not allowed in varable name!");
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
                SCTConsole.Instance.PrintDebugLine("Error: Variable name must contain characters!");
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
                SCTConsole.Instance.PrintDebugLine("Error: Symbols not allowed in varable name!");
                if (p_ErrorCallback != null)
                {
                    m_NameTextbox.Text = m_Name;
                    m_NameTextbox.Invalidate();
                    p_ErrorCallback("Invalid Input: Symbols other than underscore ('_') are not allowed!", this);
                }
                return;
            }

            ChangeNameStateMessage cnm = VaribaleNameGuard.ChangeName(m_Name, newText);

            if (cnm == ChangeNameStateMessage.ERROR_NameAlreadyExists)
            {
                SCTConsole.Instance.PrintDebugLine("Error: Name already exists!");
                if (p_ErrorCallback != null)
                {
                    m_NameTextbox.Text = m_Name;
                    m_NameTextbox.Invalidate();
                    p_ErrorCallback("Invalid Input: NAME ALREADY EXISTS", this);
                }
                return;
            }
            ResetNumeric();
            SetShaderVariableFromNumeric();
            m_Name = newText;

            m_ShaderVariable.SetName(m_Name);
            SCTConsole.Instance.PrintDebugLine("TEXT CHANGED to: " + newText);
            SetShaderVariableFromNumeric();
        }

        //////////////////// NUMERIC COMPONENTS HANDLING  ////////////////////
        private void Numeric_LostFocus(object sender, EventArgs e)
        {
            SetShaderVariableFromNumeric();

        }
        private void Numeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SetShaderVariableFromNumeric();
            }
        }

        private void SetShaderVariableFromNumeric()
        {
            m_ShaderVariable.Set((float)m_Numeric[0].Value, (float)m_Numeric[1].Value,
                (float)m_Numeric[2].Value,(float)m_Numeric[3].Value);

            
            m_ColourPanel.BackColor = Color.FromArgb(
                255,
                (int)(((m_Numeric[0].Value>1)?1:m_Numeric[0].Value)*255),
               (int)(((m_Numeric[1].Value > 1) ? 1 : m_Numeric[1].Value) * 255), 
                (int)(((m_Numeric[3].Value > 1) ? 1 : m_Numeric[2].Value) * 255));
        }

        private void ResetNumeric()
        {
            m_Numeric[0].Value = 0;
            m_Numeric[1].Value = 0;
            m_Numeric[2].Value = 0;
            m_Numeric[3].Value = 1;
        }

    }
}
