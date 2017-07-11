#include "BehaviourComponent.h"

namespace KLM_FRAMEWORK
{

	BehaviourComponent::BehaviourComponent(const std::string& ID) :
		Component(ID, ComponentType::BEHAVIOUR_COMPONENT),
		m_StartCalled{ false }
	{
	}


	BehaviourComponent::~BehaviourComponent()
	{

	}
}