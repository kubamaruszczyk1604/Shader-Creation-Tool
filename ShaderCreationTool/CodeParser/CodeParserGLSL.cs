using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShaderCreationTool
{
    class CodeParserGLSL: ICodeParser
    {
        private List<string> signatures;
        private List<Connection> m_InputConnections = new List<Connection>();
        public CodeParserGLSL()
        {
            signatures = new List<string>();
        }

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
       
            try
            {
                FunctionNodeDescription desc = node.NodeDescription;
                string signature = CreateSignature(desc);
                if (signatures.Contains(signature))
                {
                    status = "Repeated signature: " + signature;
                    status += "SKIPPING..";
                    return true;
                }
                string body = "{\r\n";
                body += ParseCode(desc.GetFunctionString());
                body += "\r\n}\r\n";
                functionCode = signature + body;
                signatures.Add(signature);
               // SCTConsole.Instance.PrintDebugLine(functionCode);
            }
            catch(Exception e)
            {
                status = e.Message;
                return false;
            }
            status = "ok";
            return true;
        }
        public bool TranslateNodeListIntoFunctions(List<SCTFunctionNode> nodes, out string functionCode, out string status)
        {
            signatures.Clear();
            functionCode = "";
            status = "";
            bool allOk = true;
            foreach(SCTFunctionNode node in nodes)
            {
                string tempFunctionCode;
                string tempStatus;
                if (TranslateNodeIntoFunction(node, out tempFunctionCode, out tempStatus))
                {
                    functionCode += tempFunctionCode;
                }
                else
                {
                    allOk = false;
                }
                status += tempStatus;
                
            }

            return allOk;
        }


        public bool TranslateNetwork(List<ISCTNode> nodes, List<Connection> connections, out string code, out string status)
        {
            code = "";
            status = "";

            return false;
        }


        

        public string ConstructFunctionCall(SCTFunctionNode node)
        {
            string output = string.Empty;


            List<Connector> outConnectors = node.GetAllConnectors(ConnectionDirection.Out);
            List<string> outVarParameters = new List<string>();

            foreach(Connector c in outConnectors)
            {
                if (!c.Connected) continue;
                string assembled = TranslateVariableType(c.VariableType) + "  " + c.ParentConnection.OutVariableName +"\r\n";

                outVarParameters.Add(c.ParentConnection.OutVariableName);
                output += assembled;
            }



            output += node.NodeDescription.Name + "(";
            List<Connector> inConnectors = node.GetAllConnectors(ConnectionDirection.In);
            foreach (Connector c in inConnectors)
            {
                if (!c.Connected) continue;

                output += c.ParentConnection.OutVariableName + ", ";
                
             
            }
            for(int i = 0; i < outVarParameters.Count;++i)
            {
                output += outVarParameters[i];
                if (i < outVarParameters.Count - 1) output += ", ";
            }
            output += ");";

            SCTConsole.Instance.PrintDebugLine("\r\n\r\n\r\n" + output);
            return output;
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

            signature += ")\r\n";
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

        private string ParseCode(string code)
        {
            string s = code;
            s = s.Replace("VECTOR4 ", "vec4 ").Replace("VECTOR3 ", "vec3 ").Replace("VECTOR2 ", "vec2 ").Replace("FLOAT ", "float ").Replace("COLOUR ", "vec4 ");
            return s;
        }


    }
}
