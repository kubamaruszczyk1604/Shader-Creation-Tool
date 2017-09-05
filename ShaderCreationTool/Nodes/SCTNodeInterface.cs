using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

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
        Function = 6,
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
        void Serialize(XmlWriter target);
        void ChangeUniqueID(string uniqueID);

    }

    interface IInputNode: ISCTNode
    {
       void AddOnCloseCallback(NodeCloseButtonCallback callback);
       void AddInputErrorCallback(NodeInputError callback);
       string GetVariableName();
       ShaderVariableType GetShaderVariableType();
       void ChangeVariableName(string varName);

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

   
}
