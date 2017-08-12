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
        private static readonly string[] s_InputOptionsText =
        {
            "Single Float Number",
            "Vector Float 2",
            "Vector Float 3",
            "Vector Float 4",
            "Colour RGBA",
            "Texture 2D"
        };

        private List<string> m_Items;
        private NodeType m_ReturnedNodeType;
        private FunctionNodeDescription m_FuntionNodeDescription;
        private bool m_InputNodes;

        public int Selection { get; set; }
        public NodeType RequestedNodeType { get { return m_ReturnedNodeType; } }

        public FunctionNodeDescription ObtainedFunctionNodeDescription
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

        private void ItemSelectedInputNodes(object sender, EventArgs e)
        {
            Selection = ((ListBox)sender).SelectedIndex;

            m_ReturnedNodeType = (NodeType)Selection;
            this.DialogResult = DialogResult.OK;
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
            m_InputNodes = false;
            listBox1.DataSource = m_Items;
        }

       

        private void ItemSelectedFuntionNodes(object sender, EventArgs e)
        {

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

       

        


    }
}
