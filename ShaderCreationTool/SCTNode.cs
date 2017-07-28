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
        private List<Connector> m_SourceConnectors;
        private List<Connector> m_DestinationConnectors;


        public SCTNode(Panel nodeTemplate, Point initAtLocation)
        {
            m_SctElement = nodeTemplate.CopyAsSCTElement(true);
            m_SctElement.Visible = true;
            m_SctElement.Location = initAtLocation;

            m_Mover = new MovableObject(m_SctElement);
            m_SourceConnectors = new List<Connector>();
            m_DestinationConnectors = new List<Connector>();

            List<CheckBox> boxes = ControlExtensions.GetAllChildreenControls<CheckBox>(m_SctElement).Cast<CheckBox>().ToList();

            for(int i = 0; i < boxes.Count;++i)
            {
                Connector tempCon = new Connector(boxes[i]);
                //m_Connector.RegisterListener_BeginConnection(OnConnectionBegin);
                if (tempCon.Type == ConnectorType.Source)
                {
                    m_SourceConnectors.Add(tempCon);
                }
                else if (tempCon.Type == ConnectorType.Destination)
                {
                    m_DestinationConnectors.Add(tempCon);
                }
            }
        }



        public void RegisterListener_OnMoved(ObjectMovedCallback onMovedCallback)
        {
            m_Mover.AddObjectMovedEventListener(onMovedCallback);
        }

        public void RegisterListener_OnBeginConnection(BeginConnectionCallback onBeginConnection)
        {
            foreach(Connector c in m_SourceConnectors)
            {
                c.RegisterListener_BeginConnection(onBeginConnection);
            }
            foreach (Connector c in m_DestinationConnectors)
            {
                c.RegisterListener_BeginConnection(onBeginConnection);
            }
        }
    }
}
