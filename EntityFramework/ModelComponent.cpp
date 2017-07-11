#include "ModelComponent.h"

namespace KLM_FRAMEWORK
{

	ModelComponent::ModelComponent(const std::string& ID, Mesh* const mesh, Material*const material) :
		Component(ID, ComponentType::MODEL_COMPONENT),
		p_Mesh{ mesh },
		p_Material{ material }
	{
	}

	ModelComponent::~ModelComponent()
	{
	}
}