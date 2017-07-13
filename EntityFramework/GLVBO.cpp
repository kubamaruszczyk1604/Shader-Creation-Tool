#include "GLVBO.h"


namespace KLM_FRAMEWORK
{


	const std::array<int, 5> const GLVBO::LookupTableTopologyType =
	{
		GL_POINTS,
		GL_LINES,
		GL_LINE_STRIP,
		GL_TRIANGLES,
		GL_TRIANGLE_STRIP
	};

	GLVBO::GLVBO() :
		VBO(),
		ptr_VBO{ 0,0 }, 
		ptr_VAO{ 0 },
		m_IndexCount{ 0 }
	{
	}
	GLVBO::~GLVBO()
	{
	}
	bool GLVBO::Create(Vertex * vertices, int numVertices)
	{

		//VBO
		glGenBuffers(2, ptr_VBO);// two buffers: [0] - for vertex buffer, [1] - for index buffer.
		glBindBuffer(GL_ARRAY_BUFFER, ptr_VBO[0]); //bind [0] as array buffer
		glBufferData(GL_ARRAY_BUFFER, numVertices*sizeof(Vertex), vertices, GL_STATIC_DRAW); //Copy data to buffer

		//VAO
		glGenVertexArrays(1, &ptr_VAO);
		glBindVertexArray(ptr_VAO);
		glBindBuffer(GL_ARRAY_BUFFER, ptr_VBO[0]);
		glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), 0);// position
		glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), reinterpret_cast<const GLvoid*>(3 * sizeof(float)));
		glVertexAttribPointer(2, 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), reinterpret_cast<const GLvoid*>(6 * sizeof(float)));
		glVertexAttribPointer(3, 2, GL_FLOAT, GL_FALSE, sizeof(Vertex), reinterpret_cast<const GLvoid*>(9 * sizeof(float)));

		glEnableVertexAttribArray(0); //position
		glEnableVertexAttribArray(1); //normal
		glEnableVertexAttribArray(2); //tangent
		glEnableVertexAttribArray(3); //uv

		m_VertexCount = numVertices;
		return true;
	}
	void GLVBO::Draw(PrimitiveType primitiveType) const
	{
		glBindVertexArray(ptr_VAO);
		if (m_IndexCount == 0)
		{
			glDrawArrays(GL_TRIANGLES, 0, m_VertexCount);
		}
		else
		{
			glDrawElements(GL_TRIANGLES, m_IndexCount, GL_UNSIGNED_INT, 0);
		}
	}
	bool GLVBO::CreateIndexBuffer(unsigned * const indices, const unsigned numInidices)
	{
		if (numInidices < 1) return false;
		for (int i = 0; i < numInidices; ++i)
		{
			m_Indices.push_back(indices[i]);
		}
		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ptr_VBO[1]);
		glBufferData(GL_ELEMENT_ARRAY_BUFFER, numInidices*sizeof(int), &indices[0], GL_STATIC_DRAW);
		m_IndexCount = numInidices;
		m_indicesPtr = (int*)indices;

		return true;
	}
}
