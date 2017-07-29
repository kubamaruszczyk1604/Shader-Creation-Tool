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


        public ConnectionLine(Control control)
        {
            p_Control = control;
            m_Invalidate = false;
        }


        public void DrawConnectionLine(Graphics g, Point a, Point b)
        {
            Point start = (a.X <= b.X) ? a : b;
            Point end = (start.Equals(a)) ? b : a;
            int halfXDist = (int)(((float)end.X - (float)start.X) * 0.85f);

            Point mid1 = new Point(start.X + halfXDist, start.Y);
            Point mid2 = new Point(mid1.X, end.Y);

            int arrowSize = 3;
            Point[] points =
             {
                start,
                mid1,
                mid2,
                end
                // Arrow
                //new Point(end.X- arrowSize, end.Y - arrowSize),
                //new Point(end.X- arrowSize, end.Y + arrowSize),
                //new Point(end.X, end.Y)
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

        }


        public void DrawConnectionLine(Graphics g, Control control1, Control control2)
        {
            Point start = new Point(control1.Left + control1.Width, control1.Top + control1.Height / 2);
            Point end = new Point(control2.Left, control2.Top + control2.Height / 2);
            this.DrawConnectionLine(g, start, end);
        }

        public void DrawConnectionLine(Graphics g, Control control1, Control control2, int ancestorCountC1, int ancestorCountC2)
        {

            Point C1TransformStack = new Point(0, 0);
       

            Control c1 = control1;
            while(control1.Parent != null && ancestorCountC1 > 0)
            {
                c1 = c1.Parent;
                C1TransformStack = new Point(c1.Location.X + C1TransformStack.X, c1.Location.Y + C1TransformStack.Y);
                ancestorCountC1--;
            }


            Point C2TransformStack = new Point(0, 0);


            Control c2 = control2;
            while (control2.Parent != null && ancestorCountC2 > 0)
            {
                c2 = c2.Parent;
                C2TransformStack = new Point(c2.Location.X + C2TransformStack.X, c2.Location.Y + C2TransformStack.Y);
                ancestorCountC2--;
            }



            Point start = new Point(control1.Left + control1.Width + C1TransformStack.X,
               C1TransformStack.Y + control1.Top + control1.Height / 2);
            Point end = new Point(control2.Left + C2TransformStack.X,  C2TransformStack.Y + control2.Top + control2.Height / 2);
            this.DrawConnectionLine(g, start, end);
        }

        public void Invalidate()
        {
            m_Invalidate = true;
        }

        static public Rectangle GetRectangleByLine(Point p1, Point p2)
        {
            return new Rectangle(p1.X - 40, p1.Y - 40,
                Math.Abs(p1.X - p2.X) + 60, Math.Abs(p1.Y - p2.Y) + 60);
        }
    
    }
}
