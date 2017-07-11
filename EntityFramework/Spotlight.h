#pragma once
#include "LightBase.h"

namespace KLM_FRAMEWORK
{
	class Spotlight : public LightBase
	{
	public:
		Spotlight::Spotlight(
			const Vec3& position,
			const Colour& ambient,
			const Colour&  diffuse,
			const Colour&  specular,
			const Vec3& direction,
			float cutoffInDeg);


		Spotlight::Spotlight(
			const Vec3& position,
			const Colour& ambient,
			const Colour& diffuse,
			const Colour& specular,
			const Vec3& direction,
			float cutoffInDeg,
			float decay);


		void SetAttenuationConstants(float a, float b, float c);
		void SetDirection(const Vec3& direction);
		void SetDecay(float decay);
		void SetConeSize(float deg);

		~Spotlight();
	};

}