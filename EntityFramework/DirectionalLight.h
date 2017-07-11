#pragma once
#include "LightBase.h"

namespace KLM_FRAMEWORK
{
	class DirectionalLight : public LightBase
	{
	public:
		DirectionalLight(
			const Vec3& direction,
			const Colour& ambient,
			const Colour& diffuse,
			const Colour& specular);


		~DirectionalLight();
	};


}
