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

namespace ShaderCreationTool
{
    delegate void OnPlaceNodeCallback(Point location);
    delegate void OnPlaceNodeCanceledCallback(Point location);
    class NodeInstantiator
    {
        private static Panel s_Panel;
        private static OnPlaceNodeCallback s_OnPlaceCallback;
        private static OnPlaceNodeCanceledCallback s_OnCanceled;
        private static ObjectMovedCallback s_MovedCallback;
        private static bool s_PlacingFlag;
        private static bool s_LeftPressed;


        static public void SetupInstantiator(Panel drawPanel)
        {
            s_Panel = drawPanel;
            s_Panel.SendToBack();
            s_PlacingFlag = false;
            s_LeftPressed = false;
        }


        static public void AddOnObjectMovedCallback(ObjectMovedCallback callback)
        {
            s_MovedCallback += callback;
        }

        static public void AddOnPlaceListener(OnPlaceNodeCallback callback)
        {
            s_OnPlaceCallback += callback;
        }

        static public void AddOnCancelListener(OnPlaceNodeCanceledCallback callback)
        {
            s_OnCanceled += callback;
        }

        static public void StartPlacing()
        {
            s_Panel.Visible = true;
            s_PlacingFlag = true;
        }

        static public void Update(Panel mainPanel)
        {
            if (s_PlacingFlag)
            {
                s_Panel.Location = mainPanel.PointToClient(
                    new Point(Cursor.Position.X - s_Panel.Size.Width / 2,Cursor.Position.Y));
                OnMoved();
                if (s_LeftPressed)
                {
                    s_PlacingFlag = false;
                    s_Panel.Visible = false;
                    if(s_OnPlaceCallback != null)
                    {
                        s_OnPlaceCallback(s_Panel.Location);
                    }

                }
            }
            s_LeftPressed = false;
        }

        static public void CaptureLeftMousePress()
        {
           if(s_PlacingFlag) s_LeftPressed = true;
        }

        static public void CancelInstantiate()
        {
            s_Panel.Visible = false;
            s_PlacingFlag = false;
        }

        static private void OnMoved()
        {
            s_Panel.SendToBack();
            if (s_MovedCallback != null) s_MovedCallback();
        }




    }
}
