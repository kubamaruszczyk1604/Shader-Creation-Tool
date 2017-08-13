#include "ShaderVariableContainer.h"



namespace KLM_FRAMEWORK
{
	ShaderVectorVariable^ ShaderVariableContainer::GetShaderVectorVariable(int index)
	{
		if (s_ShaderVaraiablesList.Count == 0) return nullptr;
		//if (index >= s_ShaderVaraiablesList.Count - 1) return nullptr;
		return s_ShaderVaraiablesList[index];
	}

	void ShaderVariableContainer::AddVectorVariable(ShaderVectorVariable ^ variable)
	{
		s_ShaderVaraiablesList.Add(variable);
	}
}