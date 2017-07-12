#include "GLShaderProgram.h"


namespace KLM_FRAMEWORK
{
	GLShaderProgram::GLShaderProgram(GLShader * vertexShader, GLShader * fragmentShader):
		p_VertexShader{vertexShader},
		p_FragmentShader{fragmentShader}
	{
	}

	GLShaderProgram::~GLShaderProgram()
	{
		if (p_VertexShader) glDetachShader(m_ProgId, p_VertexShader->GetID());
		if (p_FragmentShader) glDetachShader(m_ProgId, p_FragmentShader->GetID());
		glDeleteProgram(m_ProgId);
	}

	bool GLShaderProgram::Create()
	{
		if (m_Created) return true;
		m_ProgId = glCreateProgram();
		glAttachShader(m_ProgId, p_VertexShader->GetID());
		glAttachShader(m_ProgId, p_FragmentShader->GetID());
		glLinkProgram(m_ProgId);
		m_Created = true;
		return true;
	}

	bool GLShaderProgram::SetAsCurrent()
	{
		if (p_VertexShader&&p_FragmentShader)
		{
			glUseProgram(m_ProgId);
			return true;
		}
		return false;
	}
}