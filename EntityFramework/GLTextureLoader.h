#pragma once
#include "StdIncludes.h"


namespace KLM_FRAMEWORK
{
	class GLTextureLoader
	{
	public:
		GLTextureLoader() = default;
		~GLTextureLoader();

		GLTextureLoader& operator=(const GLTextureLoader&) = delete;
		GLTextureLoader(const GLTextureLoader&) = delete;

		static GLuint LoadTexture(const std::string& path);
	};

}