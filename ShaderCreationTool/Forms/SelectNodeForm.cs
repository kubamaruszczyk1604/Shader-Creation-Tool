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
  
    public partial class SelectNodeForm : Form
    {
        enum DisplayedNodeType { Input,Function,Attrib };

        private static readonly string[] s_InputOptionsText =
        {
            "Single Float Number",
            "Vector Float 2",
            "Vector Float 3",
            "Vector Float 4",
            "Colour RGBA",
            "Texture 2D",
        };

        private static readonly string[] s_AttribOptionsText =
        {
            "Position",
            "Normal",
            "Texture UVs",
            "TangentVector"
        };

        private static NodeType StringToNodeType(string str)
        {
            switch(str)
            {
                case "Single Float Number": { return NodeType.Input_Float; }
                case "Vector Float 2": { return NodeType.Input_Float2; }
                case "Vector Float 3": { return NodeType.Input_Float3; }
                case "Vector Float 4": { return NodeType.Input_Float4; }
                case "Colour RGBA": { return NodeType.Input_Colour; }
                case "Texture 2D": { return NodeType.Input_Texture2D; }
                case "Position": { return NodeType.AttribPosition; }
                case "Normal": { return NodeType.AttribNormal; }
                case "Texture UVs": { return NodeType.AttribUVs; }
                case "TangentVector": { return NodeType.AttribTangent; }
                default: { return NodeType.Target; } // error condition

            }
        }

       

        private List<string> m_Items;
        private NodeType m_ReturnedNodeType;
        private FunctionNodeDescription m_FuntionNodeDescription;
      //  private bool m_InputNodes;
        private DisplayedNodeType m_DisplayedType;
        private bool m_ReadingOK;

        public int Selection { get; set; }
        public NodeType RequestedInputNodeType { get { return m_ReturnedNodeType; } }

        public FunctionNodeDescription RequestedFunctionNodeDescription
        {
            get { return m_FuntionNodeDescription; }
        }

        // for input nodes - predefined set of options
        public SelectNodeForm(bool input)
        {
            if (input)
            {
                m_Items = s_InputOptionsText.ToList();
                m_DisplayedType = DisplayedNodeType.Input;
            }
            else
            {
                m_Items = s_AttribOptionsText.ToList();
                m_DisplayedType = DisplayedNodeType.Attrib;
            }
            InitializeComponent();
            listBox1.DataSource = m_Items;

        }


        // Read set of options from XML external file (use path)
        public SelectNodeForm(string listPath)
        {
            InitializeComponent();
            if (FunctionNodeConfigMgr.ReadNodesFromFile(listPath))
            {
                m_Items = FunctionNodeConfigMgr.NodeList;
                m_ReturnedNodeType = NodeType.Funtion;
                m_DisplayedType = DisplayedNodeType.Function;
                listBox1.DataSource = m_Items;
                m_ReadingOK = true;
               
            }
            else
            {
                m_ReadingOK = false;
                this.DialogResult = DialogResult.Abort;
            }

           
        }


        private void ItemSelectedInputNodes(object sender, EventArgs e)
        {
          
            Selection = ((ListBox)sender).SelectedIndex;
            m_ReturnedNodeType = StringToNodeType((string)((ListBox)sender).SelectedValue);
            this.DialogResult = DialogResult.OK;
         
        }

        private void ItemSelectedAttribNodes(object sender, EventArgs e)
        {

            Selection = ((ListBox)sender).SelectedIndex;
            m_ReturnedNodeType = StringToNodeType((string)((ListBox)sender).SelectedValue);
            this.DialogResult = DialogResult.OK;

        }

        private void ItemSelectedFuntionNodes(object sender, EventArgs e)
        {
            if (!m_ReadingOK)
            {
                this.DialogResult = DialogResult.Abort;
                return;
            }
            Selection = ((ListBox)sender).SelectedIndex;
            m_FuntionNodeDescription = FunctionNodeConfigMgr.GetFunctionNodeDescription(Selection);
            this.DialogResult = DialogResult.OK;
        }






        /////////////////////////////  DIRECT UI EVENTS ///////////////////////////////
        private void OnInput(object sender, EventArgs e)
        {
            if (m_DisplayedType == DisplayedNodeType.Input)
            {
                ItemSelectedInputNodes(sender, e);
            }
            else if (m_DisplayedType == DisplayedNodeType.Function)
            {
                ItemSelectedFuntionNodes(sender, e);
            }
            else if (m_DisplayedType == DisplayedNodeType.Attrib)
            {
                ItemSelectedAttribNodes(sender, e);
            }
        }
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            OnInput(sender, e);
        }

        private void listBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)//if enter pressed
            {
                OnInput(sender, e);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SelectNodeForm_Shown(object sender, EventArgs e)
        {
            if (!m_ReadingOK && m_DisplayedType == DisplayedNodeType.Function)
            {
                //this.Hide();
                MessageBox.Show("Error While reading XML FILE:" + FunctionNodeConfigMgr.LastStatus, "XML ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
    }
}
