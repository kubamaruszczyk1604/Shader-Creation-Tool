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
        public static Control EditAreaPanel = null;

        public static XmlWriter SerializeStaticCounters(XmlWriter target, 
            int inTextureCnt,
            int inColourCnt,
            int inNumCnt,
            int attrSimpleCnt,
            int attrSelCnt, 
            int functionCnt)
        {
            target.WriteStartElement("STATIC_COUNTERS");
            target.WriteAttributeString("IN_TEX", inTextureCnt.ToString());
            target.WriteAttributeString("IN_COL", inColourCnt.ToString());
            target.WriteAttributeString("IN_NUM", inNumCnt.ToString());
            target.WriteAttributeString("ASIMPLE", attrSimpleCnt.ToString());
            target.WriteAttributeString("ASELECT", attrSelCnt.ToString());
            target.WriteAttributeString("FUNCTION", functionCnt.ToString());
            target.WriteEndElement();

            return target;
        }

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
            target.WriteElementString("ADDITIONAL_INFO", desc.AdditionalInfo);
            target.WriteEndElement();//SHADER_VAR_DESC
            return target;
        }

        public static XmlWriter SerializeFunctionNodeDescription(XmlWriter target, FunctionNodeDescription desc)
        {
            target.WriteStartElement("FUNCTION_NODE_DESC");
            target.WriteAttributeString("NAME", desc.Name);
            target.WriteElementString("CODE", desc.GetFunctionString());

            target.WriteStartElement("INPUT_VARIABLES");
            for (int i = 0; i < desc.InputCount; ++i)
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
        public static XmlWriter SerializeTargetNode(XmlWriter target, FrameBufferNode node)
        {
            target.WriteStartElement("TARGET_NODE");
            target.WriteAttributeString("UNIQUE_ID", node.GetNodeID().ToString());
            SerializePosition(target, node.GetPosition());
            target.WriteEndElement();
            return target;
        }

        //SERIALIZE CONNETION
        public static XmlWriter SerializeConnection(XmlWriter target, Connection connection)
        {
            target.WriteStartElement("CONNECTION");
            target.WriteAttributeString("SOURCE_NODE_ID", connection.SourceConnector.ParentNode.GetNodeID());
            target.WriteAttributeString("DESTINATION_NODE_ID", connection.DestinationConnector.ParentNode.GetNodeID());
            target.WriteAttributeString("SOURCE_CON_LOCAL_ID", connection.SourceConnector.LocalID);
            target.WriteAttributeString("DESTINATION_CON_LOCAL_ID", connection.DestinationConnector.LocalID);
            Point a;
            Point b;
            connection.GetLineConfig(out a, out b);
            target.WriteAttributeString("AX", a.X.ToString());
            target.WriteAttributeString("AY", a.Y.ToString());
            target.WriteAttributeString("BX", b.X.ToString());
            target.WriteAttributeString("BY", b.Y.ToString());
            target.WriteEndElement();
            return target;
        }

        public static void Save(string path, List<ISCTNode> nodes, List<Connection> connections)
        {

            List<IInputNode> inputNodes = nodes.FindAll(o => o is IInputNode).Cast<IInputNode>().ToList();
            List<AttribNodeSimple> simpleAttribNodes = nodes.FindAll(o => o is AttribNodeSimple).Cast<AttribNodeSimple>().ToList();
            List<AttribNodeWithSelection> selectionAttribNodes = nodes.FindAll(o => o is AttribNodeWithSelection).Cast<AttribNodeWithSelection>().ToList();
            List<SCTFunctionNode> functionNodes = nodes.FindAll(o => o is SCTFunctionNode).Cast<SCTFunctionNode>().ToList();
            FrameBufferNode targetNode = (FrameBufferNode)nodes.Find(o => o is FrameBufferNode);



            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineOnAttributes = true;
            XmlWriter writer = XmlWriter.Create(path, settings);
            writer.WriteStartDocument();
            writer.WriteComment("SHADER CREATION TOOL NETWORK FILE");
            writer.WriteStartElement("SCT_NETWORK");

            SerializeStaticCounters(writer, 
                InputNodeTexture2D.CounterState(), 
                InputNodeColour.CounterState(),
                InputNodeVector.CounterState(),
                AttribNodeSimple.CounterState(),
                AttribNodeWithSelection.CounterState(),
                SCTFunctionNode.CounterState());

           

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
                functionNode.Serialize(writer);
            }
            writer.WriteEndElement();//FUNCTION NODES

            SerializeTargetNode(writer, targetNode);


            writer.WriteStartElement("CONNECTIONS");
       
            foreach (Connection con in connections)
            {
                SerializeConnection(writer, con);
            }
            writer.WriteEndElement();//CONNECTIONS

            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Flush();
            writer.Close();

        }

        ////////////////////////////// DESERIALIZATION ///////////////////////////////////////////

        public static bool DeserializeNodeInfo(XmlNode xmlNode, out NodeType nodeType, out string nodeID)
        {
            nodeType = NodeType.Function;
            nodeID = string.Empty;
            foreach (XmlAttribute attrib in xmlNode.Attributes)
            {
                if (attrib.Name == "TYPE") nodeType = (NodeType)Enum.Parse(typeof(NodeType), attrib.Value);
                else if (attrib.Name == "ID") nodeID = attrib.Value;
                else if (attrib.Name == "UNIQUE_ID") nodeID = attrib.Value;
            }
            if (nodeID == string.Empty) return false;
            return true;
        }

        public static bool DeserializeNodeInfo(XmlNode xmlNode, out NodeType nodeType, out string nodeID, out int selectedIndex)
        {
            nodeType = NodeType.Target;
            nodeID = string.Empty;
            selectedIndex = -1;
            foreach (XmlAttribute attrib in xmlNode.Attributes)
            {
                if (attrib.Name == "TYPE") nodeType = (NodeType)Enum.Parse(typeof(NodeType), attrib.Value);
                else if (attrib.Name == "ID") nodeID = attrib.Value;
                else if (attrib.Name == "SELECTED_INDEX") selectedIndex = int.Parse(attrib.Value);

            }
            if (nodeType == NodeType.Target) return false;
            if (nodeID == string.Empty) return false;
            if (selectedIndex == -1) return false;
            return true;
        }

        public static bool DeserializePosition(XmlNode node, out Point position)
        {
            try
            {
                int x = 0;
                int y = 0;
                foreach (XmlAttribute attrib in node.Attributes)
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

        public static bool DeserializeShadevVarDesc(XmlNode node, out ShaderVariableDescription svdesc)
        {
            svdesc = null;
            string varName = string.Empty;
            ShaderVariableType type =ShaderVariableType.Single;
            ConnectionDirection direction = ConnectionDirection.In;
            int counter = 0;
            foreach(XmlAttribute attrib in node.Attributes)
            {
                if(attrib.Name == "NAME")
                { varName = attrib.Value; counter++; }
                else if (attrib.Name == "TYPE")
                {
                    try { type = (ShaderVariableType)Enum.Parse(typeof(ShaderVariableType), attrib.Value); }
                    catch { return false; }
                    counter++;
                }
                else if (attrib.Name == "CONNECTION_DIRECTION")
                {
                    try { direction = (ConnectionDirection)Enum.Parse(typeof(ConnectionDirection), attrib.Value); }
                    catch { return false; }
                    counter++;
                }
            }
          
            if (counter < 3) return false;
            svdesc = new ShaderVariableDescription(varName, type, direction);
            foreach (XmlNode addInfoNode in node.ChildNodes)
            {
                if(addInfoNode.Name == "ADDITIONAL_INFO")
                svdesc.AdditionalInfo = addInfoNode.InnerText;
            }
            return true;
                
        }


        public static bool DeserializeFunctionDesc(XmlNode node, out FunctionNodeDescription desc)
        {
            desc = null;
            try
            {
                string name = node.Attributes[0].Value;
                desc = new FunctionNodeDescription(name);
            }
            catch
            { return false; }
            foreach(XmlNode child in node.ChildNodes)
            {
                if (child.Name == "CODE")
                {
                    desc.SetFucntionString(child.InnerText);
                }
                else if (child.Name == "INPUT_VARIABLES")
                {
                    foreach (XmlNode inVarNode in child.ChildNodes)
                    {
                        ShaderVariableDescription svd;
                        if (DeserializeShadevVarDesc(inVarNode, out svd))
                        {
                            if (svd.ConnectionDirection == ConnectionDirection.In)
                            { desc.AddInputVariable(svd); }
                            else return false;
                        }
                    }
                }

                else if (child.Name == "OUTPUT_VARIABLES")
                {
                    foreach (XmlNode outVarNode in child.ChildNodes)
                    {
                        ShaderVariableDescription svd;
                        if (DeserializeShadevVarDesc(outVarNode, out svd))
                        {
                            if (svd.ConnectionDirection == ConnectionDirection.Out)
                            { desc.AddOutputVariable(svd); }
                            else return false;
                        }
                    }
                }
            }


            return true;
        }

        public static bool DeserializeFrameBufferNode(XmlNode node, ref FrameBufferNode fbNode)
        {

            foreach (XmlNode nd in node.ChildNodes)
            {
                if (nd.Name == "POSITION")
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
            if (PlaceNodeCalback == null) return false;

            NodeType type = NodeType.Target;
            string id = string.Empty;
            DeserializeNodeInfo(xmlNode, out type, out id);

            /// Positon
            Point position = new Point(0, 0);
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
                else if (child.Name == "SHADER_COLOUR_VARIABLE")
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
                else if (child.Name == "SHADER_VECTOR_VARIABLE")
                {
                    foreach (XmlAttribute attrib in child.Attributes)
                    {
                        if (attrib.Name == "X") data[0] = attrib.Value;
                        else if (attrib.Name == "Y") data[1] = attrib.Value;
                        else if (attrib.Name == "Z") data[2] = attrib.Value;
                        else if (attrib.Name == "W") data[3] = attrib.Value;
                        else if (attrib.Name == "VARNAME") varName = attrib.Value;
                    }
                }
            }

            inNode = (IInputNode)PlaceNodeCalback(position, type);// new node created here
            inNode.ChangeUniqueID(id);
            inNode.ChangeVariableName(varName);
            if (type == NodeType.Input_Texture2D)
            {
                InputNodeTexture2D nd = (InputNodeTexture2D)inNode;
                nd.ChangeTexture(data[0]);
            }
            else if (type == NodeType.Input_Colour)
            {
                InputNodeColour nd = (InputNodeColour)inNode;
                nd.ChangeColour(float.Parse(data[0]), float.Parse(data[1]), float.Parse(data[2]), float.Parse(data[3]));
            }
            else
            {
                InputNodeVector nd = (InputNodeVector)inNode;
                List<float> fData = new List<float>();
                for (int i = 0; i < data.Length; ++i)
                {
                    if (data[i] == null) break;
                    fData.Add(float.Parse(data[i]));
                }
                nd.SetNewData(fData.ToArray());
            }

            return true;
        }

        public static bool DeserializeAttribNodeSimple(XmlNode xmlNode, out IAttribNode attribNode)
        {
            attribNode = null;
            if (xmlNode.Attributes.Count < 2) return false;
            if (PlaceNodeCalback == null) return false;
            NodeType type = NodeType.Target;
            string id = string.Empty;
            DeserializeNodeInfo(xmlNode, out type, out id);
            /// Positon
            Point position = new Point(0, 0);
            foreach (XmlNode child in xmlNode.ChildNodes)
            {
                if (child.Name == "POSITION") DeserializePosition(child, out position);
            }

            attribNode = (IAttribNode)PlaceNodeCalback(position, type);// new node created here
            attribNode.ChangeUniqueID(id);
            return true;

        }

        public static bool DeserializeAttribNodeWithSelection(XmlNode xmlNode, out IAttribNode attribNode)
        {
            attribNode = null;
            if (xmlNode.Attributes.Count < 2) return false;


            NodeType type = NodeType.Target;
            string id = string.Empty;
            int selectedIndex = 0;
            DeserializeNodeInfo(xmlNode, out type, out id, out selectedIndex);
            /// Positon
            Point position = new Point(0, 0);
            foreach (XmlNode child in xmlNode.ChildNodes)
            {
                if (child.Name == "POSITION") DeserializePosition(child, out position);

            }

            attribNode = (IAttribNode)PlaceNodeCalback(position, type);// new node created here
            attribNode.ChangeUniqueID(id);
            AttribNodeWithSelection selAttr = (AttribNodeWithSelection)attribNode;
            selAttr.ChangeSelectionIndex(selectedIndex);

            return true;
        }


        public static bool DeserializeFunctionNode(XmlNode xmlNode, out SCTFunctionNode functionNode)
        {
            functionNode = null;
            if (xmlNode.Attributes.Count < 1) return false;
            if (PlaceNodeCalback == null) return false;
            NodeType type;
            string id;
            DeserializeNodeInfo(xmlNode, out type, out id);

            /// Positon
            Point position = new Point(0, 0);
            foreach (XmlNode child in xmlNode.ChildNodes)
            {
                if (child.Name == "POSITION") DeserializePosition(child, out position);
                if (child.Name == "FUNCTION_NODE_DESC")
                {
                    FunctionNodeDescription desc;
                    if (DeserializeFunctionDesc(child, out desc))
                    {
                        NodeInstantiator.StartPlacingXML(desc);
                    }
                    else return false;
                    
                }
            }

            functionNode = (SCTFunctionNode)PlaceNodeCalback(position, type);// new node created here
            functionNode.ChangeUniqueID(id);
            return true;


        }

        static public bool DeserializeConnection(XmlNode connNode, List<ISCTNode> allNodes)
        {
            ISCTNode nodeSource = null;
            ISCTNode nodeDestination = null;
            string sourceLocalID = string.Empty;
            string destinationLocalID = string.Empty;
            int ax = 0; int ay = 0; int bx = 0; int by = 0;

            foreach (XmlAttribute attrib in connNode.Attributes)
            {
                if (attrib.Name == "SOURCE_NODE_ID")
                {
                    nodeSource = allNodes.Find(o => o.GetNodeID() == attrib.Value);
                    if (nodeSource == null) return false;
                }
                else if (attrib.Name == "DESTINATION_NODE_ID")
                {
                    nodeDestination = allNodes.Find(o => o.GetNodeID() == attrib.Value);
                    if (nodeDestination == null) return false;
                }
                else if (attrib.Name == "SOURCE_CON_LOCAL_ID")
                {
                    sourceLocalID = attrib.Value;
                }
                else if(attrib.Name == "DESTINATION_CON_LOCAL_ID")
                {
                    destinationLocalID = attrib.Value;
                }
                else if (attrib.Name == "AX")
                { if (!int.TryParse(attrib.Value, out ax)) return false;}
                else if (attrib.Name == "AY")
                { if (!int.TryParse(attrib.Value, out ay)) return false; }
                else if (attrib.Name == "BX")
                { if (!int.TryParse(attrib.Value, out bx)) return false; }
                else if (attrib.Name == "BY")
                { if (!int.TryParse(attrib.Value, out by)) return false; }


            }

            Connection con = new Connection(nodeSource.GetConnector(sourceLocalID),
                nodeDestination.GetConnector(destinationLocalID),
                EditAreaPanel);
            con.ApplyLineConfigAsync(new Point(ax, ay), new Point(bx, by),1000);
            ConnectionManager.AddConnecion(con);
            
            SCTConsole.Instance.PrintLine("\r\nCONNECTION");
            SCTConsole.Instance.PrintLine("Source node: " + nodeSource.GetNodeID() + ", DESTINATION NODE: " + nodeDestination.GetNodeID());
            SCTConsole.Instance.PrintLine("Source connector: " + sourceLocalID + ",  Destination connector: " + destinationLocalID);
            return true;
        }

        

        public static bool ReadNodes(string path, ref List<ISCTNode> allNodes, OnPlaceNodeCallback callback, Control editAreaPanel)
        {
            PlaceNodeCalback = callback;
            EditAreaPanel = editAreaPanel;
            XmlNodeList nodes;
            try
            {
                string xmlContent = File.ReadAllText(path);
                XmlDocument XdOC = new XmlDocument();
                XdOC.LoadXml(xmlContent);
                XmlElement document = XdOC.DocumentElement;
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
                    foreach (XmlNode inNode in node.ChildNodes)
                    {
                        if (inNode.Name != "UNIFORM_INPUT_NODE") continue;
                        IInputNode result;
                       if(!DeserializeInputNode(inNode, out result)) return false;
                    }
                }
                else if (node.Name == "SIMPLE_ATTRIB_NODES")
                {
                    // process simple attrib nodes
                    foreach (XmlNode inNode in node.ChildNodes)
                    {
                        if (inNode.Name != "SIMPLE_ATTRIB_NODE") continue;
                        IAttribNode result;
                        if (!DeserializeAttribNodeSimple(inNode, out result)) return false; 
                    }
                }
                else if (node.Name == "SELECTION_ATTRIB_NODES")
                {
                    // selection attrib nodes
                    foreach (XmlNode inNode in node.ChildNodes)
                    {
                        if (inNode.Name != "SELECTION_ATTRIB_NODE") continue;
                        IAttribNode result;
                        if(!DeserializeAttribNodeWithSelection(inNode, out result)) return false;
                    }
                }
                else if (node.Name == "FUNCTION_NODES")
                {
                    //proccess function nodes
                    foreach (XmlNode inNode in node.ChildNodes)
                    {
                        if (inNode.Name != "FUNCTION_NODE") continue;
                        SCTFunctionNode result;
                        if(!DeserializeFunctionNode(inNode, out result)) return false;
                    }
                }
                else if (node.Name == "TARGET_NODE")
                {
                    // process target node
                    FrameBufferNode fbn = (FrameBufferNode)allNodes.Find(o => o is FrameBufferNode);
                   if(!DeserializeFrameBufferNode(node, ref fbn)) return false;
                }
                else if (node.Name == "STATIC_COUNTERS")
                {
                    try
                    {
                        foreach (XmlAttribute attr in node.Attributes)
                        {
                            if (attr.Name == "IN_TEX") InputNodeTexture2D.SetCounter(int.Parse(attr.Value));
                            else if (attr.Name == "IN_COL") InputNodeColour.SetCounter(int.Parse(attr.Value));
                            else if (attr.Name == "IN_NUM") InputNodeVector.SetCounter(int.Parse(attr.Value));
                            else if (attr.Name == "ASIMPLE") AttribNodeSimple.SetCounter(int.Parse(attr.Value));
                            else if (attr.Name == "ASELECT") AttribNodeWithSelection.SetCounter(int.Parse(attr.Value));
                            else if (attr.Name == "FUNCTION") SCTFunctionNode.SetCounter(int.Parse(attr.Value));
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
           // EditAreaPanel.Invalidate();
            foreach (XmlNode node in nodes)
            {
                if (node.Name == "CONNECTIONS")
                {
                    // process input nodes
                    foreach (XmlNode connNode in node.ChildNodes)
                    {
                        if (connNode.Name != "CONNECTION") continue;
                        DeserializeConnection(connNode,allNodes);
                      
                    }
                    EditAreaPanel.Invalidate();
                    EditAreaPanel.Update();
                }
            }

                return true;
        }



    }
}
