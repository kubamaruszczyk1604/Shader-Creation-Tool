#pragma once
#include "StdIncludes.h"
#include "Component.h"

namespace KLM_FRAMEWORK
{
	enum class ProjectionType
	{
		ORTHO = 0,
		PERSPECTIVE = 1
	};


	class Entity;
	class Camera : public Component
	{

	private:
		glm::mat4 m_ProjectionMat;
		glm::mat4 m_TransformMat;
		glm::mat4 m_IdentityMat;

		ProjectionType m_ProjectionType;
		float m_FOV;
		float m_Near;
		float m_Far;
		glm::vec3 m_NoParrentPos;
		glm::vec3 m_NoParrentRotation;
		bool m_Active;

	public:
		Camera(const ProjectionType& projection, const float& fovDeg, const float& fnear, const float& ffar);
		virtual ~Camera();

		Camera(const Camera&) = delete;
		Camera &operator=(const Camera&) = delete;

	public:

		float GetFOV()const { return m_FOV; }
		float GetNear()const { return  m_Near; }
		float GetFar()const { return m_Far; }
		ProjectionType GetProjectionType()const { return m_ProjectionType; }
		void SetActive(bool active) { m_Active = active; }
		bool isActive()const { return  m_Active; }

		Vec4 GetPosition() const { return m_TransformMat* Vec4(0, 0, 0, 1); }

		const glm::mat4& GetProjectionMatrix(const int& scrWidth, const int& scrHeight);
		const glm::mat4& SetTransformMatrix(const Vec3& translation, const Vec3& rotation);
		const glm::mat4& SetTransformMatrix(const glm::mat4& mat);
		const glm::mat4& GetViewMatrix()const { return glm::inverse(m_TransformMat); }
		const glm::mat4& GetTransformMatrix()const { return m_TransformMat; }

		inline Vec3 GetLocalPosition() const { return m_NoParrentPos; }
		inline Vec3 GetLocalRotation() const { return m_NoParrentRotation; }

	};
}