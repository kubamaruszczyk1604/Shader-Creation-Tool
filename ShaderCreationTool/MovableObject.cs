using System.Drawing;
using System.Windows.Forms;

namespace ShaderCreationTool
{
    public delegate void ObjectMovedCallback();
    class MovableObject
    {
        private Control p_Control;
        private Point m_MouseDownLocation;

       
        public ObjectMovedCallback OnObjectMoved;

        public MovableObject(Control control)
        {
            p_Control = control;
            p_Control.MouseDown += MoveControlMouseCapture;
            p_Control.MouseMove += MoveControlMouseMove;
            OnObjectMoved = null;

        }

        public void AddObjectMovedEventListener(ObjectMovedCallback method)
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
