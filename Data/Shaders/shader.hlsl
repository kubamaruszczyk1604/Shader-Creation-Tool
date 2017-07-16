struct light
{
	bool enabled; // 
	float4 position; // w=0 dir, (0,0,-1,0) 
	float4 ambient; // (0,0,0,1) 
	float4 diffuse; // 0:(1,1,1,1), (0,0,0,0) 
	float4 specular; // 0:(1,1,1,1), (0,0,0,0) 
	float spot_cutoff; // 180deg (dflt) xor [0,90]deg 
	float3 spot_direction; // (0,0,1) 
	float spot_exponent; // [0,128] 
	float3 attenuation; // (c:1,l:0,q:0) 
};



Texture2D colorMap:register(t0);
Texture2D specularMap:register(t1);
Texture2D normalMap:register(t2);
Texture2D heightMap:register(t3);


///////////////////////////////
/* uniform */

SamplerState colorSampler:register(s0);
StructuredBuffer<light> light_list : register(t4);




struct VIn
{
    float4 position : POSITION;
    float3 nor : NORMAL;
    float3 tang : TANGENT;
    float2 uv : TEXCOORD;
};

struct VOut
{
    float4 position : SV_POSITION;
    float4 posWorld : V_POSITION;
    float4 normal : NORMAL;
    float2 tex0 : TEXCOORD;
};
// entry

cbuffer Uniforms_PerFrame
{
    float4x4 MVP;
    float4x4 INV_TRANSP_WORLD;
    float4x4 WORLD;
    float4 Mat_Ambient;
    float4 Mat_Diffuse;
    float4 Mat_Specular;
    float time;
    float3 numLights;
    float3 camPos;
    float empty;

}






float3 get_light_vector(light lgt, float3 pos)
{
    //return float3(0, 0, -10);
	return ((0.0 != lgt.position.w) ? (lgt.position.xyz - pos)
		: (lgt.position.xyz)); 
}

float get_light_attenuation(light lgt, float dist)
{
	float attenuation_at_lpos_infty = 1.0;
	return ((0.0 != lgt.position.w) ? 1.0 / ((lgt.attenuation.x) +
		(lgt.attenuation.y * dist) +
		(lgt.attenuation.z * dist * dist)) : attenuation_at_lpos_infty);
}

void accumulate_lights(StructuredBuffer<light> lgt, float3 pos, float3 norm, float3 eye, float shininess, inout float4 iambient, inout float4 idiffuse, inout float4 ispecular, int numL)
{
	uint lgt_size = 0, dummy = 0; lgt.GetDimensions(lgt_size, dummy);
	for (uint index = 0; index < numL; ++index)
	{
		if (lgt[index].enabled)
		{
			//iambient = float4(1, 1, 0, 1);
			float3 lvec = get_light_vector(lgt[index], pos);
				float attenuation = get_light_attenuation(lgt[index], length(lvec));
			lvec = normalize(lvec);
			float NdotL = max(dot(norm, lvec), 0.0);
			if (0.0 < NdotL)
			{
				if (2 == lgt[index].position.w)
				{
					float cos_cur_angle = dot(-lvec, normalize(lgt[index].spot_direction));
					float cos_outer_angle = saturate(cos(lgt[index].spot_cutoff));
					float cos_inner_angle = saturate(cos((1.0 - (lgt[index].spot_exponent / 128.0)) * lgt[index].spot_cutoff));
					attenuation *= saturate((cos_cur_angle - cos_outer_angle) / (cos_inner_angle - cos_outer_angle));
				}
				float3 H = normalize(lvec - normalize(pos - eye));
					float NdotH = max(dot(norm, H), 0.0);
				float exponent = max(128.0 / max(0.0, min(128.0, shininess)), 0.0);
				float4 lit_result = lit(NdotL, NdotH, exponent);

			    iambient += (attenuation * lgt[index].ambient * lit_result.x);
				idiffuse += (attenuation * lgt[index].diffuse * lit_result.y);
				ispecular += (attenuation * lgt[index].specular * lit_result.z);
			}
		}

	}
}


float softInterval(float a, float b, float c, float d, float x)
{
	return smoothstep(a, b, x) - smoothstep(c, d, x);
}

float4 UnderwaterEffect(float4 inCol,float3 cameraPos,float3 objPos,float maxDist)
{
	float lng = length(cameraPos - objPos);
	if (lng > maxDist) lng = maxDist;
	return inCol - lng* float4(0.005, 0.002, 0.00001, 0);
}


float4 UnderwaterEffect(float4 inCol, float3 cameraPos, float3 objPos, float maxDist,float4 subtractStep)
{
	float lng = length(cameraPos - objPos);
	if (lng > maxDist) lng = maxDist;
	return inCol - lng * subtractStep;
}
////////////////////////////////////////////////////////








VOut VShader(VIn input)
{
	VOut output;
	float4 vertex = input.position;

    output.position = mul(MVP, float4(vertex.xyz, 1));
    output.posWorld = mul(WORLD, float4(vertex.xyz, 1));
	output.normal = mul(INV_TRANSP_WORLD, normalize(float4(input.nor, 0)));
 
	output.tex0 = input.uv;

	return output;
}


VOut VShaderPart(VIn input)
{
	VOut output;
	float4 inp = input.position;

	float4 transformedPos = mul(MVP, float4(inp.xyz, 1));
	float4 worldPos = mul(WORLD, float4(inp.xyz, 1));

	output.position = transformedPos;
	output.posWorld = worldPos;
	output.tex0 = input.uv;

	return output;
}

float4 PSParticle(float4 position : SV_POSITION, float4 posWorld : V_POSITION, float4 normal : NORMAL, float2 tex0 : TEXCOORD) : SV_TARGET
{
	float4 result  = float4(0,0, 0, 0);
	float4 samp = (0.0 < numLights.z) ? colorMap.Sample(colorSampler, tex0) : float4(1, 1, 1, 1);

	result +=  Mat_Ambient*samp;
	result.a *= numLights.y;
	result.r -= numLights.y*0.25;
	result.g -= numLights.y*0.25;
	//result.a = Mat_Diffuse.a;


	return result;//UnderwaterEffect(result, camPos, posWorld.xyz, 70);
}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
VOut VShaderFish(VIn input)
{
	VOut output;
	float4 inp = input.position;

		float d = (1.0* sin(cos(5.3 * time)) * softInterval(-5.5, -5.0, -4.0, 0.0, inp.x))* sign(inp.x);
	inp.z += d;


	float4 transformedPos = mul(MVP, float4(inp.xyz, 1));
		float4 worldPos = mul(WORLD, float4(inp.xyz, 1));

		output.position = transformedPos;
	output.posWorld = worldPos;
	output.normal = mul(INV_TRANSP_WORLD, normalize(float4(input.nor, 0)));
	//output.normal = mul(MVP, float4(input.nor,0));
	//output.normal = float4(input.nor.x, input.nor.y, input.nor.z, 1);
	output.tex0 = input.uv;

	return output;
}

VOut VShaderFishFast(VIn input)
{
	VOut output;
	float4 inp = input.position;

	float d = (0.7* sin(cos(19.3 * time)) * softInterval(-5.5, -5.0, -4.0, 0.0, inp.x))* sign(inp.x);
	inp.z += d;


	float4 transformedPos = mul(MVP, float4(inp.xyz, 1));
	float4 worldPos = mul(WORLD, float4(inp.xyz, 1));

	output.position = transformedPos;
	output.posWorld = worldPos;
	output.normal = mul(INV_TRANSP_WORLD, normalize(float4(input.nor, 0)));
	output.tex0 = input.uv;

	return output;
}


VOut VShaderFishFins(VIn input)
{
	VOut output;
	float4 inp = input.position;

	float d = (0.7 *sin(cos(5.3 * time)) * softInterval(-2.5, -1.0, -1.0, 0.0, inp.x))* sign(inp.x);
	inp.z += d;

	float4 transformedPos = mul(MVP, float4(inp.xyz, 1));
	float4 worldPos = mul(WORLD, float4(inp.xyz, 1));

	output.position = transformedPos;
	output.posWorld = worldPos;
	output.normal = mul(INV_TRANSP_WORLD, normalize(float4(input.nor, 0)));
	output.tex0 = input.uv;

	return output;
}


VOut VShaderManta(VIn input)
{
	VOut output;
	float4 inp = input.position;
	
    //deformation moved to cpu
	/*inp.y += softInterval(-2, 0.0, 0.0, 2, inp.x);
	inp.y += softInterval(-3, -1.0, 1.0, 3, inp.x);

	
	if (abs(inp.x) > 3.5)
	{
		if (inp.y < 1.3)
		{
			inp.y += 2 * abs(1 / inp.x*inp.y);
		}
		inp.y -= abs(1 / inp.x*inp.y);
		inp.x *= 1.5;

		if (inp.y<0)inp.y *= 4 * abs(1 / inp.x);
	}*/
	
	
	//movement
	float d = (1.0* sin(cos(3 * time)) * softInterval(-12.5, -8.0, -7.0, 0.0, inp.x))* sign(inp.x);
	float e = -(1.0* sin(cos(3 * time)) * softInterval(0.0, 7.0, 8.0, 12.5, inp.x))* sign(inp.x);
	inp.z += d + e;



	float4 transformedPos = mul(MVP, float4(inp.xyz, 1));
		float4 worldPos = mul(WORLD, float4(inp.xyz, 1));

		output.position = transformedPos;
	output.posWorld = worldPos;
	output.normal = mul(INV_TRANSP_WORLD, normalize(float4(input.nor, 0)));
	output.tex0 = input.uv;

	return output;
}


float4 fp_main(float4 position : SV_POSITION, float4 posWorld : V_POSITION, float4 normal : NORMAL, float2 tex0 : TEXCOORD) : SV_TARGET
{
	float4 result;
	float4 ambientL = float4(0, 0, 0, 0);
	float4 diffuseL = float4(0, 0, 0, 0);
	float4 specularL = float4(0, 0, 0, 0);

	accumulate_lights(light_list, posWorld.xyz, normalize(normal.xyz), camPos, 0.1, ambientL, diffuseL, specularL, 20);


    
	float4 samp =  colorMap.Sample(colorSampler, tex0);
    result = float4(0, 0, 0, 0);

	 result += Mat_Ambient * samp;
     result += diffuseL* Mat_Diffuse * samp;
	 result += specularL*Mat_Specular;
    //result.a = 0.6f;

	return result;

}





float4 PShader2(float4 position : SV_POSITION, float4 posWorld : V_POSITION, float4 normal : NORMAL, float2 tex0 : TEXCOORD) : SV_TARGET
{
	return normal* Mat_Ambient;  
}

float4 PSUIShader(float4 position : SV_POSITION, float4 posWorld : V_POSITION, float4 normal : NORMAL, float2 tex0 : TEXCOORD) : SV_TARGET
{
	//normal.a = 0.3;
	//return normal*Mat_Ambient; //float4(1,1,1,1);// color * globalColour * kub;
    return Mat_Ambient * colorMap.Sample(colorSampler, tex0);
}


float4 PSTextShader(float4 position : SV_POSITION, float4 posWorld : V_POSITION, float4 normal : NORMAL, float2 tex0 : TEXCOORD) : SV_TARGET
{
	return colorMap.Sample(colorSampler, tex0) + Mat_Ambient + float4(0, 0, 0, -1.0);

}