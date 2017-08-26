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
    class AttribNodeWithSelection: IDisposable, IAttribNode
    {
        

        private Panel m_SctElement;
        private MovableObject m_Mover;
        private List<Connector> m_OutputConnectors;
        private NodeCloseButtonCallback p_CloseCallback;

        public static string [] VariablesStrings
        {
            get
            {
                var list = new List<string>();
                list.AddRange(AttribVariableStrings.O_POSITION_VAR_NAMES);
                list.AddRange(AttribVariableStrings.O_NORMAL_VAR_NAMES);
                list.AddRange(AttribVariableStrings.O_CAMERA_POS_VAR_NAMES);
                return list.ToArray();
            }
        }

        private string m_Name;
        private NodeType m_NodeType;
        private static bool s_ButtonsLocked = false;
        private string m_UniqueID;
        private static int s_InstanceCounter = 0;
        private ShaderVariableType m_VarType;

        public NodeType GetNodeType() { return m_NodeType; }
        public string GetNodeID() { return m_UniqueID; }
        public ShaderVariableType GetShaderVariableType() { return m_VarType; }



        public AttribNodeWithSelection(Panel nodeTemplate, Point location)
        {  
            //Copy template (make local instance)
            m_SctElement = nodeTemplate.CopyAsSCTElement(true);
            m_SctElement.Location = location;
            m_SctElement.MouseDown += Panel_MouseDown;

            //Make object movable
            m_Mover = new MovableObject(m_SctElement);

            m_OutputConnectors = new List<Connector>();
            List<Control> allControlls = ControlExtensions.GetAllChildreenControls<Control>(m_SctElement).Cast<Control>().ToList();
            Label title = (Label)allControlls.Find(o => o.Name.Contains("Title"));

            if (title == null) throw new Exception("Incorrect window template in Attrib Node Vector!");

            if(title.Name.Contains("vertexPos"))
            {
                m_VarType = ShaderVariableType.Vector3;
                m_NodeType = NodeType.AttribPosition;
            }
            else if (title.Name.Contains("normal"))
            {
                m_VarType = ShaderVariableType.Vector3;
                m_NodeType = NodeType.AttribNormal;
            }
            else if (title.Name.Contains("camera"))
            {
                m_VarType = ShaderVariableType.Vector4;
                m_NodeType = NodeType.AttribInput_CameraPos;
            }
            else
            {
                throw new Exception("Incorrect window template in Attrib Node Vector!");
            }


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
                        Connector tempCon = new Connector(checkBox, m_VarType, this, "OUT_" + checkBox.Text + connectorCounter.ToString());
                        m_OutputConnectors.Add(tempCon);
                        connectorCounter++;
                    }
                    else
                    {
                        SCTConsole.Instance.PrintDebugLine("Checkbox name seqence error in ATTRIB_POSITION Node");
                    }
                }
                else if (control is ComboBox)
                {
                    ComboBox comb = (ComboBox)control;
                    comb.SelectedIndexChanged += Combo_SelectedIndexChanged;
                    string[] arr;
                    
                    switch(m_NodeType)
                    {
                        case NodeType.AttribPosition: { arr = AttribVariableStrings.O_POSITION_VAR_NAMES; break; }
                        case NodeType.AttribNormal: { arr = AttribVariableStrings.O_NORMAL_VAR_NAMES; break; }
                        case NodeType.AttribInput_CameraPos: { arr = AttribVariableStrings.O_CAMERA_POS_VAR_NAMES; break; }
                        default: { arr = new string[0]; break; }
                    }
                    foreach(string s in arr)
                    {
                        if (s.Contains("World")) comb.Items.Add("World Space");
                        else if (s.Contains("Object")) comb.Items.Add("Object Space");
                        else if (s.Contains("Eye")) comb.Items.Add("Eye Space");

                    }
                    comb.SelectedIndex = 0;
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


            if (connectorCounter < 1)
            {
                SCTConsole.Instance.PrintDebugLine("ERROR: No output connectors in AttribNode_Position1");
                throw new Exception("ERROR: No output connectors in AttribNode_Position1");
            }
            s_InstanceCounter++;
            ShowNode(1);
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


        public string GetVariableName()
        {
            return m_Name;
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

        private void Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (m_NodeType == NodeType.AttribPosition)
            { m_Name = AttribVariableStrings.O_POSITION_VAR_NAMES[cb.SelectedIndex]; }
            else if (m_NodeType == NodeType.AttribNormal)
            { m_Name = AttribVariableStrings.O_NORMAL_VAR_NAMES[cb.SelectedIndex]; }
            else if (m_NodeType == NodeType.AttribInput_CameraPos)
            { m_Name = AttribVariableStrings.O_CAMERA_POS_VAR_NAMES[cb.SelectedIndex]; }
        }

    }
}
