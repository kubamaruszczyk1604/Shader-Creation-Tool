#include "Camera.h"

namespace KLM_FRAMEWORK
{

	Camera::Camera(const ProjectionType& projection, const float& fovDeg, const float& fnear, const float& ffar) :
		Component("comp_camera", ComponentType::CAMERA_COMPONENT),
		m_ProjectionMat(),
		m_TransformMat(),
		m_IdentityMat(),
		m_ProjectionType(projection),
		m_FOV(fovDeg),
		m_Near(fnear),
		m_Far(ffar),
		m_Active(true)

	{

	}

	Camera::~Camera()
	{
	}

	const glm::mat4 & Camera::GetProjectionMatrix(const int & scrWidth, const int & scrHeight)
	{

		if (m_ProjectionType == ProjectionType::PERSPECTIVE)
		{
			m_ProjectionMat = glm::perspectiveFovLH(glm::radians(m_FOV),
				static_cast<float>(scrWidth),
				static_cast<float>(scrHeight),
				m_Near,
				m_Far);
		}
		else
		{
			m_ProjectionMat = m_IdentityMat;
		}

		return m_ProjectionMat;
	}

	const Mat4& Camera::SetTransformMatrix(const Vec3& translation, const Vec3& rotation)
	{
		// not implemented
		return m_TransformMat;
	}



	const glm::mat4 & Camera::SetTransformMatrix(const glm::mat4 & mat)
	{
		m_TransformMat = mat;
		m_TransformMat = glm::inverse(m_TransformMat);
		return m_TransformMat;
	}
}