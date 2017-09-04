using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using System.Windows.Forms;
using System.Drawing;

namespace ShaderCreationTool
{
    
    class XmlNodeSerializer
    {

        public static OnPlaceNodeCallback PlaceNodeCalback = null;


        public static XmlWriter SerializeColour(XmlWriter target, ShaderVectorVariable variable)
        {
            target.WriteStartElement("SHADER_COLOUR_VARIABLE");
            target.WriteAttributeString("R", variable.GetR().ToString());
            target.WriteAttributeString("G", variable.GetG().ToString());
            target.WriteAttributeString("B", variable.GetB().ToString());
            target.WriteAttributeString("A", variable.GetA().ToString());
            target.WriteAttributeString("VARNAME", variable.GetName());
            target.WriteEndElement();
            return target;
        }

        public static XmlWriter SerializeVector(XmlWriter target, ShaderVectorVariable variable)
        {
            string[] names = { "X", "Y", "Z", "W" };
            target.WriteStartElement("SHADER_VECTOR_VARIABLE");
            for (uint i = 0; i < variable.GetSize(); ++i)
            {
                target.WriteAttributeString(names[i], variable.GetElement(i).ToString());
            }
            target.WriteAttributeString("VARNAME", variable.GetName());
            target.WriteEndElement();
            return target;
        }

        public static XmlWriter SerializeTextureVariable(XmlWriter target, ShaderTextureVariable variable)
        {
            target.WriteStartElement("SHADER_TEXTURE_VARIABLE");
            target.WriteAttributeString("PATH", variable.GetPath());
            target.WriteAttributeString("VARNAME", variable.GetName());
            target.WriteEndElement();
            return target;
        }

        public static XmlWriter SerializePosition(XmlWriter target, Point position)
        {
            target.WriteStartElement("POSITION");
            target.WriteAttributeString("X", position.X.ToString());
            target.WriteAttributeString("Y", position.Y.ToString());
            target.WriteEndElement();//POSITION
            return target;
        }

        public static XmlWriter SerializeShaderVariableDescription(XmlWriter target, ShaderVariableDescription desc)
        {
            target.WriteStartElement("SHADER_VAR_DESC");
            target.WriteAttributeString("NAME", desc.Name);
            target.WriteAttributeString("TYPE", desc.Type.ToString());
            target.WriteAttributeString("CONNECTION_DIRECTION", desc.ConnectionDirection.ToString());
            target.WriteElementString("ADDITIONAL_INFO",desc.AdditionalInfo);
            target.WriteEndElement();//SHADER_VAR_DESC
            return target;
        }

        public static XmlWriter SerializeFunctionNodeDescription(XmlWriter target, FunctionNodeDescription desc)
        {
            target.WriteStartElement("FUNCTION_NODE_DESC");
            target.WriteAttributeString("NAME", desc.Name);
            target.WriteElementString("CODE", desc.GetFunctionString());

            target.WriteStartElement("INPUT_VARIABLES");
            for(int i = 0; i < desc.InputCount;++i)
            {
                SerializeShaderVariableDescription(target, desc.GetInVariableDescription(i));
            }
            target.WriteEndElement();// INPUT_VARIABLES

            target.WriteStartElement("OUTPUT_VARIABLES");
            for (int i = 0; i < desc.OutputCount; ++i)
            {
                SerializeShaderVariableDescription(target, desc.GetOutVariableDescription(i));
            }
            target.WriteEndElement();// OUTPUT_VARIABLES



            target.WriteEndElement();//Function Node Desc

            return target;
        }
           

        public static XmlWriter SerializeColourInputNode(XmlWriter target, InputNodeColour node)
        {
            target.WriteStartElement("UNIFORM_INPUT_NODE");
            target.WriteAttributeString("TYPE", NodeType.Input_Colour.ToString());
            target.WriteAttributeString("ID", node.GetNodeID().ToString());
            SerializePosition(target, node.GetPosition());
            SerializeColour(target, node.GetRawShaderVariable());
            target.WriteEndElement();//NODE

            return target;
        }
        // SERIALIZE NUMERIC VALUE NODE
        public static XmlWriter SerializeVectorInputNode(XmlWriter target, InputNodeVector node)
        {
            target.WriteStartElement("UNIFORM_INPUT_NODE");
            target.WriteAttributeString("TYPE", node.GetNodeType().ToString());
            target.WriteAttributeString("ID", node.GetNodeID().ToString());
            SerializePosition(target, node.GetPosition());
            SerializeVector(target, node.GetRawShaderVariable());
            target.WriteEndElement();//NODE

            return target;
        }

        // SERIALIZE TEXTURE NODE
        public static XmlWriter SerializeTextureInputNode(XmlWriter target, InputNodeTexture2D node)
        {
            target.WriteStartElement("UNIFORM_INPUT_NODE");
            target.WriteAttributeString("TYPE", node.GetNodeType().ToString());
            target.WriteAttributeString("ID", node.GetNodeID().ToString());
            SerializePosition(target, node.GetPosition());
            SerializeTextureVariable(target, node.GetRawShaderVariable());
            target.WriteEndElement();//NODE

            return target;
        }

        // SERIALIZE SIMPLE ATTRIB NODE
        public static XmlWriter SerializeAttribNodeSimple(XmlWriter target, AttribNodeSimple node)
        {
            target.WriteStartElement("SIMPLE_ATTRIB_NODE");
            target.WriteAttributeString("TYPE", node.GetNodeType().ToString());
            target.WriteAttributeString("ID", node.GetNodeID().ToString());
            SerializePosition(target, node.GetPosition());
            target.WriteEndElement();//NODE

            return target;
        }

        // SERIALIZE SELECTION ATTRIB NODE
        public static XmlWriter SerializeAttribNodeWithSelection(XmlWriter target, AttribNodeWithSelection node)
        {
            target.WriteStartElement("SELECTION_ATTRIB_NODE");
            target.WriteAttributeString("TYPE", node.GetNodeType().ToString());
            target.WriteAttributeString("ID", node.GetNodeID().ToString());
            target.WriteAttributeString("SELECTED_INDEX", node.GetSelectedIndex().ToString());
            SerializePosition(target, node.GetPosition());
            target.WriteEndElement();//NODE

            return target;
        }

        // SERIALIZE FUNCTION NODE
        public static XmlWriter SerializeFunctionNode(XmlWriter target, SCTFunctionNode node)
        {
            target.WriteStartElement("FUNCTION_NODE");
           // target.WriteComment("Function name is: " + node.NodeDescription.Name);
            target.WriteAttributeString("UNIQUE_ID", node.GetNodeID().ToString());
            SerializePosition(target, node.GetPosition());
            SerializeFunctionNodeDescription(target, node.NodeDescription);
            target.WriteEndElement();
            return target;
        }

        // SERIALIZE TARGET NODE
        public static XmlWriter SerializeTargegNode(XmlWriter target, FrameBufferNode node)
        {
            target.WriteStartElement("TARGET_NODE");
            target.WriteAttributeString("UNIQUE_ID", node.GetNodeID().ToString());
            SerializePosition(target, node.GetPosition());
            target.WriteEndElement();
            return target;
        }

        public static void Save(string path, List<ISCTNode> nodes)
        {

            List<IInputNode> inputNodes = nodes.FindAll(o => o is IInputNode).Cast<IInputNode>().ToList();
            List<AttribNodeSimple> simpleAttribNodes = nodes.FindAll(o => o is AttribNodeSimple).Cast<AttribNodeSimple>().ToList();
            List<AttribNodeWithSelection> selectionAttribNodes  = nodes.FindAll(o => o is AttribNodeWithSelection).Cast<AttribNodeWithSelection>().ToList();
            List<SCTFunctionNode> functionNodes = nodes.FindAll(o => o is SCTFunctionNode).Cast<SCTFunctionNode>().ToList();
            FrameBufferNode targetNode = (FrameBufferNode)nodes.Find(o => o is FrameBufferNode);



            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(path, settings);
            writer.WriteStartDocument();
            writer.WriteComment("SHADER CREATION TOOL NETWORK FILE");
            writer.WriteStartElement("SCT_NETWORK");

            writer.WriteStartElement("INPUT_NODES");
            foreach (IInputNode inputNode in inputNodes)
            {
                inputNode.Serialize(writer);
            }
            writer.WriteEndElement();//INPUT NODES


            writer.WriteStartElement("SIMPLE_ATTRIB_NODES");
            foreach (AttribNodeSimple simpleAttribNode in simpleAttribNodes)
            {
                simpleAttribNode.Serialize(writer);
            }
            writer.WriteEndElement();//SIMPLE_ATTRIB_NODES

            writer.WriteStartElement("SELECTION_ATTRIB_NODES");
            foreach (AttribNodeWithSelection selectionAttribNode in selectionAttribNodes)
            {
                selectionAttribNode.Serialize(writer);
            }
            writer.WriteEndElement();//SIMPLE_ATTRIB_NODES

            writer.WriteStartElement("FUNCTION_NODES");
            foreach (SCTFunctionNode functionNode in functionNodes)
            {
                functionNode .Serialize(writer);
            }
            writer.WriteEndElement();//FUNCTION NODES

            SerializeTargegNode(writer, targetNode);

            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Flush();
            writer.Close();

        }

        ////////////////////////////// DESERIALIZATION ///////////////////////////////////////////

       public static bool DeserializePosition(XmlNode node, out Point position)
        {

            try
            {
                int x = 0;
                int y = 0;
                foreach(XmlAttribute attrib in node.Attributes)
                {
                    if (attrib.Name == "X") x = int.Parse(attrib.Value);
                    if (attrib.Name == "Y") y = int.Parse(attrib.Value);
                }
                position = new Point(x, y);
                return true;
            }
            catch
            {
                position = new Point(0, 0);
                return false;
            }
            
        }

        public static bool DeserializeFrameBufferNode(XmlNode node, ref FrameBufferNode fbNode)
        {
    
            foreach (XmlNode nd in node.ChildNodes)
            {
                if(nd.Name == "POSITION")
                {
                    int x = int.Parse(nd.Attributes[0].Value);
                    int y = int.Parse(nd.Attributes[1].Value);
                    fbNode.SetPosition(new Point(x, y));
                }
            } 
            return true;
        }

        public static bool DeserializeInputNode(XmlNode xmlNode, out IInputNode inNode)
        {
            inNode = null;
            if (xmlNode.Attributes.Count < 2) return false;

            NodeType type = NodeType.Target;
            string id = string.Empty;
           foreach (XmlAttribute attrib in xmlNode.Attributes)
           {
                if(attrib.Name == "TYPE")
                {
                    type = (NodeType)Enum.Parse(typeof(NodeType), attrib.Value);
                }
                else if (attrib.Name == "ID")
                {
                    id = attrib.Value;
                }
           }
            if (type == NodeType.Target) return false;
            if (id == string.Empty) return false;

            SCTConsole.Instance.PrintDebugLine("WE ARE HERE");
            SCTConsole.Instance.PrintDebugLine("ID IS: " + id);
            SCTConsole.Instance.PrintDebugLine("TYPE: " + type.ToString());

            /// Positon
            Point position = new Point(0,0);
            /// Variable name
            string varName = string.Empty;

            string[] data = new string[4];

            foreach (XmlNode child in xmlNode.ChildNodes)
            {
                if (child.Name == "POSITION") DeserializePosition(child, out position);
                if (child.Name == "SHADER_TEXTURE_VARIABLE")// seserialize textire
                {
                    foreach (XmlAttribute attrib in child.Attributes)
                    {
                        if (attrib.Name == "PATH") data[0] = attrib.Value;
                        else if (attrib.Name == "VARNAME") varName = attrib.Value;
                    }
                }
               else if(child.Name == "SHADER_COLOUR_VARIABLE")
               {
                    foreach (XmlAttribute attrib in child.Attributes)
                    {
                        if (attrib.Name == "R") data[0] = attrib.Value;
                        else if (attrib.Name == "G") data[1] = attrib.Value;
                        else if (attrib.Name == "B") data[2] = attrib.Value;
                        else if (attrib.Name == "A") data[3] = attrib.Value;
                        else if (attrib.Name == "VARNAME") varName = attrib.Value;
                    }
                }
            }

            if (PlaceNodeCalback != null)
            {
                inNode = (IInputNode)PlaceNodeCalback(position, type);
                if(type == NodeType.Input_Texture2D)
                {
                    InputNodeTexture2D nd = (InputNodeTexture2D)inNode;
                    nd.ChangeTexture(data[0]);
                    nd.ChangeVarName(varName);
                }
                else if (type == NodeType.Input_Colour)
                {
                    InputNodeColour nd = (InputNodeColour)inNode;
                    nd.ChangeVariableName(varName);
                    nd.ChangeColour(float.Parse(data[0]), float.Parse(data[1]), float.Parse(data[2]), float.Parse(data[3]));
                   
                }
           } 
            return true;
        }


        public static bool  ReadNodes(string path, ref List<ISCTNode> allNodes)
        {
            
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
                SCTConsole.Instance.PrintDebugLine("Category name: " + node.Name);
                if (node.Name == "INPUT_NODES")
                {
                    // process input nodes
                    foreach(XmlNode inNode in node.ChildNodes)
                    {
                        if (inNode.Name != "UNIFORM_INPUT_NODE") continue;
                        IInputNode result;
                        DeserializeInputNode(inNode, out result);
                        
                    }
                  
                }
                else if(node.Name == "SIMPLE_ATTRIB_NODES")
                {
                    // process simple attrib nodes
                    Console.Write("pruk");
                }
                else if(node.Name == "SELECTION_ATTRIB_NODES")
                {
                    // selection attrib nodes
                    Console.Write("pruk");
                }
                else if(node.Name == "FUNCTION_NODES")
                {
                    //proccess function nodes
                    Console.Write("pruk");
                }
                else if(node.Name == "TARGET_NODE")
                {
                    // process target node
                    FrameBufferNode fbn = (FrameBufferNode)allNodes.Find(o => o is FrameBufferNode);
                    DeserializeFrameBufferNode(node, ref fbn);

                }
                

            }

            return true;
        }



    }
}
