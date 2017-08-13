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
		static List<ShaderVectorVariable^> s_ShaderVaraiablesList;
	
	public:
		static int GetSize_VectorVariables() { return s_ShaderVaraiablesList.Count; }
		static void AddVectorVariable(ShaderVectorVariable^ variable);
		static ShaderVectorVariable^ GetShaderVectorVariable(int index);

	};
}
