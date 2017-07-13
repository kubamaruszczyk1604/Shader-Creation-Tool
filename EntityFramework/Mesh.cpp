#include "Mesh.h"
#include "Renderer.h"

namespace KLM_FRAMEWORK
{
	

	Mesh::Mesh() :m_BuffersCreated{ false }
	{
		if (Renderer::GetAPI() == GfxAPI::D3D11)
		{
			m_pVBO = new D3D11VBO();
		}
		else if (Renderer::GetAPI() == GfxAPI::GL)
		{
			m_pVBO = new GLVBO();
		}
	}

	Mesh::~Mesh()
	{
		delete m_pVBO;
	}


	bool Mesh::AddVertex(const Vertex & v)
	{
		if (!m_BuffersCreated)
		{
			m_Vertices.push_back(v);
			return true;
		}
		return false;
	}

	const VBO* Mesh::CreateVertexBuffer()
	{
		m_BuffersCreated = true;
		m_pVBO->Create(&m_Vertices[0], m_Vertices.size());
		return m_pVBO;
	}

	const VBO * Mesh::CreateVertexBuffer(std::vector<unsigned>& indices)
	{
		CreateVertexBuffer();
		m_pVBO->CreateIndexBuffer(&indices[0], indices.size());
		return m_pVBO;
	}

}