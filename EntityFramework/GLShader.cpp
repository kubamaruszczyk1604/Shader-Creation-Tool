#include "GLShader.h"



namespace KLM_FRAMEWORK
{
	//#define PRINT_SHADERS

	GLShader::GLShader(ShaderType type) :
		Shader(ShaderAPI::GL,type),
		m_Id(0)
	{
		if (type == ShaderType::VERTEX)
		{
			m_Id = glCreateShader(GL_VERTEX_SHADER);
		}
		else if (type == ShaderType::FRAGMENT)
		{
			m_Id = glCreateShader(GL_FRAGMENT_SHADER);
		}


	}


	GLShader::~GLShader()
	{

		glDeleteShader(m_Id);
	}

	bool GLShader::LoadFromFile(const std::string & path)
	{

		std::ifstream ifs(path);

		if (!ifs.is_open()) return false;
		std::string result;
		char buffer;
		while (ifs.get(buffer))
		{
			result += buffer;
		}

#ifdef PRINT_SHADERS
		std::cout << result << std::endl;
#endif
		const char * c = result.c_str();
		glShaderSource(m_Id, 1, &c, NULL);
		return true;
	}

	bool GLShader::Compile(std::string& log)
	{
		glCompileShader(m_Id);
		GLint isCompiled = 0;
		glGetShaderiv(m_Id, GL_COMPILE_STATUS, &isCompiled);
		if (isCompiled == GL_FALSE)
		{
			GLint maxLength = 0;
			glGetShaderiv(m_Id, GL_INFO_LOG_LENGTH, &maxLength);

			// The maxLength includes the NULL character
			std::vector<char> errorLog(maxLength);
			glGetShaderInfoLog(m_Id, maxLength, &maxLength, &errorLog[0]);
			log = std::string(errorLog.begin(), errorLog.end());
			// Provide the infolog in whatever manor you deem best.
			// Exit with failure.
			glDeleteShader(m_Id); // Don't leak the shader.
			return false;
		}
		else
		{
			log = "NO ERRORS";
			return true;
		}
	}

	GLuint GLShader::GetID()
	{
		return m_Id;
	}



}
