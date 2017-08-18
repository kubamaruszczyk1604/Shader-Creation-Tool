using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShaderCreationTool
{
    class CodeParserGLSL
    {

        bool TranslateInputVariables(List<IInputNode> inputNodes, out string declarationsCode, out string status)
        {
            declarationsCode = "";
            status = "";
            return false;
        }
        bool TranslateNodeIntoFunction(SCTFunctionNode node, out string functionCode, out string status)
        {

            functionCode = "";
            status = "";
            return false;
        }
        bool TranslateNetwork(List<ISCTNode> nodes, List<Connection> connections, out string code, out string status)
        {
            code = "";
            status = "";
            return false;
        }
    }
}
