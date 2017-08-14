using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShaderCreationTool
{
    public enum NodeType
    {
        Input_Float = 0,
        Input_Float2 = 1,
        Input_Float3 = 2,
        Input_Float4 = 3,
        Input_Colour = 4,
        Input_Texture2D = 5,
        Funtion = 6,
        Target = 7
    }

    delegate void NodeCloseButtonCallback(ISCTNode sender);
    delegate void NodeInputError(string errorDescription, ISCTNode sender);

    interface ISCTNode
    {
        void AddOnMovedCallback(ObjectMovedCallback onMovedCallback);
        void AddOnBeginConnectionCallback(BeginConnectionCallback onBeginConnection);
        void AddOnBreakConnectionCallback(BreakConnectionCallback onBreakConnection);
        Connector GetConnector(ConnectionDirection type, int index);
        List<Connector> GetAllConnectors(ConnectionDirection type);
        List<Connector> GetAllConnectors();
        NodeType GetNodeType();
        string GetNodeID();
    }

    interface IInputNode
    {
       void AddOnCloseCallback(NodeCloseButtonCallback callback);
       void AddInputErrorCallback(NodeInputError callback);
    }

    class NodeIDCreator
    {
        static public string CreateID(NodeType type, int counter)
        {
            string str = "SCT_NODE_" + 
                type.ToString() + DateTime.Now.ToString("_yy_MM_dd_HH_mm_ss_") + counter;
            return str;
        }
    }

    class LockableNodes
    {
        static public void LockButtons()
        {
            SCTFunctionNode.LockButtons();
            InputNodeColour.LockButtons();
            InputNodeVector.LockButtons();
        }

        static public void UnlockButtons()
        {
            SCTFunctionNode.UnlockButtons();
            InputNodeColour.UnlockButtons();
            InputNodeVector.UnlockButtons();
        }
    }
}
