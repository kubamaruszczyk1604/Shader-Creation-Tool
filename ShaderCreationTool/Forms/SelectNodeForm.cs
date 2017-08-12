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
        private List<string> m_Items;
        private NodeType m_ReturnedNodeType;
        private FunctionNodeDescription m_FuntionNodeDescription;

        public int Selection { get; set; }
        public NodeType RequestedNodeType { get { return m_ReturnedNodeType; } }
        public FunctionNodeDescription ObtainedFunctionNodeDescription
        { get { return m_FuntionNodeDescription; } }

        // for input nodes - predefined set of options
        public SelectNodeForm()
        {
            m_Items = new List<string>();
            m_Items.Add("Blynn-Phong Lighting");
            m_Items.Add("Split");
            m_Items.Add("Multiply");
            m_ReturnedNodeType = NodeType.Input;
            InitializeComponent();
            listBox1.DataSource = m_Items;
        }
        // Read set of options from XML external file (use path)
        public SelectNodeForm(string listPath)
        {
            m_Items = new List<string>();
            m_Items.Add("Test1");
            m_Items.Add("Test2");
            m_Items.Add("Test3");
            m_ReturnedNodeType = NodeType.Funtion;
            InitializeComponent();
            listBox1.DataSource = m_Items;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(m_ReturnedNodeType == NodeType.Input)
            {
                ItemSelectedInputNodes(sender, e);
            }
            else if(m_ReturnedNodeType == NodeType.Funtion)
            {
                ItemSelectedFuntionNodes(sender, e);
            }
        }

        private void ItemSelectedInputNodes(object sender, EventArgs e)
        {
            Selection = ((ListBox)sender).SelectedIndex;
            // SCTConsole.Instance.PrintLine("Selected item: " + Selection.ToString());
            this.DialogResult = DialogResult.OK;
        }

        private void ItemSelectedFuntionNodes(object sender, EventArgs e)
        {

        }
       
    }
}
