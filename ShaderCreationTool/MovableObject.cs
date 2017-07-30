using System.Drawing;
using System.Windows.Forms;

namespace ShaderCreationTool
{
    public delegate void ObjectMovedCallback();
    class MovableObject
    {
        private Control p_Control;
        private Point m_MouseDownLocation;
        private bool m_HorizontalMovementLock;
        private bool m_VerticalMovementLock;

        private Point m_LowestLimit;
        private Point m_HighestLimit;
        private bool m_RestrictionEnabled;
        
       
        ////////////////////////////////////////  PUBLIC  /////////////////////////////////////////

        public ObjectMovedCallback OnObjectMoved;

        public MovableObject(Control control)
        {
            p_Control = control;
            p_Control.MouseDown += MoveControlMouseCapture;
            p_Control.MouseMove += MoveControlMouseMove;
            OnObjectMoved = null;
            m_HorizontalMovementLock = false;
            m_VerticalMovementLock = false;
            m_RestrictionEnabled = false;

            m_LowestLimit = new Point(0, 0);
            m_HighestLimit = new Point(500, 500);
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

                if (!m_HorizontalMovementLock)
                {
                    int newPosX = e.X + control.Left - m_MouseDownLocation.X;
                    if (m_RestrictionEnabled)
                    {
                        if (m_RestrictionEnabled)
                        {
                            if (m_LowestLimit.X > newPosX) newPosX = m_LowestLimit.X;
                            else if (m_HighestLimit.X < newPosX) newPosX = m_HighestLimit.X;
                        }
                    }
                    
                    control.Left = newPosX;
                    
                }
                if (!m_VerticalMovementLock)
                {

                    int newPosY = e.Y + control.Top - m_MouseDownLocation.Y;
                    if (m_RestrictionEnabled)
                    {
                        if (m_LowestLimit.Y > newPosY) newPosY = m_LowestLimit.Y;
                        else if (m_HighestLimit.Y < newPosY) newPosY = m_HighestLimit.Y;
                    }
                    
                    control.Top = newPosY;
                   
                }

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

        public void SetHorizontalLock(bool locked)
        {
            m_HorizontalMovementLock = locked;
        }

        public void SetVerticalLock(bool locked)
        {
            m_VerticalMovementLock = locked;
        }

        public void EnableMovementRestriction()
        {
            m_RestrictionEnabled = true;
        }

        public void DisableMovementRestriction()
        {
            m_RestrictionEnabled = false;
        }


        public void SetMovementRestrictionPoints(Point lowestLimit, Point highestLimit)
        {
            m_LowestLimit = lowestLimit;
            m_HighestLimit = highestLimit;
        }
    }
}
