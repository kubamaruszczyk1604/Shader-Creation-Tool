#pragma once
#include "StdIncludes.h"
namespace KLM_FRAMEWORK
{
	class Transform
	{

	private:
		Vec3 m_Position;
		Vec3 m_Rotation;
		Vec3 m_Scale;
		Mat4 m_World;
		Mat4 m_ParentTransformStack;

	public:
		Transform();
		Transform(const Transform&) = delete;
		Transform &operator=(const Transform&) = delete;
		virtual ~Transform();

	public:
		const Vec3& GetPosition()const { return m_Position; }
		const Vec3& GetLocalPosition()const { return m_Position; }
		const Vec3& GetRotation()const { return m_Rotation; }
		const Vec3& GetScale()const { return m_Scale; }
		const Mat4& GetWorldMat()const { return m_World; }

		void SetPosition(const Vec3& position) { m_Position = position; }
		void SetPosition(float x, float y, float z) { m_Position = Vec3(x, y, z); }
		void SetPositionX(float x) { m_Position.x = x; }
		void SetPositionY(float y) { m_Position.y = y; }
		void SetPositionZ(float z) { m_Position.z = z; }
		void UpdatePosition(Vec3 step) { m_Position += step; }
		void UpdatePosition(float x, float y, float z) { m_Position += Vec3(x, y, z); }

		void SetRotation(const Vec3& rotation) { m_Rotation = rotation; }
		void SetRotation(float x, float y, float z) { m_Rotation = Vec3(x, y, z); }
		void SetRotationX(float x) { m_Rotation.x = x; }
		void SetRotationY(float y) { m_Rotation.y = y; }
		void SetRotationZ(float z) { m_Rotation.z = z; }
		void SetRotation(KLM_AXIS axis, float rotation);
		void Rotate(KLM_AXIS axis, float rotation);
		void RotateXYZ(float x, float y, float z) 
		{ 
			m_Rotation.x += x; 
			m_Rotation.y += y; 
			m_Rotation.z += z;
		}
		void RotateX(float x) { m_Rotation.x += x; }
		void RotateY(float y) { m_Rotation.y += y; }
		void RotateZ(float z) { m_Rotation.z += z; }
	

		void SetScale(const Vec3& scale) { m_Scale = scale; }
		void SetScale(float x, float y, float z) { m_Scale = Vec3(x, y, z); }
		void SetScaleX(float x) { m_Scale.x = x; }
		void SetScaleY(float y) { m_Scale.y = y; }
		void SetScaleZ(float z) { m_Scale.z = z; }

		void        SetWorld(const glm::mat4& world) { m_World = world; }
		const Mat4& GetParentTransformStack() { return m_ParentTransformStack; }
		void        SetParentTransformStack(const Mat4& pts) { m_ParentTransformStack = pts; }

		Vec3         GetWorldPosition() { return Vec3(m_World*Vec4(0, 0, 0, 1)); }

	

	};

}