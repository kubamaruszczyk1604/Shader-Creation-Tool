using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShaderCreationTool
{
    interface ICodeParser
    {
        bool TranslateNetworkFragment(List<ISCTNode> nodes, List<Connection> connections, out string fragmentShaderCode, out string status);
        bool TranslateNetworkVertex(List<ISCTNode> nodes, List<Connection> connections, out string vertexShaderCode, out string status);
    }
}
