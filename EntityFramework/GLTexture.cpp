#include "GLTexture.h"
#include "GLTextureLoader.h"

namespace KLM_FRAMEWORK
{
	const GLuint GLTexture::s_TextureAddresModeLookupTable[5] =
	{   GL_REPEAT,
		GL_MIRRORED_REPEAT,
		GL_CLAMP,
		GL_CLAMP_TO_BORDER,
		GL_MIRROR_CLAMP_TO_EDGE
	};


	GLuint GLTexture::GetGLAddressMode(TextureAddressMode addressMode)
	{
		return s_TextureAddresModeLookupTable[static_cast<unsigned>(addressMode)];
	}

	bool GLTexture::CreateGLTexture(const std::string & str, TextureAddressMode wrapMode, GLTexture*& output)
	{
		std::ifstream ifs(str);
		if (!ifs.is_open()) return false;
		ifs.close();

		GLuint textureID = GLTextureLoader::LoadTexture(str);
		if (textureID == -1) return false;
		
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GetGLAddressMode(wrapMode));
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GetGLAddressMode(wrapMode));

		output = new GLTexture(textureID);

		return true;
	}

	GLTexture::GLTexture(GLuint id):m_TextureID{id}
	{
		
	}

	GLTexture::~GLTexture()
	{
		glDeleteTextures(1,&m_TextureID);
	}
}