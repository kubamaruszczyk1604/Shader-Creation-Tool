#pragma once
#include "StdIncludes.h"

namespace KLM_FRAMEWORK
{
	struct Vertex
	{
		//float x, y, z;
		Vec3 Position;
		Vec3 Normal;
		Vec3 Tangent;
		Vec2 Texcoord0;

		Vertex(float x, float y, float z)
		{
			Position = Vec3(x, y, z);
		}
		Vertex(const Vec3& pos)
		{
			Position = pos;
		}

		Vertex(float x, float y, float z, float u, float v)
		{
			Position = Vec3(x, y, z);
			Texcoord0.x = u;
			Texcoord0.y = v;
		}
		Vertex(float x, float y, float z, float nx, float ny, float nz, float u, float v)
		{
			Position = Vec3(x, y, z);
			Normal = Vec3(nx, ny, nz);
			Texcoord0.x = u;
			Texcoord0.y = v;
		}
	};

}