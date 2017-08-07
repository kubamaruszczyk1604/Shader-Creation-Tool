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
        Destination,
        Source
    }

    delegate void BeginConnectionCallback(Connector sender);
    delegate void BreakConnectionCallback(Connector sender);

    class Connector
    {
        // PRIVATE (MEMBERS)
        private const string s_InSlotSequenceID = "In_Slot";
        private const string s_OutSlotSequenceID = "Out_Slot";

        private CheckBox m_Control;
        private ConnectorType m_ConnectorType;
        private BeginConnectionCallback m_BeginConnectionCallback;
        private BreakConnectionCallback m_BreakConnectionCallback;
        private Connection p_ParentConnection;

        private ShaderVariableType m_VariableType;

        static private Connector s_PreviouslyClickedConnector;

        // PRIVATE (METHODS)
        private void OnClick(object sender, EventArgs e)
        { 
            if (!Connected)
            {
                m_Control.Checked = false;
                if (m_BeginConnectionCallback != null)
                { 
                    m_BeginConnectionCallback(this);
                }


            }
            else // then click means - disconnect
            {
                m_Control.Checked = true;
                if (m_BreakConnectionCallback != null)
                {
                    m_BreakConnectionCallback(this);
                }
               // Disconnect();
               
            }
            s_PreviouslyClickedConnector = this;
        }
        
        /////////////////////////////////////////////////////////  PUBLIC  /////////////////////////////////////////////////

        // PROPERTIES
        public ConnectorType Type { get { return m_ConnectorType; } }
        public bool Connected { get { return (p_ParentConnection==null)?false:true; } }
        public Control WinFormControl { get { return m_Control; } }
        public Connection ParentConnection { get { return p_ParentConnection; } }
       
        public Connector(CheckBox control,ShaderVariableType variableType)
        {
            m_Control = control;
            m_Control.Click += OnClick;
            m_Control.Checked = false;
            m_VariableType = variableType;

            if (m_Control.Name.Contains(s_InSlotSequenceID))
            {
                m_ConnectorType = ConnectorType.Destination;
            }
            else if (m_Control.Name.Contains(s_OutSlotSequenceID))
            {
                m_ConnectorType = ConnectorType.Source;
            }
            else
            {
                throw new Exception("Slot did not contain correct sequence ID.");
            }
        }

        
         /// <summary>
         /// Registered method will be called when this connector is not connected and is clicked on.
         /// </summary>
         /// <param name="method">Method to be registered</param>
        public void AddCallback_BeginConnectionRequest(BeginConnectionCallback method)
        {
            m_BeginConnectionCallback += method;
        }

        public void AddCallback_BreakConnectionRequest(BreakConnectionCallback method)
        {
            m_BreakConnectionCallback += method;
        }

        public void SetAsConnected(Connection connection)
        {
            p_ParentConnection = connection;
            if (p_ParentConnection != null)
            {
                m_Control.Checked = true;
            }
        }      

        public void Disconnect()
        {
            m_Control.Checked = false;
            p_ParentConnection = null; 
        }

        static public Connector GetPreviouslyClickedConnector()
        {
            return s_PreviouslyClickedConnector;
        }

    }
}
