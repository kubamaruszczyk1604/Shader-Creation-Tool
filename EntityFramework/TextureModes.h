#pragma once


namespace KLM_FRAMEWORK
{

	enum class TextureType
	{
		D3D11TEXTURE = 0,
		GLTEXTURE = 1
	};


	enum class TextureAddressMode
	{
		WRAP = 0,
		MIRROR = 1,
		CLAMP = 2,
		BORDER = 3,
		MIRROR_ONCE = 4
	};




}