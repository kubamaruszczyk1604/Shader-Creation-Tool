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
			if (normalsIn) {normal = orgin - currentPos;}
			else {normal = currentPos - orgin;}
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



}