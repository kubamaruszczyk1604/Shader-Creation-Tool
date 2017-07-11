#pragma once
namespace KLM_FRAMEWORK
{
	enum class ShaderType
	{
		VERTEX = 0,
	    FRAGMENT = 1
	};

	enum class ShaderAPI
	{
		D3D11 = 0,
		GL = 1
	};

	class Shader
	{
	private:
		ShaderType m_ShaderType;
		ShaderAPI m_API;

	protected:
		Shader(ShaderAPI api, ShaderType type);

	public:
		

		Shader(const Shader&) = delete;
		Shader& operator=(const Shader&) = delete;

		ShaderType GetType() { return m_ShaderType; }
		ShaderAPI GetAPI() { return m_API; }

		virtual ~Shader();
	};

}