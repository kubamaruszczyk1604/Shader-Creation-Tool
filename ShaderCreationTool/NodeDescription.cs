using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShaderCreationTool
{
    class ShaderVariableDescription
    {
        public string PName;
        public ShaderVariableType PType;
    }

    class NodeDescription
    {
        private List<ShaderVariableDescription> m_InputVariables;
        private List<ShaderVariableDescription> m_OutputVariables;
        private string m_FunctionCode;

        public NodeDescription()
        {
            m_InputVariables = new List<ShaderVariableDescription>();
            m_OutputVariables = new List<ShaderVariableDescription>();
            m_FunctionCode = string.Empty;
        }

        public void AddInputVariable(ShaderVariableDescription desc)
        {
            m_InputVariables.Add(desc);
        }

        public void AddOutputVariable(ShaderVariableDescription desc)
        {
            m_OutputVariables.Add(desc);
        }

        public void SetFucntionString(string str)
        {
            m_FunctionCode = str;
        }


        public ShaderVariableDescription GetInVariableDescription(int index)
        {
            return m_InputVariables[index];
        }

        public ShaderVariableDescription GetOutVariableDescription(int index)
        {
            return m_OutputVariables[index];
        }

        public string GetFunctionString()
        {
            return m_FunctionCode;
        }

    }
}
