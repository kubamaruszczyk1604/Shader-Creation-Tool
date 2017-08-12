using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShaderCreationTool.Forms
{
    public partial class SelectNodeForm : Form
    {
        private List<string> m_Items;

        public int Selection { get; set; }
        public SelectNodeForm()
        {
            m_Items = new List<string>();
            m_Items.Add("Blynn-Phong Lighting");
            m_Items.Add("Split");
            m_Items.Add("Multiply");
            InitializeComponent();
            listBox1.DataSource = m_Items;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Selection = ((ListBox)sender).SelectedIndex;
           // SCTConsole.Instance.PrintLine("Selected item: " + Selection.ToString());
            this.DialogResult = DialogResult.OK;
        }
    }
}
