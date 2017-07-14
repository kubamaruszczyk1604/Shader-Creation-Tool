#pragma once
#include "StdIncludes.h"
#include "TextureModes.h"

namespace KLM_FRAMEWORK
{
	class GLTexture
	{
	private:
		static const GLuint s_TextureAddresModeLookupTable[5];

		GLuint m_TextureID{ 0 };
	public:
		static GLuint GetGLAddressMode(TextureAddressMode addressMode);

		static bool CreateGLTexture(const std::string& str, TextureAddressMode wrapMode, GLTexture*& output);

		GLuint GetID() { return m_TextureID; }
		GLTexture(GLuint id);
		~GLTexture();
	};
}
