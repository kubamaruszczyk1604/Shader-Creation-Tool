﻿using System.Drawing;
using System.Windows.Forms;


namespace ShaderCreationTool
{
    delegate ISCTNode OnPlaceNodeCallback(Point location, NodeType nodeType);
    delegate void OnPlaceNodeCanceledCallback(Point location);
    class NodeInstantiator
    {
        private static Panel s_Panel;
        private static OnPlaceNodeCallback s_OnPlaceCallback;
        private static OnPlaceNodeCanceledCallback s_OnCanceled;
        private static ObjectMovedCallback s_MovedCallback;
        private static bool s_PlacingFlag;
        private static bool s_LeftPressed;
        private static NodeType s_NodeType;
        private static FunctionNodeDescription s_NodeDescription;


        static private void OnMoved()
        {
            s_Panel.SendToBack();
            if (s_MovedCallback != null) s_MovedCallback();
        }

        /////////////////////////////////// PUBLIC ////////////////////////////////////

  

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

        static public void StartPlacing(NodeType nodeType)
        {
            s_Panel.Visible = true;
            s_PlacingFlag = true;
            s_NodeType = nodeType;
        }

        static public void StartPlacing(FunctionNodeDescription desc)
        {
            s_Panel.Visible = true;
            s_PlacingFlag = true;
            s_NodeType = NodeType.Function;
            s_NodeDescription = desc;
        }

        static public void StartPlacingXML(FunctionNodeDescription desc)
        {
            s_NodeType = NodeType.Function;
            s_NodeDescription = desc;
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
                        s_OnPlaceCallback(s_Panel.Location,s_NodeType);
                        s_NodeDescription = null; //reset node desc
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

        static public FunctionNodeDescription FunctionDescriptionStruct
        {
            get { return s_NodeDescription; }
        }




    }
}
