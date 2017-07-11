#pragma once
#include "D3DVBO.h"

namespace KLM_FRAMEWORK
{
	
	class Mesh
	{

	private:
		std::vector<Vertex> m_Vertices;
		VBO* m_pVBO;
		bool m_BuffersCreated;

	public:
		Mesh();
		~Mesh();

		bool AddVertex(const Vertex& v);
		const VBO* CreateVertexBuffer();
		const VBO* CreateVertexBuffer(std::vector<unsigned>& indices);
		VBO* GetVBO() { return m_pVBO; }


	};

}