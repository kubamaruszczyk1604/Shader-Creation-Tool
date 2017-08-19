﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShaderCreationTool
{
    class CodeParserGLSL: ICodeParser
    {

       public bool TranslateInputVariables(List<IInputNode> inputNodes, out string declarationsCode, out string status)
        {
            declarationsCode = "";
            status = "";
            const int maxTypeCharCount = 15;
            bool ok = true;
            foreach(IInputNode node in inputNodes)
            {
                string typeStr = TranslateVariableType(node.GetShaderVariableType());
                declarationsCode += "uniform " + typeStr + " "+ new string(' ',maxTypeCharCount- typeStr.Length)  + node.GetVariableName() + "\r\n";
                if (typeStr == string.Empty) ok = false;
            }
            return ok;
        }

        public bool TranslateNodeIntoFunction(SCTFunctionNode node, out string functionCode, out string status)
        {
            functionCode = "";
            status = "";

            FunctionNodeDescription desc = node.NodeDescription;
            string signature = CreateSignature(desc);

            SCTConsole.Instance.PrintDebugLine(signature);
            return false;
        }
        public bool TranslateNetwork(List<ISCTNode> nodes, List<Connection> connections, out string code, out string status)
        {
            code = "";
            status = "";
            return false;
        }

        private string CreateSignature(FunctionNodeDescription desc)
        {
            string signature = "void " + desc.Name + "(";
            

            for (int i = 0; i < desc.InputCount; ++i)
            {
                ShaderVariableDescription varDesc = desc.GetInVariableDescription(i);
                string assembled = TranslateVariableType(varDesc.Type) + " " + varDesc.Name;

                signature += assembled;
                if (i < desc.InputCount - 1) signature += ", ";
            }
            signature += ", ";

            for (int i = 0; i < desc.OutputCount; ++i)
            {
                ShaderVariableDescription varDesc = desc.GetOutVariableDescription(i);
                string assembled = "inout " + TranslateVariableType(varDesc.Type) + " " + varDesc.Name;

                signature += assembled;
                if (i < desc.OutputCount - 1) signature += ", ";
            }


            signature += ")";
            return signature;
        }

        private string TranslateVariableType(ShaderVariableType varType)
        {
            switch (varType)
            {
                case ShaderVariableType.Single: return "float";
                case ShaderVariableType.Vector2: return "vec2";
                case ShaderVariableType.Vector3: return "vec3";
                case ShaderVariableType.Vector4: return "vec4";
                case ShaderVariableType.Texture2D: return "sampler2D";
                default: return string.Empty;
             }
        }


    }
}
