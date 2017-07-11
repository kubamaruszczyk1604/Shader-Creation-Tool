#pragma once
#include "StdIncludes.h"
#include "Shader.h"


namespace KLM_FRAMEWORK
{

	class D3D11Shader: public Shader
	{
	private:
		std::string m_Path;
		std::string m_FunctionName;
		
	protected:
		D3D11Shader(const std::string& filePath, const std::string& functionName, ShaderType type);

	public:
	
		virtual ~D3D11Shader();

		virtual bool Compile() = 0;
		virtual void SetAsCurrent() = 0;

		const std::string const& GetFilePath() const { return m_Path; }
		const std::string const& GetFunctionName() const { return m_FunctionName; }
	};

}
