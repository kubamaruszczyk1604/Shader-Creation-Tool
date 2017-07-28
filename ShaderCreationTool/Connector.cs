using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace ShaderCreationTool
{
    class Connector
    {
        private Control m_Control;
        //TODO

        private void OnClick(object sender, EventArgs e)
        {
            if(m_Control is CheckBox)
            {
                CheckBox cb = (CheckBox)m_Control;
                cb.Checked = false;
            }
        }
        public Connector(Control control)
        {
            m_Control = control;
            m_Control.Click += OnClick;
        }
    }
}
