#include "D3D11VertexShader.h"
#include "D3DRenderer.h"


namespace KLM_FRAMEWORK
{
	bool D3D11VertexShader::Compile()
	{
		if (m_Compiled) return true;
		HRESULT hr = D3DX11CompileFromFile(
			GetFilePath().c_str(),
			nullptr, nullptr, GetFunctionName().c_str(),
			"vs_5_0",
			0,
			0,
			nullptr,
			&m_VS_Blob,
			nullptr,
			nullptr);

		if (FAILED(hr))
		{

			std::cout << "Could not compile Vertex Shader: error= " << hr << std::endl;
			return false;
		}
		else
		{
			std::cout << "Vertex Shader compiled successfully! " << std::endl;
			hr = DXRenderer::GetDevice()->CreateVertexShader(m_VS_Blob->GetBufferPointer(), m_VS_Blob->GetBufferSize(), nullptr, &m_VertexShader);
			if (FAILED(hr)) 
			{
				PRINTL("Renderer failed to create Vertex Shader!");
				return false;
			}
			CreateInputLayout();
			
			m_Compiled = true;
			return true;
		}
	}

	void D3D11VertexShader::SetAsCurrent()
	{
		DXRenderer::GetDeviceContext()->VSSetShader(m_VertexShader, nullptr, 0);
	}

	void D3D11VertexShader::CreateInputLayout()
	{
		//	const D3D11_INPUT_ELEMENT_DESC const descArr[] =  

		// Build the input descriptor
		std::vector<D3D11_INPUT_ELEMENT_DESC> descArr;
		descArr.push_back({ "POSITION", 0, DXGI_FORMAT_R32G32B32_FLOAT, 0, 0, D3D11_INPUT_PER_VERTEX_DATA, 0 });
		descArr.push_back({ "NORMAL", 0, DXGI_FORMAT_R32G32B32_FLOAT, 0, 12, D3D11_INPUT_PER_VERTEX_DATA, 0 });
		descArr.push_back({ "TANGENT", 0, DXGI_FORMAT_R32G32B32_FLOAT, 0, 24, D3D11_INPUT_PER_VERTEX_DATA, 0 });
		descArr.push_back({ "TEXCOORD", 0, DXGI_FORMAT_R32G32_FLOAT, 0, 36, D3D11_INPUT_PER_VERTEX_DATA, 0 });

		m_AttribCount = 4;

		DXRenderer::GetDevice()->CreateInputLayout(descArr.data(),
			m_AttribCount,
			m_VS_Blob->GetBufferPointer(),
			m_VS_Blob->GetBufferSize(),
			&m_InputLayout);

		DXRenderer::GetDeviceContext()->IASetInputLayout(m_InputLayout);
	}

	D3D11VertexShader::D3D11VertexShader(const std::string& filePath, const std::string& functionName) :
		D3D11Shader(filePath, functionName,ShaderType::VERTEX),
		m_InputLayout(nullptr), m_VertexShader(nullptr), m_VS_Blob(nullptr), m_AttribCount(0), m_Compiled(false)
	{

	}


	D3D11VertexShader::~D3D11VertexShader()
	{
	}

}