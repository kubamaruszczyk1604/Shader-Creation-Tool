using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShaderCreationTool
{
    delegate void NodeCloseButtonCallback(ISCTNode sender);
    delegate void NodeInputError(string errorDescription, ISCTNode sender);

    interface ISCTNode
    {
        void AddOnMovedCallback(ObjectMovedCallback onMovedCallback);
        void AddOnBeginConnectionCallback(BeginConnectionCallback onBeginConnection);
        void AddOnBreakConnectionCallback(BreakConnectionCallback onBreakConnection);
        Connector GetConnector(ConnectionDirection type, int index);
        List<Connector> GetAllConnectors(ConnectionDirection type);
        List<Connector> GetAllConnectors();

    }

    class LockableNodes
    {
       static public void LockButtons()
        {
            SCTFunctionNode.LockButtons();
        }

        static public void UnlockButtons()
        {
            SCTFunctionNode.UnlockButtons();
        }
    }
}
