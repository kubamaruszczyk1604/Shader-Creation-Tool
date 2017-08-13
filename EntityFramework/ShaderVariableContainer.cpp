#include "ShaderVariableContainer.h"



namespace KLM_FRAMEWORK
{
	ShaderVectorVariable^ ShaderVariableContainer::GetShaderVectorVariable(int index)
	{
		if (s_ShaderVariablesList.Count == 0) return nullptr;
		//if (index >= s_ShaderVaraiablesList.Count - 1) return nullptr;
		return s_ShaderVariablesList[index];
	}

	void ShaderVariableContainer::AddTextureVariable(ShaderTextureVariable ^ variable)
	{
		s_TextureVariablesList.Add(variable);
	}

	ShaderTextureVariable ^ ShaderVariableContainer::GetShaderTextureVariable(int index)
	{
		if (s_TextureVariablesList.Count == 0) return nullptr;
		//if (index >= s_ShaderVaraiablesList.Count - 1) return nullptr;
		return s_TextureVariablesList[index];
	}

	void ShaderVariableContainer::AddVectorVariable(ShaderVectorVariable ^ variable)
	{
		s_ShaderVariablesList.Add(variable);
	}
}