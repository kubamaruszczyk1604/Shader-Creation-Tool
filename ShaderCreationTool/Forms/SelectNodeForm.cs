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

     

        private List<string> m_Items;
        private NodeType m_ReturnedNodeType;
        private FunctionNodeDescription m_FuntionNodeDescription;
        private bool m_InputNodes;
        private bool m_ReadingOK;

        public int Selection { get; set; }
        public NodeType RequestedInputNodeType { get { return m_ReturnedNodeType; } }

        public FunctionNodeDescription RequestedFunctionNodeDescription
        {
            get { return m_FuntionNodeDescription; }
        }

        // for input nodes - predefined set of options
        public SelectNodeForm()
        {
            m_Items = s_InputOptionsText.ToList();
            m_InputNodes = true;
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
                m_InputNodes = false;
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
            m_ReturnedNodeType = (NodeType)Selection;
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
            if (m_InputNodes)
            {
                ItemSelectedInputNodes(sender, e);
            }
            else 
            {
                ItemSelectedFuntionNodes(sender, e);
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
            if (!m_ReadingOK && !m_InputNodes)
            {
                //this.Hide();
                MessageBox.Show("Error While reading XML FILE:" + FunctionNodeConfigMgr.LastStatus, "XML ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
    }
}
