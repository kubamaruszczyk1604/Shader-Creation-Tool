#include "Spotlight.h"


namespace KLM_FRAMEWORK
{

	Spotlight::Spotlight(
		const Vec3 & position,
		const Colour & ambient,
		const Colour & diffuse,
		const Colour & specular,
		const Vec3 & direction,
		float cutoffInDeg) :LightBase()
	{

		SetLightInfo(LightBase::GetSlot(),
			LightType::Spot,
			position,
			ambient,
			diffuse,
			specular,
			glm::radians(cutoffInDeg),
			direction,
			128, //default decay
			Vec3(1, 0, 0)// attenuation
			);
		std::cout << "Creating spotlight at slot: " << LightBase::GetSlot() << std::endl;

	}

	Spotlight::Spotlight(
		const Vec3 & position,
		const Colour & ambient,
		const Colour & diffuse,
		const Colour & specular,
		const Vec3 & direction,
		float cutoffInDeg,
		float decay) :LightBase()
	{

		SetLightInfo(LightBase::GetSlot(),
			LightType::Spot,
			position,
			ambient,
			diffuse,
			specular,
			glm::radians(cutoffInDeg),
			direction,
			decay,
			Vec3(1, 0, 0)// attenuation
			);
		std::cout << "Creating spotlight at slot: " << LightBase::GetSlot() << std::endl;
	}

	void Spotlight::SetAttenuationConstants(float a, float b, float c)
	{
		LightBase::SetAttenuation(a, b, c);
	}

	void Spotlight::SetDirection(const Vec3 & direction)
	{
		LightBase::SetLightDirection(direction);
	}

	void Spotlight::SetDecay(float decay)
	{
		LightBase::SetLightDecay(decay);
	}

	void Spotlight::SetConeSize(float deg)
	{
		LightBase::SetSpotConeSize(deg);
	}

	Spotlight::~Spotlight()
	{
	}

}