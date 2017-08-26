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
        Target = 7,
        AttribPosition = 8,
        AttribNormal = 9,
        AttribUVs = 10,
        AttribTangent = 11,
        AttribInput_Time = 12,
        AttribInput_CameraPos = 13
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

    interface IInputNode: ISCTNode
    {
       void AddOnCloseCallback(NodeCloseButtonCallback callback);
       void AddInputErrorCallback(NodeInputError callback);
       string GetVariableName();
       ShaderVariableType GetShaderVariableType();

    }

    interface IAttribNode: ISCTNode
    {
        void AddOnCloseCallback(NodeCloseButtonCallback callback);
        string GetVariableName();
        ShaderVariableType GetShaderVariableType();

    }


    class NodeIDCreator
    {
        static public string CreateID(NodeType type,int counter)
        {
            string str = "NODE_" + 
                type.ToString() + "_" +counter.ToString()  + DateTime.Now.ToString("_yy_MM_dd_HH_mm_ss") ;
            return str;
        }
        static public string CreateID(FunctionNodeDescription desc, int counter)
        {
            string str = "NF_" + desc.Name + "_"+ counter.ToString() +"_"
                + DateTime.Now.ToString("ss");
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

    static class AttribVariableStrings
    {
        // simple
        public static readonly string c_TimeVariableName = "UnifTime";
        public static readonly string c_UVVariableName = "UVs";


        // with space selection
        public static readonly string[] c_NamesPosition =  {
            "Position_WorldSpace",
            "Position_ObjectSpace",
            "Position_EyeSpace"
        };
        public static readonly string[] c_NamesNormal = { "Normal_InverseWorld", "Normal_ObjectScpace" };
        public static readonly string[] c_NamesCameraPos = { "Camera_WorldSpace" };
    }
}
