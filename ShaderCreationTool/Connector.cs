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
    enum ConnectorType
    {
        Input,
        Output
    }

    class Connector
    {
        private static string s_InSlotSequenceID = "In_Slot";
        private static string s_OutSlotSequenceID = "Out_Slot";

        private Control m_Control;
        private ConnectorType m_ConnectorType;

        public ConnectorType Type { get { return m_ConnectorType; } }

        //TODO


        private void OnClick(object sender, EventArgs e)
        {
            if (m_Control is CheckBox)
            {
                CheckBox cb = (CheckBox)m_Control;
                cb.Checked = false;
            }
        }

        public Connector(Control control)
        {
            m_Control = control;
            m_Control.Click += OnClick;

            if (!(m_Control is CheckBox)) throw new Exception("Connector must be injected with CheckBox Control type.");

            if (m_Control.Name.Contains(s_InSlotSequenceID))
            {
                m_ConnectorType = ConnectorType.Input;
            }
            else if (m_Control.Name.Contains(s_OutSlotSequenceID))
            {
                m_ConnectorType = ConnectorType.Output;
            }
            else
            {
                throw new Exception("Slot did not contain correct sequence ID.");
            }


        }


    }
}
