#include "GeometryGenerator.h"

namespace KLM_FRAMEWORK
{

	GeometryGenerator::~GeometryGenerator() {}

	const float GeometryGenerator::M_PI = 3.14159265358979323846f;
	const float GeometryGenerator::M_HALF_PI = M_PI * 0.5f;

	void GeometryGenerator::GenerateVertexRing(
		const Vec3 & orgin,
		double radius,
		int numOfWalls,
		bool normalsIn,
		float vValue,
		std::vector<Vertex>& verts)
	{
		double theta = (M_PI *2.0) / numOfWalls;
		Vertex center(orgin);
		center.Texcoord0 = Vec2(0, 0);
		verts.push_back(center);
		for (int i = 0; i < numOfWalls; ++i)
		{
			glm::vec3 currentPos = glm::vec3(
				orgin.x + sin((i)*theta) * radius,
				orgin.y + cos((i)*theta) * radius,
				orgin.z);


			//normals calculation
			glm::vec3 normal;
			if (normalsIn) { normal = orgin - currentPos; }
			else { normal = currentPos - orgin; }
			normal = glm::normalize(normal);

			Vertex temp(currentPos);
			temp.Normal = normal;
			temp.Texcoord0 = Vec2(i, vValue);

			verts.push_back(temp);

			if (i == numOfWalls - 1)
			{
				Vertex tmp = verts[1];
				tmp.Texcoord0.x = numOfWalls;
				verts.push_back(tmp);
			}
		}
	}


	void GeometryGenerator::CreateBlock(const Vec3 * const vertsBase, const Vec2 * const uv, Mesh *& outMesh)
	{


		const glm::vec3 frontNormal = glm::vec3(0, 0, 1);
		const glm::vec3 backNormal = glm::vec3(0, 0, -1);
		const glm::vec3 leftNormal = glm::vec3(-1, 0, 0);
		const glm::vec3 rightNormal = glm::vec3(1, 0, 0);
		const glm::vec3 bottomNormal = glm::vec3(0, -1, 0);
		const glm::vec3 topNormal = glm::vec3(0, 1, 0);

		outMesh = new Mesh();

		//front face
		for (int i = 0; i < 4; ++i)
		{
			Vertex v(vertsBase[i]);
			v.Normal = frontNormal;
			v.Texcoord0 = uv[i];
			outMesh->AddVertex(v);
		}

		//back face
		for (int i = 0; i < 4; ++i)
		{
			Vertex v(vertsBase[i]);
			v.Position.z *= -1;
			v.Normal = backNormal;
			v.Texcoord0 = uv[i];
			outMesh->AddVertex(v);
		}

		//Left face
		outMesh->AddVertex(outMesh->GetVertexRef(4));
		outMesh->AddVertex(outMesh->GetVertexRef(5));
		outMesh->AddVertex(outMesh->GetVertexRef(1));
		outMesh->AddVertex(outMesh->GetVertexRef(0));

		for (int i = 8; i < 12; ++i)
		{
			Vertex v = outMesh->GetVertexRef(i);
			v.Normal = leftNormal;
			v.Texcoord0 = uv[i - 8];
			outMesh->GetVertexRef(i) = v;
		}

		//Right face
		outMesh->AddVertex(outMesh->GetVertexRef(3));
		outMesh->AddVertex(outMesh->GetVertexRef(2));
		outMesh->AddVertex(outMesh->GetVertexRef(6));
		outMesh->AddVertex(outMesh->GetVertexRef(7));

		for (int i = 12; i < 16; ++i)
		{
			Vertex v = outMesh->GetVertexRef(i);
			v.Normal = rightNormal;
			v.Texcoord0 = uv[i - 12];
			outMesh->GetVertexRef(i) = v;
		}

		//bottom face
		outMesh->AddVertex(outMesh->GetVertexRef(3));
		outMesh->AddVertex(outMesh->GetVertexRef(7));
		outMesh->AddVertex(outMesh->GetVertexRef(4));
		outMesh->AddVertex(outMesh->GetVertexRef(0));

		for (int i = 16; i < 20; ++i)
		{
			Vertex v = outMesh->GetVertexRef(i);
			v.Normal = bottomNormal;
			v.Texcoord0 = uv[i - 16];
			outMesh->GetVertexRef(i) = v;
		}

		//top face
		outMesh->AddVertex(outMesh->GetVertexRef(1));
		outMesh->AddVertex(outMesh->GetVertexRef(5));
		outMesh->AddVertex(outMesh->GetVertexRef(6));
		outMesh->AddVertex(outMesh->GetVertexRef(2));

		for (int i = 20; i < 24; ++i)
		{
			Vertex v = outMesh->GetVertexRef(i);
			v.Normal = topNormal;
			v.Texcoord0 = uv[i - 20];
			outMesh->GetVertexRef(i) = v;
		}
		//front culling
		//unsigned indices[] =
		//{
		//	0,1,2, 0,2,3,       // front face
		//	7,6,5, 7,5,4,       // back face
		//	8,9,10, 8,10,11,    // left face
		//	12,13,14, 12,14,15, // right face
		//	16,17,18, 16,18,19, // bottom face
		//	20,21,22, 20,22,23 // top face
		//};

		////back culling
		const unsigned const indices[] = // parasoft-suppress  STL-37 "Array preffered as a local const lookup table of const values - no sorting intended ever"
		{
			0,3,2, 0,2,1,       // front face
			7,4,5, 7,5,6,       // back face
			8,11,10, 8,10,9,    // left face
			12,15,14, 12,14,13, // right face
			16,19,18, 16,18,17, // bottom face
			20,23,22, 20,22,21 // top face
		};
		outMesh->CreateVertexBuffer(std::vector<unsigned>(indices, indices + 36));
	}

	void GeometryGenerator::GenerateSphere(const float radius, const unsigned int rings, const unsigned int sectors, Mesh*& meshOut)
	{
		float const R = 1. / (float)(rings - 1);
		float const S = 1. / (float)(sectors - 1);
		int r = 0;
		int s = 0;

		meshOut = new Mesh();
		for (r = 0; r < rings; r++) for (s = 0; s < sectors; s++) {
			float const y = sin(-M_HALF_PI + M_PI * r * R);
			float const x = cos(2 * M_PI * s * S) * sin(M_PI * r * R);
			float const z = sin(2 * M_PI * s * S) * sin(M_PI * r * R);

			Vec2 UV(s*S, r*R);
			Vec3 Norm(x, y, z);
			Vec3 Pos(x * radius, y * radius, z * radius);

			meshOut->AddVertex(Vertex(Pos.x, Pos.y, Pos.z, Norm.x, Norm.y, Norm.z, UV.x, UV.y));

		}

		std::vector<unsigned> sphere_indices;
		for (r = 0; r < rings; r++) for (s = 0; s < sectors; s++) {

			sphere_indices.push_back((r + 1) * sectors + (s + 1));
			sphere_indices.push_back(r * sectors + (s + 1));
			sphere_indices.push_back(r * sectors + s);

			sphere_indices.push_back((r + 1) * sectors + s);
			sphere_indices.push_back((r + 1) * sectors + (s + 1));
			sphere_indices.push_back((r * sectors + s));

		}

		meshOut->CreateVertexBuffer(sphere_indices);

	}

	void GeometryGenerator::GenerateBox(const float size, Mesh *& meshOut)
	{

		meshOut = new Mesh();

		//meshOut->AddVertex(Vertex(Pos.x, Pos.y, Pos.z, Norm.x, Norm.y, Norm.z, UV.x, UV.y));
		std::vector<unsigned> sphere_indices;
		meshOut->CreateVertexBuffer(sphere_indices);

	}

	void GeometryGenerator::GenerateQuad(const float a, Mesh *& meshOut)
	{
		meshOut = new Mesh();

		float size = a;
		float fbDist = 0.01f;

		//front
		meshOut->AddVertex(Vertex(-size, -size, -fbDist, 0, 0, -1, 0, 0));
		meshOut->AddVertex(Vertex(size, -size, -fbDist, 0, 0, -1, 1, 0));
		meshOut->AddVertex(Vertex(size, size, -fbDist, 0, 0, -1, 1, 1));
		meshOut->AddVertex(Vertex(-size, size, -fbDist, 0, 0, -1, 0, 1));

		std::vector<unsigned> indices;
		indices.push_back(0);
		indices.push_back(1);
		indices.push_back(2);
		indices.push_back(0);
		indices.push_back(2);
		indices.push_back(3);

		meshOut->CreateVertexBuffer(indices);
	}



	void GeometryGenerator::GenerateIrregPipe(
		const Vec3 & front,
		const Vec3 & back,
		const Vec3 & frontRotation,
		const Vec3 & backRotation,
		double radius1, double radius2,
		int numOfWalls, const Vec2 & UV,
		Mesh *& outMesh)
	{



		std::vector<Vertex> verts;
		std::vector<unsigned> indices;

		//front
		GenerateVertexRing(front, radius1, numOfWalls, true, 0, verts);

		//////////////////////////
		for (int i = 0; i < verts.size(); ++i)
		{
			//Prepare rotation matrix
			glm::mat4 rotationMatrix;
			rotationMatrix = glm::rotate(rotationMatrix, frontRotation.x, glm::vec3(1, 0, 0));
			rotationMatrix = glm::rotate(rotationMatrix, frontRotation.y, glm::vec3(0, 1, 0));
			rotationMatrix = glm::rotate(rotationMatrix, frontRotation.z, glm::vec3(0, 0, 1));

			Vertex tmp = verts[i];
			glm::vec4 position(tmp.Position, 1);
			glm::vec4 normal(tmp.Normal, 0);

			glm::vec4 translation(front, 1);
			position = position - translation;
			position = rotationMatrix*position;
			position = position + translation;
			normal = rotationMatrix*normal;

			verts[i].Position = glm::vec3(position);
			verts[i].Normal = glm::vec3(normal);
		}

		//////  back
		std::vector<Vertex> backVerts;
		GenerateVertexRing(back,
			radius2, numOfWalls, true, UV.y, backVerts);

		for (int i = 0; i < backVerts.size(); ++i)
		{

			glm::mat4 rotationMatrix;
			rotationMatrix = glm::rotate(rotationMatrix, backRotation.x, glm::vec3(1, 0, 0));
			rotationMatrix = glm::rotate(rotationMatrix, backRotation.y, glm::vec3(0, 1, 0));
			rotationMatrix = glm::rotate(rotationMatrix, backRotation.z, glm::vec3(0, 0, 1));

			Vertex tmp = backVerts[i];
			glm::vec4 position(tmp.Position, 1);
			glm::vec4 normal(tmp.Normal, 0);

			glm::vec4 translation(back, 1);

			position = position - translation;
			position = rotationMatrix*position;
			position = position + translation;
			normal = rotationMatrix*normal;

			backVerts[i].Position = glm::vec3(position);
			backVerts[i].Normal = glm::vec3(normal);


		}
		verts.insert(verts.end(), backVerts.begin(), backVerts.end());

		for (int i = 0; i < verts.size(); ++i)
		{
			verts[i].Texcoord0.x *= UV.x;
		}

		for (int wall = 1; wall < numOfWalls + 1; ++wall)
		{

			int j = wall + 1;
			//if (j > numOfWalls) j = 1;

			int opposite_i = wall + 2 + numOfWalls;
			int opposite_j = j + 2 + numOfWalls;

			indices.push_back(wall);
			indices.push_back(j);
			indices.push_back(opposite_j);

			indices.push_back(wall);
			indices.push_back(opposite_j);
			indices.push_back(opposite_i);
		}

		outMesh = new Mesh();
		for (int i = 0; i < verts.size(); ++i)
		{
			outMesh->AddVertex(verts[i]);
		}

		outMesh->CreateVertexBuffer(indices);

	}


	void GeometryGenerator::GenerateIrregBlock(
		float leftW, float rightW,
		float leftH, float rightH,
		float leftL, float rightL,
		float uStart, float uEnd,
		float vStart, float vEnd, Mesh *& outMesh)
	{
		const glm::vec3 const vertsBase[] = // parasoft-suppress  STL-37 "Array preffered as a local const lookup table of const values - no sorting intended ever"
		{
			glm::vec3(-leftW,-leftH, leftL),
			glm::vec3(-leftW, leftH, leftL),
			glm::vec3(rightW, rightH, rightL),
			glm::vec3(rightW, -rightH, rightL)
		};
		const glm::vec2 const uv[] = // parasoft-suppress  STL-37 "Array preffered as a local const lookup table of const values - no sorting intended ever"
		{
			glm::vec2(uStart,vEnd),
			glm::vec2(uStart, vStart),
			glm::vec2(uEnd, vStart),
			glm::vec2(uEnd,vEnd)
		};
		CreateBlock(vertsBase, uv, outMesh);

	}

	void GeometryGenerator::GenerateIrregBlock(
		float leftW, float rightW,
		float leftH, float rightH,
		float leftL, float rightL, Mesh*& outMesh)
	{
		const glm::vec3 const vertsBase[] = // parasoft-suppress  STL-37 "Array preffered as a local const lookup table of const values - no sorting intended ever"
		{
			glm::vec3(-leftW,-leftH, leftL),
			glm::vec3(-leftW, leftH, leftL),
			glm::vec3(rightW, rightH, rightL),
			glm::vec3(rightW, -rightH, rightL)
		};
		const glm::vec2 const uv[] = // parasoft-suppress  STL-37 "Array preffered as a local const lookup table of const values - no sorting intended ever"
		{
			glm::vec2(0,1),
			glm::vec2(0, 0),
			glm::vec2(1, 0),
			glm::vec2(1,1)
		};
		CreateBlock(vertsBase, uv, outMesh);
	}
}