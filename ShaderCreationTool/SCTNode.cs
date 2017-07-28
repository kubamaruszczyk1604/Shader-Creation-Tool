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
   
    class SCTNode
    {
        private Panel m_SctElement;
        private MovableObject m_Mover;

        public SCTNode(Panel nodeTemplate, Point initAtLocation,ObjectMoved onMovedCallback)
        {
            m_SctElement = nodeTemplate.CopyAsSCTElement(true);
            m_SctElement.Visible = true;
            m_SctElement.Location = initAtLocation;

            m_Mover = new MovableObject(m_SctElement);
            m_Mover.AddObjectMovedEventListener(onMovedCallback);
        }
    }
}
