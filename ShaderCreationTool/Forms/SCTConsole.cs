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
    public partial class SCTConsole : Form
    {


        private SCTConsole()
        {
            InitializeComponent();
            
        }
        public void PrintDebugLine(String text)
        { 
            ConsoleTextBox.AppendText("DEBUG: " + text + "\r\n");
        }

        public void PrintLine(String text)
        {
            ConsoleTextBox.AppendText(text + "\r\n");

        }


        private void ConsoleForm_Load(object sender, EventArgs e)
        {
            ConsoleTextBox.AppendText("\r\n");
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        static public SCTConsole Instance = new SCTConsole();
    }


  
}
