﻿using System;
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
        private List<Connector> m_OutputConnectors;
        private List<Connector> m_InputConnectors;
        private string m_Label = string.Empty;


        //////////////////////////////////////////  PUBLIC  ///////////////////////////////////////////////
        
        public SCTNode(Panel nodeTemplate, Point location, NodeDescription description)
        {
            m_SctElement = nodeTemplate.CopyAsSCTElement(true);
            m_SctElement.Visible = true;
            m_SctElement.Location = location;

            m_Mover = new MovableObject(m_SctElement);
            m_OutputConnectors = new List<Connector>();
            m_InputConnectors = new List<Connector>();

            List<CheckBox> boxes = ControlExtensions.GetAllChildreenControls<CheckBox>(m_SctElement).Cast<CheckBox>().ToList();

            CheckBox inBox = null;
            CheckBox outBox = null;
            for (int i = 0; i < boxes.Count; ++i)
            {
                if(boxes[i].Name.Contains(Connector.s_InSlotSequenceID))
                {
                    inBox = boxes[i];
                }
                else if (boxes[i].Name.Contains(Connector.s_OutSlotSequenceID))
                {
                    outBox = boxes[i];
                }
            }

            boxes.Clear();

            for(int i = 0; i < description.InputCount;++i)
            {
                CheckBox cd = (i==0)? inBox : inBox.CopyAsSCTElement(true);
                cd.Location = new Point(cd.Location.X, cd.Location.Y + 20*i);
                cd.Text = description.GetInVariableDescription(i).Name;
                Connector tempCon = new Connector(cd, description.GetInVariableDescription(i).Type);
                m_InputConnectors.Add(tempCon);
            }

            for (int i = 0; i < description.OutputCount; ++i)
            {
                CheckBox cd = (i == 0) ? outBox : outBox.CopyAsSCTElement(true);
                cd.Location = new Point(cd.Location.X, cd.Location.Y + 20 * i);
                cd.Text = description.GetOutVariableDescription(i).Name;
                Connector tempCon = new Connector(cd, description.GetOutVariableDescription(i).Type);
                m_OutputConnectors.Add(tempCon);
            }

            List<Label> labels = ControlExtensions.GetAllChildreenControls<Label>(m_SctElement).Cast<Label>().ToList();
            labels[0].Text = description.Name;

        }

        public SCTNode(Panel nodeTemplate, Point location, ObjectMovedCallback onObjectMoved, NodeDescription description) :
            this(nodeTemplate,location,description)
        {
            RegisterListener_OnMoved(onObjectMoved);
        }


        /// <summary>
        /// Registered method will be called when node is moved
        /// </summary>
        /// <param name="onMovedCallback"></param>
        public void RegisterListener_OnMoved(ObjectMovedCallback onMovedCallback)
        {
            m_Mover.AddObjectMovedEventListener(onMovedCallback);
        }

        /// <summary>
        /// Registered callback method will be called when any of the node's connectors is clicked
        /// </summary>
        /// <param name="onBeginConnection"></param>
        public void RegisterListener_OnBeginConnection(BeginConnectionCallback onBeginConnection)
        {
            foreach(Connector c in m_OutputConnectors)
            {
                c.AddCallback_BeginConnectionRequest(onBeginConnection);
            }
            foreach (Connector c in m_InputConnectors)
            {
                c.AddCallback_BeginConnectionRequest(onBeginConnection);
            }
        }

        /// <summary>
        /// Registered method will be called when user clicks on one of the connected connectors
        /// </summary>
        /// <param name="onBreakConnection"></param>
        public void RegisterListener_OnBreakConnection(BreakConnectionCallback onBreakConnection)
        {
            foreach (Connector c in m_OutputConnectors)
            {
                c.AddCallback_BreakConnectionRequest(onBreakConnection);
            }
            foreach (Connector c in m_InputConnectors)
            {
                c.AddCallback_BreakConnectionRequest(onBreakConnection);
            }
        }

        public Connector GetConnector(ConnectionDirection type, int index)
        {
            if (type == ConnectionDirection.Out) return m_OutputConnectors[index];
            else if (type == ConnectionDirection.In) return m_InputConnectors[index];
            else return null;
        }

        public List<Connector> GetAllConnectors(ConnectionDirection type)
        {
            if (type == ConnectionDirection.Out) return m_OutputConnectors;
            else if (type == ConnectionDirection.In) return m_InputConnectors;
            else return null;
        }

        public List<Connector> GetAllConnectors()
        {
            List<Connector> outList = new List<Connector>();
            outList.AddRange(m_OutputConnectors);
            outList.AddRange(m_InputConnectors);
            return outList;
        }


    }
}
