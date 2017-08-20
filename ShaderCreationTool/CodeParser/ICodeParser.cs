using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShaderCreationTool
{
    interface ICodeParser
    {
        bool TranslateInputVariables(List<IInputNode> inputNodes, out string declarationsCode, out string status);
        bool TranslateNodeIntoFunction(SCTFunctionNode node, out string functionCode, out string status);
        bool TranslateNodeListIntoFunctions(List<SCTFunctionNode> nodes, out string functionCode, out string status);
        bool TranslateNetwork(List<ISCTNode> nodes, List<Connection> connections, out string code, out string status);
        string ConstructFunctionCall(SCTFunctionNode node);
    }
}
