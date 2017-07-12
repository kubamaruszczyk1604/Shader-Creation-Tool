#pragma once
#include "GLShader.h"
namespace KLM_FRAMEWORK
{
	class GLShaderProgram
	{

	private:
		GLuint m_ProgId;
		GLShader* p_VertexShader;
		GLShader* p_FragmentShader;
		bool m_Created{ false };


	public:
		GLShaderProgram(GLShader* vertexShader, GLShader* fragmentShader);
		virtual ~GLShaderProgram();

		GLShaderProgram& operator=(GLShaderProgram&) = delete;
		GLShaderProgram(const GLShaderProgram&) = delete;

		bool Create();
		bool SetAsCurrent();
		GLuint GetID() { return m_ProgId; }
	};

}