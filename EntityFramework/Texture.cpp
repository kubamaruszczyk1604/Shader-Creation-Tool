#include "Texture.h"
#include "D3DTexture.h"
#include "GLTexture.h"
#include "Renderer.h"

namespace KLM_FRAMEWORK
{
	Texture::Texture(const std::string& fileName, TextureAddressMode mode)
	{
		m_API = Renderer::GetAPI();
		
		if (m_API== GfxAPI::D3D11)
		{
			
			D3DTexture* texture{ nullptr };
		    m_Created = D3DTexture::CreateDXTexture(fileName, mode, texture);
			if (m_Created)
			{
				m_pTexture = texture;
			}
		}

		else if (m_API == GfxAPI::GL)
		{
			GLTexture* texture{ nullptr };
			m_Created = GLTexture::CreateGLTexture(fileName, mode, texture);
			if (m_Created)
			{
				m_pTexture = texture;
			}
		}

	}


	Texture::~Texture()
	{
		if (m_API == GfxAPI::D3D11)
		{
			D3DTexture* texture = static_cast<D3DTexture*>(m_pTexture);
			delete texture;
			texture = nullptr;
		}
		else if (m_API == GfxAPI::GL)
		{
			GLTexture* texture = static_cast<GLTexture*>(m_pTexture);
			delete texture;
			texture = nullptr;
		}
		m_pTexture= nullptr;
	}
}