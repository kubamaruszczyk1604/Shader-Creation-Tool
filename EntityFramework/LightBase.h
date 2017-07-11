#pragma once
#include "LightStruct.h"


namespace KLM_FRAMEWORK
{
	enum class LightType
	{
		Directional = 0,
		Point = 1,
		Spot = 2
	};

	class LightBase
	{
	public:
		static const unsigned MAX_LIGHTS;// = 20;
	private:
		unsigned m_Slot;
		static ShaderLightInfoStruct m_LightInfo[];
		static bool UpdateRequest;
		static std::vector<int> s_AvailableSlotsIndices;

	protected: // Only child classes should acces it. 

		static void RequestUpdate();
		explicit LightBase();

		unsigned Slot()const { return m_Slot; }
		int GetSlot() { return m_Slot; }

		//static LightInfo& GetLightInfo(unsigned index)  { m_LightInfo[index]; }
		static void SetLightInfo(int index,
			LightType type,
			const Vec3& position,
			const Colour& ambient,
			const Colour& diffuse,
			const Colour& specular,
			float spotCutoff,
			const Vec3& direction,
			float spotDecay,
			const Vec3& att
			);

		void SetAttenuation(float a, float b, float c);
		void SetLightDirection(const Vec3& direction);
		void SetLightDecay(float decay);
		void SetSpotConeSize(float deg);

	public:
		static void InitializeLightSystem();
		static bool IsRequestingUpdate();
		static const ShaderLightInfoStruct* const GetLightInfo() { return m_LightInfo; }
		static void UpdateFinished();

		LightBase(const LightBase&) = delete;
		LightBase &operator=(const LightBase&) = delete;
		virtual ~LightBase();

		Vec3 GetPosition()const { return m_LightInfo[m_Slot].Position; }
		Colour GetAmbient()const { return m_LightInfo[m_Slot].Ambient; }
		Colour GetDiffuse()const { return m_LightInfo[m_Slot].Diffuse; }
		Colour GetSpecular()const { return m_LightInfo[m_Slot].Specular; }

		void SetPosition(const Vec3& position, int type);
		void UpdatePosition(float x, float y, float z);
		void UpdatePosition(const Vec3& xyz);
		void SetAmbient(const Colour& ambient);

		void SetDiffuse(const Colour& diffuse);
		void SetSpecular(const Colour& specular);
		void Enable(bool on) { m_LightInfo[m_Slot].Enabled = on; }
		bool isEnabled()const { return m_LightInfo[m_Slot].Enabled; }

	};

}