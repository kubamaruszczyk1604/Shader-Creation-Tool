#include "D3DVBO.h"



namespace KLM_FRAMEWORK
{
	//const D3D_PRIMITIVE_TOPOLOGY const VBO::LookupTableTopologyType[] = 
	const std::array<D3D_PRIMITIVE_TOPOLOGY, 5> const D3D11VBO::LookupTableTopologyType = 
	{
		D3D_PRIMITIVE_TOPOLOGY::D3D11_PRIMITIVE_TOPOLOGY_POINTLIST,
		D3D_PRIMITIVE_TOPOLOGY::D3D11_PRIMITIVE_TOPOLOGY_LINELIST,
		D3D_PRIMITIVE_TOPOLOGY::D3D11_PRIMITIVE_TOPOLOGY_LINESTRIP,
		D3D_PRIMITIVE_TOPOLOGY::D3D11_PRIMITIVE_TOPOLOGY_TRIANGLELIST,
		D3D_PRIMITIVE_TOPOLOGY::D3D11_PRIMITIVE_TOPOLOGY_TRIANGLESTRIP
	};

	D3D11VBO::D3D11VBO() :
		VBO(),
		m_pVBO{ nullptr },
		m_pIndexBO{ nullptr },
		m_VertexCount{ 0 },
		m_IndexCount{ 0 }
	{
	}

	D3D11VBO::~D3D11VBO()
	{

	}

	bool D3D11VBO::Create(Vertex * const vertices, int const numVertices)
	{
		if (numVertices<1) return false;
		m_VertexCount = numVertices;

		D3D11_BUFFER_DESC bd;
		ZeroMemory(&bd, sizeof(bd));

		bd.Usage = D3D11_USAGE_DYNAMIC;
		bd.ByteWidth = sizeof(Vertex) * numVertices;
		bd.BindFlags = D3D11_BIND_VERTEX_BUFFER;
		bd.CPUAccessFlags = D3D11_CPU_ACCESS_WRITE;

		DXRenderer::GetDevice()->CreateBuffer(&bd, nullptr, &m_pVBO);

		D3D11_MAPPED_SUBRESOURCE ms;
		DXRenderer::GetDeviceContext()->Map(m_pVBO, 0, D3D11_MAP_WRITE_DISCARD, 0, &ms);
		memcpy(ms.pData, vertices, sizeof(Vertex) * numVertices);
		DXRenderer::GetDeviceContext()->Unmap(m_pVBO, 0);

		return true;
	}

	void D3D11VBO::Draw(PrimitiveType const primitiveType) const
	{
		const UINT stride = sizeof(Vertex);
		const UINT offset = 0;
		DXRenderer::GetDeviceContext()->IASetVertexBuffers(0, 1, &m_pVBO, &stride, &offset);
		DXRenderer::GetDeviceContext()->IASetIndexBuffer(m_pIndexBO, DXGI_FORMAT_R32_UINT, 0);
		//DXRenderer::GetDeviceContext()->IASetPrimitiveTopology(LookupTableTopologyType[static_cast<int>(primitiveType)]);
		DXRenderer::GetDeviceContext()->IASetPrimitiveTopology(D3D_PRIMITIVE_TOPOLOGY::D3D11_PRIMITIVE_TOPOLOGY_TRIANGLELIST);
		
		if (!m_pIndexBO)DXRenderer::GetDeviceContext()->Draw(m_VertexCount, 0);
		else DXRenderer::GetDeviceContext()->DrawIndexed(m_IndexCount, 0, 0);
	}

	bool D3D11VBO::CreateIndexBuffer(unsigned * const indices, const unsigned numInidices)
	{
		if (numInidices < 1) return false;
		for (int i = 0; i < numInidices; ++i)
		{
			m_Indices.push_back(indices[i]);
		}
		m_IndexCount = numInidices;
		D3D11_BUFFER_DESC bufferDesc;
		bufferDesc.Usage = D3D11_USAGE_DEFAULT;
		bufferDesc.ByteWidth = sizeof(unsigned int) * m_IndexCount;
		bufferDesc.BindFlags = D3D11_BIND_INDEX_BUFFER;
		bufferDesc.CPUAccessFlags = 0;
		bufferDesc.MiscFlags = 0;

		D3D11_SUBRESOURCE_DATA InitData;
		InitData.pSysMem = indices;
		InitData.SysMemPitch = 0;
		InitData.SysMemSlicePitch = 0;

		const HRESULT result = DXRenderer::GetDevice()->CreateBuffer(&bufferDesc, &InitData, &m_pIndexBO);
		if (FAILED(result)) return false;
		return true;
	}

}