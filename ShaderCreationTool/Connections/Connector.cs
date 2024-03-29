﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace ShaderCreationTool
{
    public enum ConnectionDirection
    {
        In,
        Out
    }

    delegate bool BeginConnectionCallback(Connector sender);
    delegate void BreakConnectionCallback(Connector sender);

    class Connector
    {
        // PRIVATE (MEMBERS)
        public const string s_InSlotSequenceID = "In_Slot";
        public const string s_OutSlotSequenceID = "Out_Slot";

        private CheckBox m_Control;
        private ConnectionDirection m_ConnectorType;
        private BeginConnectionCallback m_BeginConnectionCallback;
        private BreakConnectionCallback m_BreakConnectionCallback;
        private Connection p_ParentConnection;

        private ShaderVariableType m_VariableType;
        private string m_VariableName;
        private ISCTNode p_ParentNode;
        private Color m_StandardColour;
        private string m_LocalID;
        private ShaderVariableDescription m_ShaderVariableDescription;
        static private Connector s_PreviouslyClickedConnector;
        static private bool m_AllConnectorsLocked = false;

        // PRIVATE (METHODS)
        private void OnClick(object sender, EventArgs e)
        {

            SCTConsole.Instance.PrintDebugLine("Connector with local ID: " + this.LocalID);
            SCTConsole.Instance.PrintDebugLine("PARENT ID: " + p_ParentNode.GetNodeID());

            if (!Connected)
            {
                m_Control.Checked = false;
                if (m_AllConnectorsLocked) return;
                if (m_BeginConnectionCallback != null)
                { 
                   if( m_BeginConnectionCallback(this))
                    {
                        s_PreviouslyClickedConnector = this;
                    }
                }
               

            }
            else // then click means - disconnect
            {
                m_Control.Checked = true;
                if (m_AllConnectorsLocked) return;
                if (m_BreakConnectionCallback != null)
                {
                    m_BreakConnectionCallback(this);
                }
               // Disconnect();       
            }
        }
        
        /////////////////////////////////////////////////////////  PUBLIC  /////////////////////////////////////////////////

        // PROPERTIES
        public ConnectionDirection DirectionType { get { return m_ConnectorType; } }
        public ShaderVariableType VariableType { get { return m_VariableType; } }
        public string VariableName { get { return m_VariableName; } }
        public bool Connected { get { return (p_ParentConnection==null)?false:true; } }
        public Control WinFormControl { get { return m_Control; } }
        public Connection ParentConnection { get { return p_ParentConnection; } }
        public ISCTNode ParentNode { get { return p_ParentNode; } }
        public string LocalID { get { return m_LocalID; } }
        public bool HasShaderVariableDescription { get { return (m_ShaderVariableDescription == null) ? false : true; } }
        public ShaderVariableDescription UsedShaderVariableDescription { get { return m_ShaderVariableDescription; } }
       
        static Connector Load(CheckBox control, ISCTNode parentNode,StreamReader reader)
        {
            return null;
        }

        public Connector(CheckBox control, ShaderVariableType variableType,ISCTNode parentNode, string localID)
        {
            m_LocalID = localID;
            p_ParentNode = parentNode;
            m_Control = control;
            m_Control.Click += OnClick;
            m_Control.Checked = false;
            m_VariableType = variableType;
            m_VariableName = control.Text;
            m_StandardColour = m_Control.ForeColor;

            if (m_Control.Name.Contains(s_InSlotSequenceID))
            {
                m_ConnectorType = ConnectionDirection.In;
            }
            else if (m_Control.Name.Contains(s_OutSlotSequenceID))
            {
                m_ConnectorType = ConnectionDirection.Out;
            }
            else
            {
                throw new Exception("Slot did not contain correct sequence ID.");
            }
        }

        public void SetShaderVaribaleDescription(ShaderVariableDescription desc)
        {
            m_ShaderVariableDescription = desc;
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

        public void SetBackHighlighted()
        {
            m_Control.BackColor = Color.Coral;
            m_Control.ForeColor = Color.White;
        }

        public void DisableBackHighlighted()
        {
            m_Control.BackColor = Color.Transparent;
            m_Control.ForeColor = m_StandardColour;
        }

        static public Connector GetPreviouslyClickedConnector()
        {
            return s_PreviouslyClickedConnector;
        }

        public static void LockAllConnectors()
        {
            m_AllConnectorsLocked = true;
        }

        public static void UnlockAllConnectors()
        {
            m_AllConnectorsLocked = false;
        }

    }
}
