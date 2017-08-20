using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShaderCreationTool
{
    enum ChangeNameStateMessage
    {
        ChangeOK,
        AddingNewName,
        ERROR_NameAlreadyExists
        
    }
    static class VaribaleNameGuard
    {
        static private List<string> s_Names = new List<string>();

        static public ChangeNameStateMessage ChangeName(string oldName, string newName)
        {
            ChangeNameStateMessage retState = ChangeNameStateMessage.ChangeOK;
            if (s_Names.Count == 0)
            {
                s_Names.Add(newName);
                return ChangeNameStateMessage.AddingNewName;
            }
            if (s_Names.Contains(newName)) return ChangeNameStateMessage.ERROR_NameAlreadyExists; 
            if (oldName == string.Empty)
            {
                s_Names.Add(newName);
                return ChangeNameStateMessage.AddingNewName;
            }
            if (s_Names.Contains(oldName))
            {
                s_Names.Remove(oldName);
                s_Names.Add(newName);
            }

            return retState;

        }
    }
}
