#pragma once
#include "StdIncludes.h"
#include "DataWrappers.h"
#include "ResourceManager.h"

namespace KLM_FRAMEWORK
{
	using namespace System;
	using namespace System::Collections::Generic;

	public ref class ShaderVariableContainer
	{
	private:
		static List<ShaderVectorVariable^> s_ShaderVariablesList;
		static List<ShaderTextureVariable^> s_TextureVariablesList;
	
	public:
		static int GetSize_VectorVariables() { return s_ShaderVariablesList.Count; }
		static void AddVectorVariable(ShaderVectorVariable^ variable);
		static ShaderVectorVariable^ GetShaderVectorVariable(int index);

		static int GetSize_TextureVariables() { return s_TextureVariablesList.Count; }
		static void AddTextureVariable(ShaderTextureVariable^ variable);
		static ShaderTextureVariable^ GetShaderTextureVariable(int index);

	};
}
