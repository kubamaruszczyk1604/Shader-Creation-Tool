using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace ShaderCreationTool
{
    class ConnectionLine
    {
        private readonly Pen m_Pen = new Pen(Color.FromArgb(100,100,100,255), 3);
        private Control p_Control;
        private bool m_Invalidate;
        private float m_Pan;
        private Button m_RegulationButton;
        private MovableObject m_RegulationButtonMover;
        private Point m_OldRegulationPos;
        private bool m_RegulationMoving;
        private float m_StartX;
        private float m_EndX;


        private void OnObjectMoved()
        {

            
            m_RegulationMoving = true;
            m_Invalidate = true;
            p_Control.Update();


            // m_Pan += 0.01f;
            // if (m_Pan > 0.85f) m_Pan = 0.85f;
            float range = Math.Abs(m_EndX - m_StartX);

            float distance = m_RegulationButton.Location.X + m_RegulationButton.Width/2 - m_StartX;

            m_Pan = distance / range;
          
            SCTConsole.Instance.PrintLine("Pan range calculated: " + range.ToString());
        }

        void OnMouseUpOnRegulation(object sender, MouseEventArgs e)
        {
            m_RegulationMoving = false;
           
        }

        ////////////////////////////////////////  PUBLIC  ///////////////////////////////////////////////

        public ConnectionLine(Control control)
        {
            p_Control = control;
            m_Invalidate = false;
            m_Pan = 0.55f;
            m_StartX = 0; m_EndX = 0;
            m_RegulationButton = new Button();
            p_Control.Controls.Add(m_RegulationButton);
            m_RegulationButton.Size = new Size(10, 10);
            m_RegulationButton.Location = new Point(-400, 300);
            m_RegulationButton.BackColor = Color.FromArgb(100, 100, 100, 255);
            m_RegulationButton.ForeColor = Color.FromArgb(255, 100, 100, 130);
            m_RegulationButton.FlatStyle = FlatStyle.Flat;
            m_RegulationButton.Parent = p_Control;
            m_RegulationButton.MouseUp += OnMouseUpOnRegulation;
            m_RegulationButtonMover = new MovableObject(m_RegulationButton);
            m_RegulationButtonMover.AddObjectMovedEventListener(OnObjectMoved);
            m_RegulationButtonMover.SetVerticalLock(true);
            m_RegulationButtonMover.EnableMovementRestriction();
            m_RegulationMoving = false;
        }

        public void SetPan(float x)
        {
            if (x < 0.2f) x = 0.2f;
            if (x > 0.9f) x = 0.9f;

            m_Pan = x;
        }

        public void DrawConnectionLine(Graphics g, Point a, Point b)
        {

            Point orgin = (a.X <= b.X) ? a : b;
            Point endOrgin = (orgin.Equals(a)) ? b : a;

            int marginDistance = 30;

            if(Math.Abs(endOrgin.X-orgin.X) < 2*marginDistance)
            {
                marginDistance = 5;
                m_Pan = 0.5f;
            }

            Point aFixed = new Point(a.X + marginDistance, a.Y);
            Point bFixed = new Point(b.X - marginDistance, b.Y); 

            Point start = (aFixed.X <= bFixed.X) ? aFixed : bFixed;
            Point end = (start.Equals(aFixed)) ? bFixed : aFixed;

            m_StartX = start.X;
            m_EndX = end.X;

            int halfXDist = (int)(((float)end.X - (float)start.X) * m_Pan);

            Point mid1 = new Point(start.X + halfXDist, start.Y);
            Point mid2 = new Point(mid1.X, end.Y);
            m_RegulationButtonMover.SetMovementRestrictionPoints(start, end);
            if (!m_RegulationMoving)

            m_RegulationButton.Location = new Point(mid1.X - m_RegulationButton.Width / 2,
                    mid1.Y - (mid1.Y - mid2.Y) / 2);


            int arrowSize = 5;
            Point[] points =
             {

                start,
                mid1,
                mid2,
                end,
                // Arrow
                new Point(end.X- arrowSize, end.Y - arrowSize),
                new Point( end.X- arrowSize, end.Y + arrowSize),
                new Point(end.X, end.Y)
             };
            if (m_Invalidate)
            {
                //  g.DrawRectangle(m_Pen,GetRectangleByLine(start, mid1));
                p_Control.Invalidate(GetRectangleByLine(start, mid1));
                if (mid1.Y < mid2.Y)
                {
                    //  g.DrawRectangle(m_Pen, GetRectangleByLine(mid1, mid2));
                    p_Control.Invalidate(GetRectangleByLine(mid1, mid2));
                }
                else
                {
                    //   g.DrawRectangle(m_Pen, GetRectangleByLine(mid2, mid1));
                    p_Control.Invalidate(GetRectangleByLine(mid2, mid1));
                }
                // g.DrawRectangle(m_Pen, GetRectangleByLine(mid2, end));
                p_Control.Invalidate(GetRectangleByLine(mid2, end));
                m_Invalidate = false;
            }
            g.DrawLines(m_Pen, points);
            g.DrawLine(m_Pen,a, aFixed);
            g.DrawLine(m_Pen, b, bFixed);
            m_OldRegulationPos = m_RegulationButton.Location;
        }


        public void DrawConnectionLine(Graphics g, Control control1, Control control2)
        {
            Point start = new Point(control1.Left + control1.Width, control1.Top + control1.Height / 2);
            Point end = new Point(control2.Left, control2.Top + control2.Height / 2);
            this.DrawConnectionLine(g, start, end);
        }

        public void DrawConnectionLine(Graphics g, Control sourceControl, Control destinationControl, int ancestorCountC1, int ancestorCountC2)
        {

            Point C1TransformStack = new Point(0, 0);
       

            Control c1 = sourceControl;
            while(sourceControl.Parent != null && ancestorCountC1 > 0)
            {
                c1 = c1.Parent;
                C1TransformStack = new Point(c1.Location.X + C1TransformStack.X, c1.Location.Y + C1TransformStack.Y);
                ancestorCountC1--;
            }


            Point C2TransformStack = new Point(0, 0);


            Control c2 = destinationControl;
            while (destinationControl.Parent != null && ancestorCountC2 > 0)
            {
                c2 = c2.Parent;
                C2TransformStack = new Point(c2.Location.X + C2TransformStack.X, c2.Location.Y + C2TransformStack.Y);
                ancestorCountC2--;
            }


            Point start = new Point(sourceControl.Left + sourceControl.Width + C1TransformStack.X,
               C1TransformStack.Y + sourceControl.Top + sourceControl.Height / 2);
            Point end = new Point(destinationControl.Left + C2TransformStack.X,  C2TransformStack.Y + destinationControl.Top + destinationControl.Height / 2);
            this.DrawConnectionLine(g, start, end);
        }

        public void Invalidate()
        {
            m_Invalidate = true;
        }

        static public Rectangle GetRectangleByLine(Point p1, Point p2)
        {
            return new Rectangle(p1.X - 60, p1.Y - 60,
                Math.Abs(p1.X - p2.X) + 90, Math.Abs(p1.Y - p2.Y) + 90);
        }
    
    }
}
