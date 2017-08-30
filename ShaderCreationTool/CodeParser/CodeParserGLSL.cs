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
        private string m_VertexOutVars;


        public CodeParserGLSL()
        {
            m_Signatures = new List<string>();
            m_DefaultValuesTable = new Dictionary<ShaderVariableType, Regex>();
            m_DefaultValuesTable.Add(ShaderVariableType.Single, new Regex(@"^\d+\.*\d*$"));
            m_DefaultValuesTable.Add(ShaderVariableType.Vector2, new Regex(@"^\(\d+\.*\d*\,\d+\.*\d*\)$"));
            m_DefaultValuesTable.Add(ShaderVariableType.Vector3, new Regex(@"^\(\d+\.*\d*\,\d+\.*\d*\,\d+\.*\d*\)$"));
            m_DefaultValuesTable.Add(ShaderVariableType.Vector4, new Regex(@"^\(\d+\.*\d*\,\d+\.*\d*\,\d+\.*\d*\,\d+\.*\d*\)$"));
        }

    


        public bool TranslateNetworkVertex(List<ISCTNode> nodes, List<Connection> connections, out string vertexShaderCode, out string status)
        {
            vertexShaderCode = string.Empty;
            status = string.Empty;
            string NL = "\r\n";
            string TAB = "   ";

            //version
            vertexShaderCode += AttribVariableStrings.SHADER_VERSION_STR + NL + NL;
            //Layout in variables
            vertexShaderCode += "layout(location = 0) in vec3 " + AttribVariableStrings.IN_POSITION_VAR_STR + ";" + NL;
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
            m_VertexOutVars = outVars;

            vertexShaderCode += outVars + NL;

            //main
            vertexShaderCode += "void main()\r\n{" + NL + NL;
            //LINE1
            vertexShaderCode += TAB + AttribVariableStrings.O_POSITION_VAR_NAMES[0] + " = (" +
                AttribVariableStrings.U_WORLD_MAT_VAR_NAME + " * vec4(" + AttribVariableStrings.IN_POSITION_VAR_STR +
                ",1)).xyz;" + NL;
            //LINE2
            vertexShaderCode += TAB + AttribVariableStrings.O_POSITION_VAR_NAMES[1] + " = " +
                AttribVariableStrings.IN_POSITION_VAR_STR + ";" + NL;
            //LINE3
            vertexShaderCode += TAB + AttribVariableStrings.O_POSITION_VAR_NAMES[2] + " = (" +
             AttribVariableStrings.U_VIEW_MAT_VAR_NAME + " * vec4(" + AttribVariableStrings.IN_POSITION_VAR_STR +
             ",1)).xyz;" + NL + NL;

            //LINE 5
            vertexShaderCode += TAB + AttribVariableStrings.O_NORMAL_VAR_NAMES[0] +
                " = mat3(" + AttribVariableStrings.U_INVERSE_WORLD_MAT_VAR_NAME +
                ") * " + AttribVariableStrings.IN_NORMAL_VAR_STR + ";" + NL;
            //LINE 6
            vertexShaderCode += TAB + AttribVariableStrings.O_NORMAL_VAR_NAMES[1] + " = " + 
                AttribVariableStrings.IN_NORMAL_VAR_STR + ";" + NL;

            //LINE 7
            vertexShaderCode += TAB + AttribVariableStrings.O_UV_VARIABLE_NAME + " = " +
                AttribVariableStrings.IN_UVS_VAR_STR + ";" + NL;
            //LINE 8
            vertexShaderCode += TAB + AttribVariableStrings.O_CAMERA_POS_VAR_NAMES[0] + " = " +
                AttribVariableStrings.U_CAMERA_POSITION_VAR_NAME + ";" + NL;
            //LINE 9
            vertexShaderCode += TAB + AttribVariableStrings.O_TIME_VARIABLE_NAME + " = " +
                AttribVariableStrings.U_TIME_VAR_NAME + ";" + NL;

            //LINE10
            vertexShaderCode += NL + TAB + "gl_Position = " + AttribVariableStrings.U_MVP_MAT_VAR_NAME + " * vec4(" +
                AttribVariableStrings.IN_POSITION_VAR_STR + ",1.0);" + NL + NL + "}";

            status += "FINISHED\r\n";
            return true;
        }

        public bool TranslateNetworkFragment(
            List<ISCTNode> nodes, 
            List<Connection> connections, 
            out string code, out string status)
        {
            code = "";
            FrameBufferNode fbNode = (FrameBufferNode)nodes.Find(o => o is FrameBufferNode);
            Connector con = fbNode.GetConnector(ConnectionDirection.In, 1);
            if(!con.Connected)
            {
                status = "FRAGMENT_NOT_CONNECTED";
                return false;
            }


            List<IInputNode> inputNodes = (nodes.FindAll(o => o is IInputNode)).Cast<IInputNode>().ToList();
            List<SCTFunctionNode> nodesToProcess = nodes.FindAll(o => o is SCTFunctionNode).Cast<SCTFunctionNode>().ToList();
            List<SCTFunctionNode> tempRestoreStateNodeList = new List<SCTFunctionNode>(nodesToProcess);
            List<Connection> inputConnections = connections.FindAll(o => o.IsDirectInputConnection);
            foreach (Connection c in inputConnections)
            {
                c.Highlighted = true;
            }

            m_VertexOutVars = m_VertexOutVars.Replace("out ", "in ").Replace("to fragment", "from vertex");
            code += AttribVariableStrings.SHADER_VERSION_STR + "\r\n\r\n" + m_VertexOutVars + "\r\n\r\n";
            code += "out vec4 FragColour;\r\n\r\n";
            //m_VertexOutVars = string.Empty;

            // Input uniform variables
            string codeUniforms;
            TranslateInputVariables(inputNodes, out codeUniforms, out status);
            code += codeUniforms;


            string codeFunctions;
            string statusFunctions;

            // Functions extraction
            TranslateNodeListIntoFunctions(nodesToProcess, out codeFunctions, out statusFunctions);
            code += "\r\n" + codeFunctions;

            // network (function calls)
            string codeNodes = ProcessNodes(ref nodesToProcess);
            string ss = "FragColour = " + con.ParentConnection.OutVariableName + ";\r\n";

            code += "\r\n\r\n void main()\r\n{\r\n" + codeNodes + "\r\n" + ss + "\r\n}";         

            foreach(SCTFunctionNode node in tempRestoreStateNodeList)
            {
                SetOutputsAsCalclulated(node, false);
            }
            status = "OK\r\n";
            return true;
        }


        private bool TranslateNodeIntoFunction(SCTFunctionNode node, out string functionCode, out string status)
        {
            functionCode = "";
            status = "";
            try
            {
                FunctionNodeDescription desc = node.NodeDescription;
                string signature = CreateFunctionSignature(desc);
                if (m_Signatures.Contains(signature))
                {
                    status = "Repeated signature: " + signature ;
                    status += "   SKIPPING..\r\n";
                    return true;
                }
                string body = "{\r\n";
                body += ParseCode(desc.GetFunctionString());
                body += "\r\n}\r\n";
                functionCode = signature + body;
                m_Signatures.Add(signature);
                // SCTConsole.Instance.PrintDebugLine(functionCode);
            }
            catch (Exception e)
            {
                status = e.Message;
                return false;
            }
            status = "OK\r\n";
            return true;
        }

        private bool TranslateNodeListIntoFunctions(List<SCTFunctionNode> nodes, out string functionCode, out string status)
        {
            m_Signatures.Clear();
            functionCode = "// SCT Nodes Translated as functions.\r\n";
            status = "";
            bool allOk = true;
            foreach (SCTFunctionNode node in nodes)
            {
                string tempFunctionCode;
                string tempStatus;
                if (TranslateNodeIntoFunction(node, out tempFunctionCode, out tempStatus))
                {
                    functionCode += tempFunctionCode + "\r\n";
                }
                else
                {
                    allOk = false;
                }
                status += tempStatus;
            }

            return allOk;
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
       

        private bool TranslateInputVariables(List<IInputNode> inputNodes, out string declarationsCode, out string status)
        {
            declarationsCode = "//Uniform nodes translated as uniform variables.\r\n";
            status = "Detected user uniform variables:\r\n";
            const int maxTypeCharCount = 15;
            bool ok = true;
            foreach (IInputNode node in inputNodes)
            {
                string typeStr = TranslateVariableType(node.GetShaderVariableType());
                declarationsCode += "uniform " + typeStr + " " + new string(' ', maxTypeCharCount - typeStr.Length) + node.GetVariableName() + ";\r\n";
                if (typeStr == string.Empty) ok = false;
                status += declarationsCode + "\r\n";
            }
            return ok;
        }

       

        private string ConstructFunctionCall(SCTFunctionNode node)
        {
            string output = "//SCT CALL: " + node.NodeDescription.Name + "\r\n";

            List<Connector> outConnectors = node.GetAllConnectors(ConnectionDirection.Out);
            List<string> outVarParameters = new List<string>();

            //declare variable types for node output
            foreach(Connector c in outConnectors)
            {
                //if connected  create output variable
                if (c.Connected)
                {
                    string assembled = TranslateVariableType(c.VariableType) + "  " + 
                        c.ParentConnection.OutVariableName + ";\r\n";
                    outVarParameters.Add(c.ParentConnection.OutVariableName);// add to function call list
                    output += "   " + assembled; // add to forward declarations
                }
                else
                {
                    // add to function call list
                    outVarParameters.Add("    dummyOut" + TranslateVariableType(c.VariableType).ToUpper());
                }
            }

            output += "   " + node.NodeDescription.Name + "(";
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
            output += ");\r\n";

            return output;
        }


        private string ProcessDefaultInput(Connector c)
        {
            if (!c.HasShaderVariableDescription) return  "no_description";
            string defaultValInfo = c.UsedShaderVariableDescription.AdditionalInfo;
            string ret = Regex.Replace(defaultValInfo, @"\s+", "");//remove all spaces
            if (ret == string.Empty) return "no_default_value_provided";
            if (!ret.Contains("DEFAULT=")) return "DEFAULT_keyword_is_missing";
            ret = Regex.Replace(ret, "DEFAULT=", "");
            ret = ret.Trim();
            if(!m_DefaultValuesTable[c.UsedShaderVariableDescription.Type].IsMatch(ret))
            {
                return "value_is_incorrect";
            }
            string type = TranslateVariableType(c.UsedShaderVariableDescription.Type);
            if(type != "float") ret = type + " " + ret;
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
            s = s.Replace("VECTOR4", "vec4").Replace("VECTOR3", "vec3").Replace("VECTOR2", "vec2").Replace("FLOAT", "float").Replace("COLOUR", "vec4");
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
