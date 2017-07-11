#include "D3DTexture.h"
#include "D3DRenderer.h"
namespace KLM_FRAMEWORK
{

	const D3D11_TEXTURE_ADDRESS_MODE D3DTexture::s_TextureAddresModeLookupTable[5] =
	{ D3D11_TEXTURE_ADDRESS_WRAP, 
		D3D11_TEXTURE_ADDRESS_MIRROR,
		D3D11_TEXTURE_ADDRESS_CLAMP, 
		D3D11_TEXTURE_ADDRESS_BORDER, 
		D3D11_TEXTURE_ADDRESS_MIRROR_ONCE 
	};

	D3D11_TEXTURE_ADDRESS_MODE D3DTexture::GetD3D11AdsressMode(TextureAddressMode addressMode)
	{
		return s_TextureAddresModeLookupTable[static_cast<unsigned>(addressMode)];
	}

	bool D3DTexture::CreateDXTexture(const std::string & str, TextureAddressMode wrapMode, D3DTexture*& output)
	{
		ID3D11ShaderResourceView* tempTexturePtr = 0;
		HRESULT hr = D3DX11CreateShaderResourceViewFromFile(DXRenderer::GetDevice(),
			const_cast<char*>(str.c_str()),
			NULL, NULL, &tempTexturePtr, NULL);

		if (FAILED(hr)) return false;

		output = new D3DTexture(tempTexturePtr, wrapMode);

		return true;
	}

	


	D3DTexture::D3DTexture(ID3D11ShaderResourceView * texture, TextureAddressMode addressMode):
		m_pTexture(texture),
		m_AddressMode(addressMode)
	{
		ZeroMemory(&sampDesc, sizeof(sampDesc));
		sampDesc.Filter = D3D11_FILTER_MIN_MAG_MIP_LINEAR;//D3D11_FILTER_MIN_MAG_MIP_LINEAR;
		sampDesc.AddressU = s_TextureAddresModeLookupTable[static_cast<int>(addressMode)];
		sampDesc.AddressV = s_TextureAddresModeLookupTable[static_cast<int>(addressMode)];
		sampDesc.AddressW = s_TextureAddresModeLookupTable[static_cast<int>(addressMode)];
		sampDesc.ComparisonFunc = D3D11_COMPARISON_NEVER;
		sampDesc.MinLOD = 0;
		sampDesc.MaxLOD = D3D11_FLOAT32_MAX;

		DXRenderer::GetDevice()->CreateSamplerState(&sampDesc, &m_SamplerState);
	}

	D3DTexture::~D3DTexture()
	{
		try 
		{
			if(m_pTexture != nullptr) m_pTexture->Release();
			m_pTexture = nullptr;
		}
		catch (std::exception& e) {}
	}
}