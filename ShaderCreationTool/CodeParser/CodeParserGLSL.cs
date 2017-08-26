using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ShaderCreationTool
{
    class CodeParserGLSL: ICodeParser
    {
        private const string VERTEX_BASE_PATH = @"..\Data\ShaderBase\vertBase.txt";
        private const string NUM_EXPR = @"\d+\.*\d*";
        private List<string> m_Signatures;
        private Dictionary<ShaderVariableType, Regex> m_DefaultValuesTable;



        public CodeParserGLSL()
        {
            m_Signatures = new List<string>();
            m_DefaultValuesTable = new Dictionary<ShaderVariableType, Regex>();
            m_DefaultValuesTable.Add(ShaderVariableType.Single, new Regex(@"^\d+\.*\d*$"));
            m_DefaultValuesTable.Add(ShaderVariableType.Vector2, new Regex(@"^\(\d+\.*\d*\,\d+\.*\d*\)$"));
            m_DefaultValuesTable.Add(ShaderVariableType.Vector3, new Regex(@"^\(\d+\.*\d*\,\d+\.*\d*\,\d+\.*\d*\)$"));
            m_DefaultValuesTable.Add(ShaderVariableType.Vector4, new Regex(@"^\(\d+\.*\d*\,\d+\.*\d*\,\d+\.*\d*\,\d+\.*\d*\)$"));
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
                declarationsCode += "uniform " + typeStr + " "+ new string(' ',maxTypeCharCount- typeStr.Length)  + node.GetVariableName() + ";\r\n";
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
                string signature = CreateFunctionSignature(desc);
                if (m_Signatures.Contains(signature))
                {
                    status = "Repeated signature: " + signature;
                    status += "SKIPPING..";
                    return true;
                }
                string body = "{\r\n";
                body += ParseCode(desc.GetFunctionString());
                body += "\r\n}\r\n";
                functionCode = signature + body;
                m_Signatures.Add(signature);
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
            m_Signatures.Clear();
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


        public bool TranslateNetworkFragment(List<ISCTNode> nodes, List<Connection> connections, out string code, out string status)
        {
            code = "";
            status = "";

            List<IInputNode> inputNodes = (nodes.FindAll(o => o is IInputNode)).Cast<IInputNode>().ToList();

            List<SCTFunctionNode> nodesToProcess = nodes.FindAll(o => o is SCTFunctionNode).Cast<SCTFunctionNode>().ToList();
            List<SCTFunctionNode> tempRestoreStateNodeList = new List<SCTFunctionNode>(nodesToProcess);
            List<Connection> inputConnections = connections.FindAll(o => o.IsDirectInputConnection);
            foreach (Connection c in inputConnections)
            {
                c.Highlighted = true;
            }

            // Input variables
            TranslateInputVariables(inputNodes, out code, out status);
            string codeFunct;
            string statusFunct;

            // Functions extraction
            TranslateNodeListIntoFunctions(nodesToProcess, out codeFunct, out statusFunct);
            code += "\r\n" + codeFunct;

            // network (function calls)
            string result = ProcessNodes(ref nodesToProcess);

            code += "\r\n\r\n void main()\r\n{\r\n" + result + "\r\n}"; 

            SCTConsole.Instance.PrintDebugLine(code);

            foreach(SCTFunctionNode node in tempRestoreStateNodeList)
            {
                SetOutputsAsCalclulated(node, false);
            }

            return false;
        }
    

        private string ProcessNodes(ref List<SCTFunctionNode> nodes)
        {
            string ret = string.Empty;

            List<SCTFunctionNode> removeList = new List<SCTFunctionNode>();
            foreach(SCTFunctionNode node in nodes)
            {
                if(HasAllInputsCalculated(node))
                {
                    ret += "\r\n" + ConstructFunctionCall(node);
                    SetOutputsAsCalclulated(node, true);
                    removeList.Add(node);         
                }
            }
   
            foreach(SCTFunctionNode rem in removeList)
            {
                nodes.Remove(rem);
            }
           
            if(nodes.Count > 0)
            {
                ret += ProcessNodes(ref nodes);
            }


            return ret;
        }



        public bool TranslateNetworkVertex(List<ISCTNode> nodes, List<Connection> connections, out string vertexShaderCode, out string status)
        {
            vertexShaderCode = string.Empty;
            status = string.Empty;
            string NL = "\r\n";
            string SP3 = "   ";
            
            //version
            vertexShaderCode += AttribVariableStrings.SHADER_VERSION_STR +NL + NL;
            //Layout in variables
            vertexShaderCode += "layout(location = 0) in vec3 " + AttribVariableStrings.IN_POSITION_VAR_STR +";"+ NL;
            vertexShaderCode += "layout(location = 1) in vec3 " + AttribVariableStrings.IN_NORMAL_VAR_STR + ";" + NL;
            vertexShaderCode += "layout(location = 2) in vec3 " + AttribVariableStrings.IN_TANGENT_VAR_STR + ";" + NL;
            vertexShaderCode += "layout(location = 3) in vec2 " + AttribVariableStrings.IN_UVS_VAR_STR + ";" + NL + NL + NL;


            vertexShaderCode += "uniform mat4 " + AttribVariableStrings.U_WORLD_MAT_VAR_NAME + ";" + NL;
            vertexShaderCode += "uniform mat4 " + AttribVariableStrings.U_INVERSE_WORLD_MAT_VAR_NAME + ";" + NL;
            vertexShaderCode += "uniform mat4 " + AttribVariableStrings.U_VIEW_MAT_VAR_NAME + ";" + NL;
            vertexShaderCode += "uniform mat4 " + AttribVariableStrings.U_MVP_MAT_VAR_NAME + ";" + NL;
            vertexShaderCode += "uniform vec3 " + AttribVariableStrings.U_CAMERA_POSITION_VAR_NAME + ";" + NL;
            vertexShaderCode += "uniform float " + AttribVariableStrings.U_TIME_VAR_NAME + ";" + NL + NL;

            string outVars = string.Empty;
            outVars += "//Passed to fragment shader" + NL;
            outVars += "out vec3 " + AttribVariableStrings.O_POSITION_VAR_NAMES[0] + ";" + NL;
            outVars += "out vec3 " + AttribVariableStrings.O_POSITION_VAR_NAMES[1] + ";" + NL;
            outVars += "out vec3 " + AttribVariableStrings.O_POSITION_VAR_NAMES[2] + ";" + NL;
            outVars += "out vec3 " + AttribVariableStrings.O_NORMAL_VAR_NAMES[0] + ";" + NL;
            outVars += "out vec3 " + AttribVariableStrings.O_NORMAL_VAR_NAMES[1] + ";" + NL;
            outVars += "out vec2 " + AttribVariableStrings.O_UV_VARIABLE_NAME + ";" + NL;
            outVars += "out vec3 " + AttribVariableStrings.O_CAMERA_POS_VAR_NAMES[0] + ";" + NL;
            outVars += "out float " + AttribVariableStrings.O_TIME_VARIABLE_NAME + ";" + NL;

            vertexShaderCode += outVars + NL;

            //main
            vertexShaderCode += "void main()\r\n{"+NL + NL;
            //LINE1
            vertexShaderCode += SP3 + AttribVariableStrings.O_POSITION_VAR_NAMES[0] + " = (" +
                AttribVariableStrings.U_WORLD_MAT_VAR_NAME + " * vec4(" + AttribVariableStrings.IN_POSITION_VAR_STR +
                ",1)).xyz;" + NL;
            //LINE2
            vertexShaderCode += SP3 + AttribVariableStrings.O_POSITION_VAR_NAMES[1] + " = " +
                AttribVariableStrings.IN_POSITION_VAR_STR + ";" + NL;
            //LINE3
            vertexShaderCode += SP3 + AttribVariableStrings.O_POSITION_VAR_NAMES[2] + " = (" +
             AttribVariableStrings.U_VIEW_MAT_VAR_NAME + " * vec4(" + AttribVariableStrings.IN_POSITION_VAR_STR +
             ",1)).xyz;" + NL + NL;

            //LINE 5
            vertexShaderCode += SP3 + AttribVariableStrings.O_NORMAL_VAR_NAMES[0] + " = mat3(" + AttribVariableStrings.U_INVERSE_WORLD_MAT_VAR_NAME +
                ") * " + AttribVariableStrings.IN_NORMAL_VAR_STR + ";" + NL;
            //LINE 6
            vertexShaderCode += SP3 + AttribVariableStrings.O_NORMAL_VAR_NAMES[1] + " = " + AttribVariableStrings.IN_NORMAL_VAR_STR + ";" + NL;

            //LINE 7
            vertexShaderCode += SP3 + AttribVariableStrings.O_UV_VARIABLE_NAME + " = " +
                AttribVariableStrings.IN_UVS_VAR_STR + ";" + NL;
            //LINE 8
            vertexShaderCode += SP3 + AttribVariableStrings.O_CAMERA_POS_VAR_NAMES[0] + " = " + 
                AttribVariableStrings.U_CAMERA_POSITION_VAR_NAME + ";" + NL;
            //LINE 9
            vertexShaderCode += SP3 + AttribVariableStrings.O_TIME_VARIABLE_NAME + " = " + 
                AttribVariableStrings.U_TIME_VAR_NAME + ";" + NL;

            //LINE10
            vertexShaderCode += NL + SP3 + "gl_Position = " + AttribVariableStrings.U_MVP_MAT_VAR_NAME + " * vec4(" +
                AttribVariableStrings.IN_POSITION_VAR_STR + ",1.0);" + NL + NL + "}";



            //if(TextFileReaderWriter.Read(VERTEX_BASE_PATH,out vertexShaderCode))
            //{
            //    status += "Reading vertex base OK.\r\n";
            //    return true;
            //}
            //status += "Reading vertex base FAILED!\r\n";
            return false;
        }

        public string ConstructFunctionCall(SCTFunctionNode node)
        {
            string output = string.Empty;

            List<Connector> outConnectors = node.GetAllConnectors(ConnectionDirection.Out);
            List<string> outVarParameters = new List<string>();

            //declare variable types for node output
            foreach(Connector c in outConnectors)
            {
                //if connected  create output variable
                if (c.Connected)
                {
                    string assembled = TranslateVariableType(c.VariableType) + "  " + c.ParentConnection.OutVariableName + ";\r\n";

                    outVarParameters.Add(c.ParentConnection.OutVariableName);// add to function call list
                    output += assembled; // add to forward declarations
                }
                else
                {
                    outVarParameters.Add("dummyOut" + TranslateVariableType(c.VariableType).ToUpper());// add to function call list
                }
            }

            output += node.NodeDescription.Name + "(";
            //place input variables inside function call
            List<Connector> inConnectors = node.GetAllConnectors(ConnectionDirection.In);
            foreach (Connector c in inConnectors)
            {
                //if connected use variable name associated with connection
                if (c.Connected)
                {
                    output += c.ParentConnection.OutVariableName + ", ";
                }

                else // otherwise use default value
                    output += ProcessDefaultInput(c) + ", ";

                
            }

            //place output variables inside function call
            for(int i = 0; i < outVarParameters.Count;++i)
            {
                output += outVarParameters[i];
                if (i < outVarParameters.Count - 1) output += ", ";
            }
            output += ");";

           // SCTConsole.Instance.PrintDebugLine("\r\n\r\n\r\n" + output);
            return output;
        }


        private string ProcessDefaultInput(Connector c)
        {
            if (!c.HasShaderVariableDescription) return  "no_description";
            string defaultValInfo = c.UsedShaderVariableDescription.AdditionalInfo;
            string ret = Regex.Replace(defaultValInfo, @"\s+", "");
            if (ret == string.Empty) return "no_default_value_provided";
            if (!ret.Contains("DEFAULT=")) return "DEFAULT_keyword_is_missing";
            ret = Regex.Replace(ret, "DEFAULT=", "");
            ret = ret.Trim();
            if(!m_DefaultValuesTable[c.UsedShaderVariableDescription.Type].IsMatch(ret))
            {
                return "value_is_incorrect";
            }
            string type = TranslateVariableType(c.UsedShaderVariableDescription.Type);
            ret = type + " " + ret;
            return ret;
        }

        private string CreateFunctionSignature(FunctionNodeDescription desc)
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
            s = s.Replace("SampleTexture", "texture2D");
            return s;
        }





        ///////////////////

        private bool HasAllInputsCalculated(SCTFunctionNode node)
        {
           
            foreach (Connector c in node.GetAllConnectors(ConnectionDirection.In))
            {
                if (!c.Connected) continue;
                if(!c.ParentConnection.IsProcessed)
                {
                    return false;
                }
            }
            return true; 
        }

        private void SetOutputsAsCalclulated(SCTFunctionNode node, bool calculated)
        {
            foreach (Connector c in node.GetAllConnectors(ConnectionDirection.Out))
            {
                if (!c.Connected) continue;
                if (calculated) c.ParentConnection.SetAsProcessed();
                else c.ParentConnection.SetAsUnprocessed();
            }
        }



    }
}
