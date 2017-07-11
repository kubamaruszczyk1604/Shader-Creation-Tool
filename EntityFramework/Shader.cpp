#include "Shader.h"

namespace KLM_FRAMEWORK
{

	

	Shader::Shader(ShaderAPI api, ShaderType type):
		m_API{api},m_ShaderType{type}
	{
	}

	Shader::~Shader()
	{
	}
}