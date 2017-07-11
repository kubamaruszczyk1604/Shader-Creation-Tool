#pragma once
#include "StdIncludes.h"

namespace KLM_FRAMEWORK
{
	struct ShaderLightInfoStruct
	{
		bool Enabled = 0; // 0:true, false 
		Vec4 Position; // w=0 dir, (0,0,-1,0) 
		Colour  Ambient; // (0,0,0,1) 
		Colour  Diffuse; // 0:(1,1,1,1), (0,0,0,0) 
		Colour  Specular; // 0:(1,1,1,1), (0,0,0,0) 
		float SpotCutoff; // 180deg (dflt) xor [0,90]deg 
		Vec3 SpotDirection; // (0,0,1) 
		float SpotDecay; // [0,128] 
		Vec3 Attenuation; // (c:1,l:0,q:0) 
	};
}