using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System;

namespace ShaderCreationTool
{
    class ReaderXML
    {

        private const string INPUT_VARIABLES = "Input_Variables";
        private const string OUTPUT_VARIABLES = "Output_Variables";
        private const string CODE = "Code";
        private const string UTIL_FUNCT = "Util_Function";
        private static readonly Regex s_VariableCorrectnessCriteria = new Regex("^[a-zA-Z0-9_]*$");



        public static bool ReadInDescriptions(string path, out List<FunctionNodeDescription> outputList, out string status)
        {
            outputList = new List<FunctionNodeDescription>();
            status = string.Empty;
            XmlNodeList nodes;
            try
            {
                string xmlContent = File.ReadAllText(path);

                XmlDocument XdOC = new XmlDocument();
                XdOC.LoadXml(xmlContent);

                XmlElement document = XdOC.DocumentElement;

                //All Nodes - ~First one is a comment rest are entities
                nodes = document.ChildNodes;
            }
            catch(Exception e)
            {
                status += e.Message;
                return false;
            }


            foreach (XmlNode node in nodes)
            {
                if (node.Name != "Node") continue; //skip all non  nodes

                FunctionNodeDescription desc;
                if (ReadNode(node, out desc, ref status))
                {
                    outputList.Add(desc);

                }
            }

            return true;
        }



        public static bool ReadInCommonCode(string path, out string code)
        {

            code = string.Empty;
            XmlNodeList nodes;
            try
            {
                string xmlContent = File.ReadAllText(path);

                XmlDocument XdOC = new XmlDocument();
                XdOC.LoadXml(xmlContent);

                XmlElement document = XdOC.DocumentElement;

                //All Nodes - ~First one is a comment rest are entities
                nodes = document.ChildNodes;
            }
            catch
            {
                return false;
            }


            foreach (XmlNode node in nodes)
            {
                if (node.Name != "COMMON_CODE") continue; //skip all non  nodes
                code += "\r\n" + node.InnerText;
               
            }

            return true;
        }


        public static string[] GetNamesList(List<FunctionNodeDescription> descriptions)
        {
            string[] names = new string[descriptions.Count];
            for (int i = 0; i < descriptions.Count; ++i)
            {
                names[i] = descriptions[i].Name;
            }
            return names;
        }

        ///////////////////////////////////////////   PRIVATE METHODS ///////////////////////////////////////

        //According to the C# specification, switch-case tables are compiled to constant hash jump tables. 
        //That is, they are constant dictionaries, not a series of if-else statements.
        private static bool GetType(string str, out ShaderVariableType type)
        {
            type = ShaderVariableType.Single;
            str = str.ToLower();
            switch (str)
            {
                case "float": { type = ShaderVariableType.Single; return true; }
                case "float2": { type = ShaderVariableType.Vector2; return true; }
                case "float3": { type = ShaderVariableType.Vector3; return true; }
                case "float4": { type = ShaderVariableType.Vector4; return true; }
                case "texture2d": { type = ShaderVariableType.Texture2D; return true; }
                default: { return false; }
            }
        }


        static private bool ReadNode(XmlNode node, out FunctionNodeDescription nodeDescription, ref string status)
        {

            nodeDescription = null;
            string nodeName = string.Empty;
            foreach (XmlAttribute attrib in node.Attributes)
            {
                if (attrib.Name == "Name") nodeName = attrib.Value;
            }
            if (nodeName == string.Empty)
            {
                status = "Node Read failed! One of the nodes misses name attributes.";
                return false;
            }
            nodeDescription = new FunctionNodeDescription(nodeName);

            XmlNodeList groups = node.ChildNodes;
            foreach (XmlNode group in groups)
            {
                if (group.Name == INPUT_VARIABLES)
                {
                    ReadInputVariables(group, ref nodeDescription, ref status);
                }
                else if (group.Name == OUTPUT_VARIABLES)
                {
                    ReadOutputVariables(group, ref nodeDescription, ref status);
                }
                else if (group.Name == CODE)
                {
                    nodeDescription.SetFucntionString(ReadCode(group));
                }
                else if (group.Name == UTIL_FUNCT)
                {
                    SubFuntionDescription ufd;
                    if (ReadUtilFunction(group, out ufd))
                    {
                        nodeDescription.AddUtilFunctDescription(ufd);
                    }
                }
            }
            status += "Node: " + nodeName + " read Ok\n";
            return true;

        }

        static private bool ReadUtilFunction(XmlNode group, out SubFuntionDescription ufdesc)
        {
            ufdesc = null;
            string name = string.Empty;
            ShaderVariableType type = ShaderVariableType.Single;
            foreach (XmlAttribute attrib in group.Attributes)
            {
                if (attrib.Name == "Name") name = attrib.Value;
                if(attrib.Name == "Returns")
                {
                    if (!GetType(attrib.Value, out type)) return false;
                }
            }
            ufdesc = new SubFuntionDescription(name, type);
            string stat= string.Empty;
            ReadInputVariables(group, ref ufdesc, ref stat);
            foreach(XmlNode nd in group.ChildNodes)
            {
                if (nd.Name != CODE) continue;
                ufdesc.SetFucntionString(ReadCode(nd));
            }
            return true;
        }

        static private string ReadCode(XmlNode codeNode)
        {
            return codeNode.InnerText;
        }


        static private void ReadInputVariables(XmlNode groupIn, ref FunctionNodeDescription desc, ref string status)
        {
            foreach (XmlNode variable in groupIn.ChildNodes)
            {
                ShaderVariableDescription varDesc;
                if (ReadVariable(variable, ConnectionDirection.In, out varDesc, ref status))
                {
                    desc.AddInputVariable(varDesc);
                }
                else
                {
                    status += "ERROR: Failed to read input variable!\n";
                }
            }
            status += "OK: Input Variables end.\n";
        }


        static private void ReadInputVariables(XmlNode groupIn, ref SubFuntionDescription desc, ref string status)
        {
            foreach (XmlNode variable in groupIn.ChildNodes)
            {
                ShaderVariableDescription varDesc;
                if (ReadVariable(variable, ConnectionDirection.In, out varDesc, ref status))
                {
                    desc.AddInputVariable(varDesc);
                }
                else
                {
                    status += "ERROR: Failed to read input variable!\n";
                }
            }
            status += "OK: Input Variables end.\n";
        }

        static private void ReadOutputVariables(XmlNode groupIn, ref FunctionNodeDescription desc, ref string status)
        {
            foreach (XmlNode variable in groupIn.ChildNodes)
            {
                ShaderVariableDescription varDesc;
                if (ReadVariable(variable, ConnectionDirection.Out, out varDesc, ref status))
                {
                    desc.AddOutputVariable(varDesc);
                }
                else
                {
                    status += "ERROR: Failed to read output variable!\n";
                }
            }
            status += "OK: Output variables end.\n";
        }

        static private bool ReadVariable(XmlNode variableNode, ConnectionDirection direction, out ShaderVariableDescription varDesc, ref string status)
        {
            varDesc = null;
            if (variableNode.Name != "Variable")
            {
                status += "Error: Non-variable node in variables section\n";
                return false;
            }
            string name = string.Empty;
            string type = string.Empty;
            foreach (XmlAttribute attrib in variableNode.Attributes)
            {
                if (attrib.Name == "Name")
                {
                    name = attrib.Value;
                }
                else if (attrib.Name == "Var_Type")
                {
                    type = attrib.Value;
                }
            }

            //Validity check
            if (name == string.Empty)
            {
                status += "ERROR: Variable name is empty!\n";
                return false;
            }
            if (type == string.Empty || type.Contains(" "))
            {
                status += "ERROR: Variable type undefined!\n";
                return false;
            }
            if (name.Contains(" "))
            {
                status += "ERROR: Variable name contains spaces!\n";
                return false;
            }
            if (char.IsDigit(name[0]))
            {
                status += "ERROR: Variable name starts with digit!\n";
                return false;
            }
            if (!s_VariableCorrectnessCriteria.IsMatch(name))
            {
                status += "ERROR: Variable name contains illegal symbols!\n";
                return false;
            }
            ShaderVariableType outType;
            if (GetType(type, out outType))
            {
                varDesc = new ShaderVariableDescription(name, outType, direction);
                varDesc.AdditionalInfo = variableNode.InnerText;
               // SCTConsole.Instance.PrintDebugLine(varDesc.AdditionalInfo);
            }
            else
            {
                status += "ERROR: Variable type is unrecognisible!\n";
                return false;
            }

            return true;
        }

    }
}
