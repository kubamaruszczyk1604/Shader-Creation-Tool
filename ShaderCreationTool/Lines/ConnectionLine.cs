﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace ShaderCreationTool
{
    class ConnectionLine: IDisposable
    {
        private readonly Pen m_StandardPen = new Pen(Color.FromArgb(100,255,155,155), 3);
        private readonly Pen m_HighlightPen = new Pen(Color.FromArgb(150, 255, 100, 100), 5);
        private readonly Size c_ButtonSize = new Size(13, 13);
        private readonly Color c_ButtonBackColour = Color.FromArgb(100, 255, 155, 155);
        private readonly Color c_ButtonForeColour = Color.FromArgb(255, 100, 100, 130);

        private Pen m_UsedPen;
        private Control p_Control;
        private bool m_Invalidate;
        private float m_Pan;
        private Button m_XRegulationButton;
        private MovableObject m_XRegulationButtonMover;

        private Button m_YRegulationButton;
        private MovableObject m_YRegulationButtonMover;

        private bool m_YMoving;
        private int m_YPosition;
        bool m_CapturedYPosition;


        private bool m_XMoving;
        private float m_StartX;
        private float m_EndX;

        private void OnXbuttonMoved()
        {        
            m_XMoving = true;
            m_Invalidate = true;
            p_Control.Update();
   
            float rangeX = Math.Abs(m_EndX - m_StartX);
            float distanceX = m_XRegulationButton.Location.X + m_XRegulationButton.Width/2 - m_StartX;

            if (rangeX == 0) rangeX = 1;// to fix nan error when there is no difference
            m_Pan = distanceX / rangeX;
          
            //SCTConsole.Instance.PrintDebugLine("Pan range calculated: " + rangeX.ToString());
        }

        private void OnYbuttonMoved()
        {
            m_YMoving = true;
            m_Invalidate = true;
            p_Control.Update();
           // SCTConsole.Instance.PrintDebugLine("Y range calculated: ");
        }

        void OnMouseUpXButton(object sender, MouseEventArgs e)
        {
            m_XMoving = false;
        }

        void OnMouseUpYButton(object sender, MouseEventArgs e)
        {
            m_YMoving = false;
        }

        ////////////////////////////////////////  PUBLIC  ///////////////////////////////////////////////

        public ConnectionLine(Control control)
        {
            m_UsedPen = m_StandardPen;
            p_Control = control;
            m_Invalidate = false;
            m_Pan = 0.55f;
            m_StartX = 0; m_EndX = 0;
            m_XRegulationButton = new Button();
            p_Control.Controls.Add(m_XRegulationButton);
            m_XRegulationButton.Size = c_ButtonSize;
            m_XRegulationButton.Location = new Point(-400, 300);
            m_XRegulationButton.BackColor = c_ButtonBackColour;
            m_XRegulationButton.ForeColor = c_ButtonForeColour;
            m_XRegulationButton.FlatStyle = FlatStyle.Flat;
            m_XRegulationButton.Parent = p_Control;
            m_XRegulationButton.MouseUp += OnMouseUpXButton;
            m_XRegulationButtonMover = new MovableObject(m_XRegulationButton);
            m_XRegulationButtonMover.AddObjectMovedEventListener(OnXbuttonMoved);
            m_XRegulationButtonMover.SetVerticalLock(true);
            m_XRegulationButtonMover.EnableMovementRestriction();


            m_YRegulationButton = new Button();
            p_Control.Controls.Add(m_YRegulationButton);
            m_YRegulationButton.Size = c_ButtonSize;
            m_YRegulationButton.Location = new Point(-400, 300);
            m_YRegulationButton.BackColor = c_ButtonBackColour;
            m_YRegulationButton.ForeColor = c_ButtonForeColour;
            m_YRegulationButton.FlatStyle = FlatStyle.Flat;
            m_YRegulationButton.Parent = p_Control;
            m_YRegulationButton.MouseUp += OnMouseUpYButton;
            m_YRegulationButtonMover = new MovableObject(m_YRegulationButton);
            m_YRegulationButtonMover.AddObjectMovedEventListener(OnYbuttonMoved);
            m_YRegulationButtonMover.SetHorizontalLock(true);
            //m_YRegulationButtonMover.EnableMovementRestriction();

            m_XMoving = false;
            m_YMoving = false;
            m_CapturedYPosition = false;
           // Highlight(true);
        }

        public void Highlight(bool on)
        {
            if (on) m_UsedPen = m_HighlightPen;
            else m_UsedPen = m_StandardPen;
            p_Control.Invalidate();
            m_Invalidate = true;
        }



        public void ApplyConfig(Point x, Point y)
        {
         
            m_XRegulationButton.Location = x;
            OnXbuttonMoved();
            m_YRegulationButton.Location = y;
            OnYbuttonMoved();
            m_XMoving = false;
            m_YMoving = false;
           // m_CapturedYPosition = false;
        }

        public void GetConfig(out Point x, out Point y)
        {
            x = new Point( m_XRegulationButton.Location.X,//- m_XRegulationButton.Size.Width/2,
                         m_XRegulationButton.Location.Y);
            y = new Point(m_YRegulationButton.Location.X, m_YRegulationButton.Location.Y +
                m_YRegulationButton.Size.Height/2);
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

            Point mid1;
        
            if(m_CapturedYPosition)
            {
                mid1 = new Point(start.X, start.Y + m_YPosition);
            }
            else
            {
                mid1 = new Point(start.X, start.Y);
            }

            int halfXDist = (int)(((float)end.X - (float)mid1.X) * m_Pan);
            Point mid2 = new Point(mid1.X + halfXDist, mid1.Y);
            Point mid3 = new Point(mid2.X, end.Y);


            m_StartX = start.X;
            m_EndX =  end.X;

            if (!m_YMoving)
            {
                m_YRegulationButton.Location = new Point(mid1.X + (mid2.X - mid1.X) / 2 - m_XRegulationButton.Width / 2,
                            mid1.Y - m_YRegulationButton.Width / 2);
            }
            else
            {
                mid1.Y = m_YRegulationButton.Location.Y;
                mid2.Y = m_YRegulationButton.Location.Y;
                m_CapturedYPosition = true;
                m_YPosition = m_YRegulationButton.Location.Y-start.Y;
            }

            m_XRegulationButtonMover.SetMovementRestrictionPoints(start, end);
            if (!m_XMoving)
            {
                m_XRegulationButton.Location = new Point(mid2.X - m_XRegulationButton.Width / 2,
                        mid2.Y - (mid2.Y - mid3.Y) / 2);
            }

            Point[] points = { start, mid1, mid2, mid3, end };

            if (m_Invalidate)
            {
                p_Control.Invalidate();
                m_Invalidate = false;
            }
           
            g.DrawLines(m_UsedPen, points);
            g.DrawLine(m_UsedPen, a, aFixed);
            g.DrawLine(m_UsedPen, b, bFixed);
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

        public void Dispose()
        {
            p_Control.Controls.Remove(m_XRegulationButton);
            p_Control.Controls.Remove(m_YRegulationButton);
            p_Control.Invalidate(false);
            p_Control.Update();
            SCTConsole.Instance.PrintDebugLine("Dispose called on line");
        }

        static public Rectangle GetRectangleByLine(Point p1, Point p2)
        {
            const int marginA = 60;
            const int marginB = 90;
            return new Rectangle(p1.X - marginA, p1.Y - marginA,
                Math.Abs(p1.X - p2.X) + marginB, Math.Abs(p1.Y - p2.Y) + marginB);
        }


    
    }
}
