#pragma once
#include "StdIncludes.h"
#include "D3D11Shader.h"

namespace KLM_FRAMEWORK
{

	class D3D11FragmentShader : public D3D11Shader
	{
	private:
		ID3D11PixelShader*		m_PixelShader;
		ID3D10Blob*             m_PS_Blob;
		bool m_Compiled;

	public:
		D3D11FragmentShader(const D3D11FragmentShader&) = delete;
		D3D11FragmentShader& operator=(const D3D11FragmentShader&) = delete;

		D3D11FragmentShader(const std::string& filePath, const std::string& functionName);
		~D3D11FragmentShader();

		bool Compile() override;
		void SetAsCurrent() override;
	};


}

