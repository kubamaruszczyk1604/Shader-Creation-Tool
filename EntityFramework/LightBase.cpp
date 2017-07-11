#include "LightBase.h"



namespace KLM_FRAMEWORK
{

	//////////////////////////////////////// LIGHT - STATIC MEMBERS ///////////////////////////////////////////
	const unsigned LightBase::MAX_LIGHTS = 20;
	ShaderLightInfoStruct LightBase::m_LightInfo[MAX_LIGHTS];
	bool LightBase::UpdateRequest{ false };

	std::vector<int> LightBase::s_AvailableSlotsIndices;

	void LightBase::RequestUpdate()
	{
		UpdateRequest = true;
	}

	LightBase::LightBase()
	{

		if (s_AvailableSlotsIndices.size() > 0)
		{
			m_Slot = s_AvailableSlotsIndices.back();
			s_AvailableSlotsIndices.pop_back();
		}

		else m_Slot = 0;

		RequestUpdate();
	};

	void LightBase::UpdateFinished()
	{
		UpdateRequest = false;
	}

	void LightBase::InitializeLightSystem()
	{
		for (int i = 0; i < MAX_LIGHTS; ++i)
		{
			s_AvailableSlotsIndices.push_back(i);
		}
	}

	bool LightBase::IsRequestingUpdate()
	{
		return UpdateRequest;
	}

	LightBase::~LightBase()
	{
		m_LightInfo[m_Slot].Enabled = false;
		s_AvailableSlotsIndices.push_back(m_Slot);
	}

	void LightBase::SetPosition(const Vec3 & position, int type)
	{
		m_LightInfo[m_Slot].Position = Vec4(position, type);
		RequestUpdate();
	}

	void LightBase::UpdatePosition(float x, float y, float z)
	{
		m_LightInfo[m_Slot].Position += Vec4(x, y, z, 0);
		RequestUpdate();
	}

	void LightBase::UpdatePosition(const Vec3& xyz)
	{
		m_LightInfo[m_Slot].Position += Vec4(xyz, 0);
		RequestUpdate();
	}

	void LightBase::SetAmbient(const Colour & ambient)
	{
		m_LightInfo[m_Slot].Ambient = ambient;
		RequestUpdate();
	}

	void LightBase::SetDiffuse(const Colour & diffuse)
	{
		m_LightInfo[m_Slot].Diffuse = diffuse;
		RequestUpdate();
	}

	void LightBase::SetSpecular(const Colour & specular)
	{
		m_LightInfo[m_Slot].Specular = specular;
		RequestUpdate();
	}

	void LightBase::SetLightInfo(int index,
		LightType type,
		const Vec3& position,
		const Colour& ambient,
		const Colour& diffuse,
		const Colour& specular,
		float spotCutoff,
		const Vec3& direction,
		float spotDecay,
		const Vec3& att)
	{
		m_LightInfo[index].Enabled = true;
		m_LightInfo[index].Position = Vec4(position, static_cast<float>(type));    // w=0 dir, (0,0,-1,0) 
		m_LightInfo[index].Ambient = ambient;
		m_LightInfo[index].Diffuse = diffuse;
		m_LightInfo[index].Specular = specular;
		m_LightInfo[index].SpotCutoff = spotCutoff;
		m_LightInfo[index].SpotDirection = direction;
		m_LightInfo[index].SpotDecay = spotDecay;
		m_LightInfo[index].Attenuation = att;
	}

	void LightBase::SetAttenuation(float a, float b, float c)
	{
		m_LightInfo[m_Slot].Attenuation = glm::vec3(a, b, c);
		RequestUpdate();
	}

	void LightBase::SetLightDirection(const Vec3 & direction)
	{
		m_LightInfo[m_Slot].SpotDirection = direction;
		RequestUpdate();
	}

	void LightBase::SetLightDecay(float decay)
	{
		m_LightInfo[m_Slot].SpotDecay = decay;
		RequestUpdate();
	}

	void LightBase::SetSpotConeSize(float deg)
	{
		m_LightInfo[m_Slot].SpotCutoff = glm::radians(deg);
		RequestUpdate();
	}

}
