#include "D3D11Shader.h"

namespace KLM_FRAMEWORK
{


	D3D11Shader::D3D11Shader(const std::string& filePath, const std::string& functionName, ShaderType type) :
		Shader(ShaderAPI::D3D11, type),
		m_Path{ filePath },
		m_FunctionName{ functionName }
	{

	}


	D3D11Shader::~D3D11Shader()
	{
	}


}