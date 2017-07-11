#pragma once
#include "StdIncludes.h"
#include "D3D11Shader.h"

namespace KLM_FRAMEWORK
{
	class D3D11VertexShader : public D3D11Shader
	{
	private:

		ID3D11InputLayout*      m_InputLayout;
		ID3D11VertexShader*		m_VertexShader;
		ID3D10Blob*             m_VS_Blob;
		int                     m_AttribCount;
		bool m_Compiled;

		void CreateInputLayout();
	public:
		D3D11VertexShader(const std::string& filePath, const std::string& functionName);
		~D3D11VertexShader();

		D3D11VertexShader(const D3D11VertexShader&) = delete;
		D3D11VertexShader& operator=(const D3D11VertexShader&) = delete;

		bool Compile() override;
		void SetAsCurrent() override;
		

	};

}