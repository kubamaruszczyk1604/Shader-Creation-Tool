using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;

using System.Runtime.InteropServices;

namespace ShaderCreationTool
{
    class ZoomController
    {
       
        private bool zoomInIn;
        private bool zoomOutIn;
        public ZoomController(Button zoomIn, Button zoomOut)
        {
            zoomInIn = false;
            zoomOutIn = false;
            zoomIn.Click += ZoomInButton_Click;
            zoomIn.MouseEnter += ZoomInButton_MouseEnter;
            zoomIn.MouseLeave += ZoomInButton_MouseLeave;

            zoomOut.Click += ZoomOutButton_Click;
            zoomOut.MouseEnter += ZoomOutButton_MouseEnter;
            zoomOut.MouseLeave += ZoomOutButton_MouseLeave;

        }

        public void RegisterLeftClick()
        {
            if (zoomInIn) Bridge.Zoom(0.3f);
            if (zoomOutIn) Bridge.Zoom(-0.3f);
        }

        private void ZoomInButton_Click(object sender, EventArgs e)
        {
            Bridge.Zoom(3);
        }

        private void ZoomOutButton_Click(object sender, EventArgs e)
        {
            Bridge.Zoom(-3);
        }


        private void ZoomInButton_MouseEnter(object sender, EventArgs e)
        {
            zoomInIn = true;
        }

        private void ZoomInButton_MouseLeave(object sender, EventArgs e)
        {
            zoomInIn = false;
        }
        private void ZoomOutButton_MouseEnter(object sender, EventArgs e)
        {
            zoomOutIn = true;
        }

        private void ZoomOutButton_MouseLeave(object sender, EventArgs e)
        {
            zoomOutIn = false;
        }
    }
}