#pragma once
#include "Shader.h"
#include "StdIncludes.h"

namespace KLM_FRAMEWORK
{

	class GLShader: public Shader
	{
	public:
		GLShader(ShaderType type);
		virtual ~GLShader();

		GLShader(const GLShader&) = delete;
		GLShader& operator=(const GLShader&) = delete;


		bool LoadFromFile(const std::string& path);
		bool Compile(std::string& log);
		GLuint GetID();

	private:
		GLuint m_Id;

	};


}

