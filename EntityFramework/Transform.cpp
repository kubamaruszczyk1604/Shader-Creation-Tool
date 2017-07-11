#include "Transform.h"

namespace KLM_FRAMEWORK
{

	Transform::Transform() :m_Position{ 0, 0, 0 }, m_Rotation{ 0, 0, 0 }, m_Scale{ 1, 1, 1 }
	{
	}


	Transform::~Transform()
	{
	}
	void Transform::SetRotation(KLM_AXIS axis, float roation)
	{
		if (axis == KLM_AXIS::X_AXIS) m_Rotation.x = roation;
		else if (axis == KLM_AXIS::Y_AXIS) m_Rotation.y = roation;
		else if (axis == KLM_AXIS::Z_AXIS) m_Rotation.z = roation;
	}
	void Transform::Rotate(KLM_AXIS axis, float roation)
	{
		if (axis == KLM_AXIS::X_AXIS) m_Rotation.x += roation;
		else if (axis == KLM_AXIS::Y_AXIS) m_Rotation.y += roation;
		else if (axis == KLM_AXIS::Z_AXIS) m_Rotation.z += roation;
	}
}