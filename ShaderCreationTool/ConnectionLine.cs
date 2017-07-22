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
        private readonly Pen m_Pen = new Pen(Color.White, 3);
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
            int halfXDist = (int)(((float)end.X - (float)start.X) * 0.8f);

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

        public void Invalidate()
        {
            m_Invalidate = true;
        }

        static public Rectangle GetRectangleByLine(Point p1, Point p2)
        {
            return new Rectangle(p1.X - 20, p1.Y - 20,
                Math.Abs(p1.X - p2.X) + 40, Math.Abs(p1.Y - p2.Y) + 40);
        }
    
    }
}
