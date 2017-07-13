#pragma once
#include "Vertex.h"
#include "StdIncludes.h"
#include "VBO.h"

namespace KLM_FRAMEWORK
{
	class GLVBO: public VBO
	{

	private:
		static const std::array<int, 5> const LookupTableTopologyType;
		GLuint ptr_VBO[2];
		GLuint ptr_VAO;
		int m_VertexCount;
		int m_IndexCount;
		int *m_indicesPtr;

	public:
		GLVBO();
		~GLVBO();

		GLVBO& operator=(const GLVBO&) = delete;
		GLVBO(const GLVBO&) = delete;

		bool Create(Vertex* vertices, int numVertices);
		void Draw(PrimitiveType primitiveType) const;
		bool CreateIndexBuffer(unsigned* indices, int numIndices);


	};
}
