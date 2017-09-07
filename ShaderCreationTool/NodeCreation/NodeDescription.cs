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

        public string AdditionalInfo { get; set; }


    }
    public class SubFuntionDescription
    {
        private List<ShaderVariableDescription> m_InputVariables;
        private ShaderVariableType m_ReturnType;
        private string m_FunctionCode;
        private string m_Name;

        public SubFuntionDescription(string name, ShaderVariableType returnType)
        {
            m_InputVariables = new List<ShaderVariableDescription>();
            m_FunctionCode = string.Empty;
            m_Name = name;
            m_ReturnType = returnType;
        }

        public string GetFunctionString()
        {
            return m_FunctionCode;
        }
        public void SetFucntionString(string str)
        {
            m_FunctionCode = str;
        }
        public void AddInputVariable(ShaderVariableDescription desc)
        {
            m_InputVariables.Add(desc);
        }
        public ShaderVariableDescription GetInVariableDescription(int index)
        {
            return m_InputVariables[index];
        }

        public ShaderVariableType GetReturnType()
        {
           return m_ReturnType;
        }

        public int InputCount { get { return m_InputVariables.Count; } }
        public string Name { get { return m_Name; } }
    }


     public class FunctionNodeDescription
    {
        private List<ShaderVariableDescription> m_InputVariables;
        private List<ShaderVariableDescription> m_OutputVariables;
        private List<SubFuntionDescription> m_FunctionDescriptions;
        private string m_FunctionCode;
        private string m_Name;

        public FunctionNodeDescription(string name)
        {
            m_InputVariables = new List<ShaderVariableDescription>();
            m_OutputVariables = new List<ShaderVariableDescription>();
            m_FunctionDescriptions = new List<SubFuntionDescription>();
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

        public void AddUtilFunctDescription(SubFuntionDescription desc)
        {
            m_FunctionDescriptions.Add(desc);
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

        public SubFuntionDescription GetSubFunctDescription(int index)
        {
            return m_FunctionDescriptions[index];
        }

        public string GetFunctionString()
        {
            return m_FunctionCode;
        }

        public int InputCount { get { return m_InputVariables.Count; } }
        public int OutputCount { get { return m_OutputVariables.Count; } }
        public int SubFunctCount { get { return m_FunctionDescriptions.Count; } }
        public string Name { get { return m_Name; } }

    }
}
