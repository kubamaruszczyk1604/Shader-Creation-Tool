#pragma once
#include "Vertex.h"


namespace KLM_FRAMEWORK
{
	enum class PrimitiveType
	{
		POINTS = 0,
		LINES = 1,
		LINE_STRIP = 2,
		TRIANGLES = 3,
		TRIANGLESTIRP = 4
	};

	class VBO
	{
		public:

		VBO() = default;
		virtual ~VBO(){}
		VBO(const VBO&) = delete;
		VBO& operator=(const VBO&) = delete;

		virtual bool Create(Vertex* vertices, int numVertices) = 0;
		virtual void Draw(PrimitiveType primitiveType)const = 0;
		virtual bool CreateIndexBuffer(unsigned * const indices, const unsigned numInidices) = 0;
		virtual const std::vector<unsigned> const& GetIndices()const = 0;
	};

}