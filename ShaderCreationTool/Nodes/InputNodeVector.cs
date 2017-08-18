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
    class InputNodeVector :ISCTNode, IDisposable,IInputNode
    {
        // Things to save: Position, Dimensions, Name, Values

        private Panel m_SctElement;
        private MovableObject m_Mover;
        private List<Connector> m_OutputConnectors;
        private string m_Label = string.Empty;
        private NodeCloseButtonCallback p_CloseCallback;
        private NodeInputError p_ErrorCallback;
        private TextBox m_NameTextbox;
        private NumericUpDown[] m_Numeric = new NumericUpDown[4];
        private string m_Name;
        private int m_ConnectorCount;
        private NodeType m_NodeType;
        private static bool s_ButtonsLocked = false;
        ShaderVectorVariable m_ShaderVariable;
        private string m_UniqueID;
        private static int s_InstanceCounter = 0;
        ShaderVariableType m_VarType;

        public NodeType GetNodeType() { return m_NodeType; }
        public string GetNodeID() { return m_UniqueID; }
        public ShaderVariableType GetShaderVariableType() { return m_VarType; }
        public InputNodeVector(Panel nodeTemplate, Point location)
        {

            m_ConnectorCount = 0;
            //Copy template (make local instance)
            m_SctElement = nodeTemplate.CopyAsSCTElement(true);
            m_SctElement.Location = location;
            m_SctElement.MouseDown += Panel_MouseDown;

            //Make object movable
            m_Mover = new MovableObject(m_SctElement);

            m_OutputConnectors = new List<Connector>();
            int tbCounter = 0;
            List<Control> allControlls = ControlExtensions.GetAllChildreenControls<Control>(m_SctElement).Cast<Control>().ToList();
            //allControlls.Add(m_SctElement);

            m_VarType = ShaderVariableType.Single;

            if (m_SctElement.Name.Contains("4"))
            { m_VarType = ShaderVariableType.Vector4; m_NodeType = NodeType.Input_Float4; }
            else if (m_SctElement.Name.Contains("3"))
            { m_VarType = ShaderVariableType.Vector3; m_NodeType = NodeType.Input_Float3; }
            else if (m_SctElement.Name.Contains("2"))
            { m_VarType = ShaderVariableType.Vector2; m_NodeType = NodeType.Input_Float2; }
            else
            { m_VarType = ShaderVariableType.Single; m_NodeType = NodeType.Input_Float; }

            m_UniqueID = NodeIDCreator.CreateID(GetNodeType(), s_InstanceCounter);

            int connectorCounter = 0;
            foreach (Control control in allControlls)
            {
                if (control.Name.Equals("")) continue;
                control.Click += AnyElement_Click;
                if (control is CheckBox)
                {
                    CheckBox checkBox = (CheckBox)control;
                    if (checkBox.Name.Contains(Connector.s_OutSlotSequenceID))
                    {
                        //////////////////////////// TU JEST TEN BLAD ------->>>>>>>>>>
                        Connector tempCon = new Connector(checkBox, m_VarType, this, "OUT_" + connectorCounter.ToString());
                        m_OutputConnectors.Add(tempCon);
                        connectorCounter++;
                    }
                    else
                    {
                        SCTConsole.Instance.PrintDebugLine("Checkbox name seqence error in VECTOR4 Node");
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
                    if (num.Name.Contains("X"))
                    { index = 0; m_ConnectorCount++; }
                    else if (num.Name.Contains("Y"))
                    { index = 1; m_ConnectorCount++; }
                    else if (num.Name.Contains("Z"))
                    { index = 2; m_ConnectorCount++; }
                    else if (num.Name.Contains("W"))
                    { index = 3; m_ConnectorCount++; }

                    m_Numeric[index] = num;
                }
                else if (control is TextBox)
                {
                    TextBox textBox = (TextBox)control;
                    SCTConsole.Instance.PrintDebugLine("Name is:" + textBox.Name);
                    textBox.Validated += TextBox_LostFocus;
                    textBox.KeyPress += TextBox_KeyPress;
                    m_NameTextbox = textBox;
                    tbCounter++;
                    m_Name = "SCT_UNIFORM_FLOAT4In_" + s_InstanceCounter.ToString();
                    m_NameTextbox.Text = m_Name;

                }
                else if (control is Panel)
                {
                    Panel p = (Panel)control;
                   

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
                SCTConsole.Instance.PrintDebugLine("ERROR: No Texboxes in Input Vector4 Node");
                throw new Exception("ERROR: No Texboxes in Input Vector4 Node");
            }

            s_InstanceCounter++;
            ShowNode(1);
            m_ShaderVariable = new ShaderVectorVariable(0, 0, 0, 1, m_Name);
            Bridge.SetVariable(m_ShaderVariable);
            RefreshShaderVariable();
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
            foreach (Control c in m_SctElement.Controls)
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
                RefreshShaderVariable();
                p_CloseCallback(this);
            }

        }

        private void AnyElement_Click(object sender, EventArgs e)
        {

            ((Control)sender).Focus();
            m_SctElement.BringToFront();
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

            m_Name = newText;
            m_ShaderVariable.SetName(m_Name);
            SCTConsole.Instance.PrintDebugLine("TEXT CHANGED to: " + newText);
            RefreshShaderVariable();
        }

        //////////////////// NUMERIC COMPONENTS HANDLING  ////////////////////
        private void Numeric_LostFocus(object sender, EventArgs e)
        {
            RefreshShaderVariable();

        }
        private void Numeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                RefreshShaderVariable();
            }
        }

        private void RefreshShaderVariable()
        {
            for(int i = 0; i < m_ConnectorCount;++i)
            {
                m_ShaderVariable.SetAtIndex(i, (float)m_Numeric[i].Value);
            }
        }


        private void ResetNumeric()
        {
            for (int i = 0; i < m_ConnectorCount; ++i)
            {
                m_Numeric[i].Value = 0;
            }
        }
    }
}
