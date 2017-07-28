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
    public delegate void ObjectMoved();
    class MovableObject
    {
        private Control p_Control;
        private Point m_MouseDownLocation;

       
        public ObjectMoved OnObjectMoved;

        public MovableObject(Control control)
        {
            p_Control = control;
            p_Control.MouseDown += MoveControlMouseCapture;
            p_Control.MouseMove += MoveControlMouseMove;
            OnObjectMoved = null;

        }

        public void AddObjectMovedEventListener(ObjectMoved method)
        {
            OnObjectMoved += method;
        }

        public void MoveControlMouseMove(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            { 
                control.Left = e.X + control.Left - m_MouseDownLocation.X;
                control.Top = e.Y + control.Top - m_MouseDownLocation.Y;
                control.Update(); 
                if(OnObjectMoved != null)
                {
                    OnObjectMoved();
                }
            }
        }

        public void MoveControlMouseCapture(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                m_MouseDownLocation = e.Location;
            }
        }


    }
}
