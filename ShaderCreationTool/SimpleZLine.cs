using System;
using System.Drawing;
using System.Windows.Forms;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShaderCreationTool
{
    class SimpleZLine
    {
        private readonly Pen m_Pen = new Pen(Color.FromArgb(70, 255, 155, 155), 3);


        private Control p_Control;
        private bool m_Invalidate;


       

        ////////////////////////////////////////  PUBLIC  ///////////////////////////////////////////////

        public SimpleZLine(Control control)
        {
            p_Control = control;
            m_Invalidate = false;
        }



        public void DrawConnectionLine(Graphics g, Point a, Point b)
        {

            Point start = (a.X <= b.X) ? a : b;
            Point end = (start.Equals(a)) ? b : a;


         

            int halfXDist = (int)(((float)end.X - (float)start.X) * 0.5f);


            Point mid1 = new Point(start.X+halfXDist, start.Y);

            Point mid2 = new Point(mid1.X, end.Y);


            Point[] points = { start, mid1, mid2, end };

            if (m_Invalidate)
            {
                p_Control.Invalidate();
                m_Invalidate = false;
            }
            g.DrawLines(m_Pen, points);
        }



        public void Invalidate()
        {
            m_Invalidate = true;
        }


  

    }
}
