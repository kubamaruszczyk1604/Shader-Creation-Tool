#include "DirectionalLight.h"


namespace KLM_FRAMEWORK
{

	DirectionalLight::DirectionalLight(const Vec3 & position, const Colour & ambient, const Colour & diffuse, const Colour & specular) :LightBase()
	{
		//Vector4 pos = position;
		//pos.w(0);//point 
		SetLightInfo(LightBase::GetSlot(),
			LightType::Directional, // 
			position,
			ambient,
			diffuse,
			specular,
			glm::radians(180.0f),
			glm::vec3(0, 0, 0),
			128,
			glm::vec3(1, 0, 0)
			);
		std::cout << "Creating directional light at slot: " << LightBase::GetSlot() << std::endl;
	}

	DirectionalLight::~DirectionalLight()
	{
	}

}


