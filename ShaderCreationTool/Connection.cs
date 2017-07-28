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
    class Connection
    {
        private Connector m_pSource;
        private Connector m_pDestination;
        private ConnectionLine m_Line;
        private bool m_ConnectedFlag;

        ////////////////////////////////////////////  PUBLIC  ////////////////////////////////////////////

        public bool IsConnected { get { return m_ConnectedFlag; } } 


        public Connection(Connector a, Connector b, Control drawOn)
        {
            if(a.Type == b.Type)
            {
                throw new Exception("Both a and b have the same direction");
            }

            m_Line = new ConnectionLine(drawOn);
            m_pSource = (a.Type == ConnectorType.Source) ? a : b;
            m_pDestination = (m_pSource.Type == a.Type) ? b : a;
            m_ConnectedFlag = true;

        }

        public void Disconnect()
        {
            m_ConnectedFlag = false;
        }

        public void Draw(Graphics g)
        {
            if (!m_ConnectedFlag) return;
            m_Line.DrawConnectionLine(g, m_pSource.WinFormControl, m_pDestination.WinFormControl);
        }

        public void UpdateOnObjectMoved()
        {
            if (!m_ConnectedFlag) return;
            m_Line.Invalidate();
        }
       

    }
}
