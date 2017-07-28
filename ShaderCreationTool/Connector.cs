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
        // PRIVATE
        private static string s_InSlotSequenceID = "In_Slot";
        private static string s_OutSlotSequenceID = "Out_Slot";

        private CheckBox m_Control;
        private ConnectorType m_ConnectorType;
        private bool m_ConnectedFlag;
    


        // PRIVATE (METHODS)
        private void OnClick(object sender, EventArgs e)
        { 
            if (!m_ConnectedFlag)
            {
   
            }
        }


        // PROPERTIES (PUBLIC)
        public ConnectorType Type { get { return m_ConnectorType; } }
        public bool Connected { get { return m_ConnectedFlag; } }


     
        // PUBLIC
        public Connector(CheckBox control)
        {
            m_Control = control;
            m_Control.Click += OnClick;
            m_ConnectedFlag = false;

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
