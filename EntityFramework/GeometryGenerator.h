#pragma once
#include "StdIncludes.h"
#include "Mesh.h"
namespace KLM_FRAMEWORK
{
	class GeometryGenerator
	{
	public:
		GeometryGenerator() = delete;
		GeometryGenerator(const GeometryGenerator&) = delete;
		GeometryGenerator& operator=(GeometryGenerator&) = delete;
		~GeometryGenerator();
	private:
		static const float M_PI;
		static const float M_HALF_PI;

		static void GenerateVertexRing(const Vec3& orgin,
			double radius,
			int numOfWalls,
			bool normalsIn,
			float vValue,
			std::vector<Vertex>& verts);


		static void CreateBlock(const Vec3* const vertsBase, const Vec2* const uv, Mesh*& outMesh);
		

	public:

		static void GenerateSphere(const float radius, const unsigned int rings, const unsigned int sectors, Mesh*& meshOut);
		static void GenerateBox(const float size, Mesh*& meshOut);
		static void GenerateQuad(const float a, Mesh*& meshOut);

		static void GenerateIrregPipe(
			const Vec3& front,
			const Vec3& back,
			const Vec3& frontRotation,
			const Vec3& backRotation,
			double radius1,
			double radius2,
			int numOfWalls,
			const Vec2& UV,
			Mesh*& outMesh);

		static void GenerateIrregBlock(
			float leftW, float rightW,
			float leftH, float rightH,
			float leftL, float rightL,
			float uStart, float uEnd,
			float vStart, float vEnd,
			Mesh*& outMesh);


		static void GenerateIrregBlock(
			float leftW, float rightW,
			float leftH, float rightH,
			float leftL, float rightL,
			Mesh*& outMesh);
	};

}