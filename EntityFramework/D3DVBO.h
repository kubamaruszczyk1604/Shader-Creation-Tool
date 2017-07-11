#pragma once
#include "VBO.h"

#include "D3DRenderer.h"

namespace KLM_FRAMEWORK
{


	class D3D11VBO:public VBO
	{
	private:
		static const std::array<D3D_PRIMITIVE_TOPOLOGY, 5> const D3D11VBO::LookupTableTopologyType;

		std::vector<unsigned> m_Indices;
		ID3D11Buffer* m_pVBO;
		ID3D11Buffer* m_pIndexBO;
		int m_VertexCount;
		int m_IndexCount;

	public:
		D3D11VBO();
		~D3D11VBO();
		D3D11VBO(const D3D11VBO&) = delete;
		D3D11VBO& operator=(const D3D11VBO&) = delete;

		bool Create(Vertex* vertices, int numVertices);
		void Draw(PrimitiveType primitiveType)const;;
		bool CreateIndexBuffer(unsigned * const indices, const unsigned numInidices);
		const std::vector<unsigned> const& GetIndices()const { return m_Indices; };
	};

}