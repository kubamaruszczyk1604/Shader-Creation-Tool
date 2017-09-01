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

        public static void Save(string path, List<ISCTNode> nodes)
        {

            List<IInputNode> inputNodes = nodes.FindAll(o => o is IInputNode).Cast<IInputNode>().ToList();
            List<SCTFunctionNode> functionNodes = nodes.FindAll(o => o is SCTFunctionNode).Cast<SCTFunctionNode>().ToList();
            FrameBufferNode targetNode = (FrameBufferNode)nodes.Find(o => o is FrameBufferNode);



            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(path, settings);
            writer.WriteStartDocument();
            writer.WriteComment("SHADER CREATION TOOL NETWORK FILE");


            writer.WriteStartElement("INPUT_NODES");

            foreach (IInputNode inputNode in inputNodes)
            {
                inputNode.Serialize(writer);
            }

            writer.WriteEndElement();//INPUT NODES


            writer.WriteEndDocument();

            writer.Flush();
            writer.Close();

        }



    }
}
