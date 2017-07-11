#pragma once
#include "StdIncludes.h"
#include "TextureModes.h"

namespace KLM_FRAMEWORK
{
	class D3DTexture
	{
	private:
		static const D3D11_TEXTURE_ADDRESS_MODE s_TextureAddresModeLookupTable[5];

		ID3D11ShaderResourceView* m_pTexture{ nullptr };
		D3D11_SAMPLER_DESC sampDesc;
		ID3D11SamplerState* m_SamplerState{ nullptr };
		TextureAddressMode m_AddressMode{ TextureAddressMode::WRAP };

	public:
		static D3D11_TEXTURE_ADDRESS_MODE GetD3D11AdsressMode(TextureAddressMode addressMode);
		static bool CreateDXTexture(const std::string& str, TextureAddressMode wrapMode, D3DTexture*& output);

		D3DTexture(const D3DTexture&) = delete;
		D3DTexture &operator=(const D3DTexture&) = delete;
		D3DTexture(ID3D11ShaderResourceView* texture, TextureAddressMode wrapMode);
		virtual ~D3DTexture();


		const ID3D11ShaderResourceView*  GetData()const { return m_pTexture; }
		const D3D11_SAMPLER_DESC* GetDescription()const { return &sampDesc; }
		ID3D11SamplerState* GetSamplerState()const { return m_SamplerState; }
	};
}
