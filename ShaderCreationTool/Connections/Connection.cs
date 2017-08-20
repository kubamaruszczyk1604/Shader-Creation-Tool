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
using System.Reflection;


namespace ShaderCreationTool
{
    class Connection: IDisposable
    {
        private Connector m_pSource;
        private Connector m_pDestination;
        private ConnectionLine m_Line;
        private bool m_ConnectedFlag;
        private Control p_DrawOnControl;
        private bool m_IsDirectInputConnection;
        private string m_OutVariableName;

        ////////////////////////////////////////////  PUBLIC  ////////////////////////////////////////////

        public bool IsDirectInputConnection { get { return m_IsDirectInputConnection;} }
        public Connector SourceConnector { get { return m_pSource; } }
        public Connector DestinationConnector { get { return m_pDestination; } }
        public string OutVariableName { get { return m_OutVariableName; } }
        public string Info
        {
            get
            {
                return  "Connection of: " + m_pSource.ParentNode.GetNodeID() + " and " + m_pDestination.ParentNode.GetNodeID();
            }
        }

       public static bool CheckConnectionValidity(Connector a, Connector b)
        {
            if (a.DirectionType == b.DirectionType)
            {
                return false;
            }
            if(a.VariableType != b.VariableType)
            {
                return false;
            }

            if(a.ParentNode == b.ParentNode)
            {
                return false;
            }

            return true;
        }

        public bool IsConnected { get { return m_ConnectedFlag; } } 
        public Control DrawOnControl { get { return p_DrawOnControl; } }


        public Connection(Connector a, Connector b, Control drawOn)
        {
            m_IsDirectInputConnection = false;
            if(a.DirectionType == b.DirectionType)
            {
                throw new Exception("Both a and b have the same direction");
            }

            m_Line = new ConnectionLine(drawOn);
            m_pSource = (a.DirectionType == ConnectionDirection.Out) ? a : b;
            m_pDestination = (m_pSource.DirectionType == a.DirectionType) ? b : a;
            m_ConnectedFlag = true;
            a.SetAsConnected(this);
            b.SetAsConnected(this);
            p_DrawOnControl = drawOn;
            if (a.ParentNode is IInputNode) m_IsDirectInputConnection = true;
            if (b.ParentNode is IInputNode) m_IsDirectInputConnection = true;

            RefreshoutVarName();

        }

        public void RefreshoutVarName()
        {
            if (m_pSource.ParentNode is IInputNode)
            {
                ISCTNode sourceNode = m_pSource.ParentNode;
                IInputNode temp = (IInputNode)sourceNode;
                m_OutVariableName = temp.GetVariableName();
            }
            else if (m_pSource.ParentNode is SCTFunctionNode)
            {

                ISCTNode sourceNode = m_pSource.ParentNode;
                SCTFunctionNode temp = (SCTFunctionNode)sourceNode;
                m_OutVariableName = m_pSource.LocalID + "_" + temp.GetNodeID();
            }
        }

        public void Disconnect()
        {
            m_ConnectedFlag = false;
            m_pDestination.Disconnect();
            m_pSource.Disconnect();
        }

        public void Draw(Graphics g)
        {
            if (!m_ConnectedFlag) return;
          if(m_Line != null)
                m_Line.DrawConnectionLine(g, m_pSource.WinFormControl, m_pDestination.WinFormControl,3,3);
        }

        public void UpdateOnObjectMoved()
        {
            if (!m_ConnectedFlag) return;
            if (m_Line != null) m_Line.Invalidate();
        }

        public void Dispose()
        {
            m_Line.Dispose();
        }

        private Point x;
        private Point y;

        public void TestKillLine()
        {
            m_Line.GetConfig(out x, out y);
            m_Line.Invalidate();
            m_Line.Dispose();
            
            m_Line = null;
        }

        public void TestRebuiltLine()
        {
            m_Line = new ConnectionLine(DrawOnControl);
            m_Line.ApplyConfig(x, y);
            m_Line.Invalidate();
        }
       

    }
}
