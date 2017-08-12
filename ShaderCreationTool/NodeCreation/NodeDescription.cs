using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShaderCreationTool
{
   
    public class ShaderVariableDescription
    {
        private string m_Name;
        private ShaderVariableType m_Type;
        private ConnectionDirection m_ConnectionDirection;

        public ShaderVariableDescription(string name, ShaderVariableType type, ConnectionDirection direction)
        {
            m_Name = name;
            m_Type = type;
            m_ConnectionDirection = direction;
        }

        public string Name { get { return m_Name; } }
        public ShaderVariableType Type { get { return m_Type; } }
        public ConnectionDirection ConnectionDirection { get { return m_ConnectionDirection; } }


    }

    public class FunctionNodeDescription
    {
        private List<ShaderVariableDescription> m_InputVariables;
        private List<ShaderVariableDescription> m_OutputVariables;
        private string m_FunctionCode;
        private string m_Name;

        public FunctionNodeDescription(string name)
        {
            m_InputVariables = new List<ShaderVariableDescription>();
            m_OutputVariables = new List<ShaderVariableDescription>();
            m_FunctionCode = string.Empty;
            m_Name = name;
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

        public int InputCount { get { return m_InputVariables.Count; } }
        public int OutputCount { get { return m_OutputVariables.Count; } }
        public string Name { get { return m_Name; } }

    }
}
