#pragma once
#include "TextureModes.h"
#include "StdIncludes.h"

namespace KLM_FRAMEWORK
{
	



	class Texture
	{

	private:
		void* m_pTexture{ nullptr };
		bool m_Created{ false };
		GfxAPI m_API;

	public:
		Texture(const std::string& fileName, TextureAddressMode mode = TextureAddressMode::WRAP);
		virtual ~Texture();
		const void* GetApiSpecificTexture() const { return m_pTexture; }
		bool IsCreated() { return m_Created; }
		GfxAPI GetAPI() { return m_API; }
	};

}