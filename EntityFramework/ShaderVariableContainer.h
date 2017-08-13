#pragma once
#include "StdIncludes.h"
#include "DataWrappers.h"

namespace KLM_FRAMEWORK
{
	using namespace System;
	using namespace System::Collections::Generic;

	public ref class ShaderVariableContainer
	{
	private:
		static List<ShaderVectorVariable^> s_ShaderVaraiablesList;

	public:
		static int GetSize() { return s_ShaderVaraiablesList.Count; }
		static void AddVariable(ShaderVectorVariable^ variable);
		static ShaderVectorVariable^ GetShaderVectorVariable(int index);

	};
}
