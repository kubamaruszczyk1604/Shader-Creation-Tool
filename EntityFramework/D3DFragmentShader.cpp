#include "D3DFragmentShader.h"
#include "D3DRenderer.h"
namespace KLM_FRAMEWORK
{
	D3D11FragmentShader::D3D11FragmentShader(const std::string& filePath, const std::string& functionName) :
		D3D11Shader(filePath, functionName,ShaderType::FRAGMENT),
		m_PixelShader{ nullptr },
		m_PS_Blob{ nullptr },
		m_Compiled{ false }
	{
	}


	D3D11FragmentShader::~D3D11FragmentShader()
	{
	}

	bool D3D11FragmentShader::Compile()
	{
		if (m_Compiled) return true;
		HRESULT hr = D3DX11CompileFromFile(
			GetFilePath().c_str(),
			nullptr, nullptr,
			GetFunctionName().c_str(),
			"ps_5_0",
			0,
			0,
			nullptr,
			&m_PS_Blob,
			nullptr,
			nullptr);


		if (FAILED(hr))
		{
			return false;
			std::cout << "Fragment Shader Failed to Compile" << std::endl;
		}
		else
		{
			std::cout << "Fragment Shader Compiled Succesfully" << std::endl;
		}

		DXRenderer::GetDevice()->CreatePixelShader(m_PS_Blob->GetBufferPointer(), m_PS_Blob->GetBufferSize(), nullptr, &m_PixelShader);
		if (FAILED(hr)) return false;

		m_Compiled = true;
		return true;

	}

	void D3D11FragmentShader::SetAsCurrent()
	{
		DXRenderer::GetDeviceContext()->PSSetShader(m_PixelShader, nullptr, 0);
	}

}

