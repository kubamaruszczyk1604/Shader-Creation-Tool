#pragma once
#include "StdIncludes.h"
#include "CrossLanguageTypes.cs"


public ref class ShaderVectorVariable
{

private:
	int m_Size;
	array<float>^ m_Data;
	ShaderVariableType m_Type;
	System::String^ m_Name;

public:
	ShaderVectorVariable(ShaderVectorVariable^ data, System::String^ name);
	ShaderVectorVariable(ShaderVariableType type, System::String^ name);
	ShaderVectorVariable(float x, float y, float z, float w, System::String^ name);
	ShaderVectorVariable(float x, float y, float z, System::String^ name);
	ShaderVectorVariable(float x, float y, System::String^ name);


	ShaderVectorVariable(float x, System::String^ name);


	int GetSize()
	{
		return m_Size;
	}

	const System::String^ const GetName()
	{
		return m_Name;
	}

	void SetName(System::String^ name)
	{
		m_Name = name;
	}

	ShaderVariableType GetType()
	{
		return m_Type;
	}

	float GetElement(unsigned i)
	{
	    return m_Data[i];
	}

	void Set(float x, float y, float z, float w);
	void Set(float x, float y, float z);
	void Set(float x, float y);
	void Set(float x);

	void SetX(float x) { m_Data[0] = x; }
	void SetY(float y) { m_Data[1] = y; }
	void SetZ(float z) { m_Data[2] = z; }
	void SetW(float w) { m_Data[3] = w; }

	void SetR(float r) { m_Data[0] = r; }
	void SetG(float g) { m_Data[1] = g; }
	void SetB(float b) { m_Data[2] = b; }
	void SetA(float a) { m_Data[3] = a; }

	float GetX() { return m_Data[0];}
	float GetY() { return m_Data[1]; }
	float GetZ() { return m_Data[2]; }
	float GetW() { return m_Data[3]; }

	float GetR() { return m_Data[0]; }
	float GetG() { return m_Data[1]; }
	float GetB() { return m_Data[2]; }
	float GetA() { return m_Data[3]; }

};

class DataConverter
{
public:
	static KLM_FRAMEWORK::Colour ToColur(ShaderVectorVariable^ floatArray)
	{
		return KLM_FRAMEWORK::Colour(floatArray->GetR(), floatArray->GetG(),
			floatArray->GetB(), floatArray->GetA());
	}

	static KLM_FRAMEWORK::Vec4 ToVec4(ShaderVectorVariable^ floatArray)
	{
		return KLM_FRAMEWORK::Vec4(floatArray->GetX(), floatArray->GetY(),
			floatArray->GetZ(), floatArray->GetW());
	}
	static KLM_FRAMEWORK::Vec3 ToVec3(ShaderVectorVariable^ floatArray)
	{
		return KLM_FRAMEWORK::Vec3(floatArray->GetX(), floatArray->GetY(),
			floatArray->GetZ());
	}
	static KLM_FRAMEWORK::Vec2 ToVec2(ShaderVectorVariable^ floatArray)
	{
		return KLM_FRAMEWORK::Vec2(floatArray->GetX(), floatArray->GetY());
	}

	static float ToFloat(ShaderVectorVariable^ floatArray)
	{
		return floatArray->GetX();
	}

	static KLM_FRAMEWORK::Vec4i ToVec4i(ShaderVectorVariable^ floatArray)
	{
		return KLM_FRAMEWORK::Vec4i(floatArray->GetX(), floatArray->GetY(),
			floatArray->GetZ(), floatArray->GetW());
	}
	static KLM_FRAMEWORK::Vec3i ToVec3i(ShaderVectorVariable^ floatArray)
	{
		return KLM_FRAMEWORK::Vec3i(floatArray->GetX(), floatArray->GetY(),
			floatArray->GetZ());
	}
	static KLM_FRAMEWORK::Vec2i ToVec2i(ShaderVectorVariable^ floatArray)
	{
		return KLM_FRAMEWORK::Vec2i(floatArray->GetX(), floatArray->GetY());
	}

	static int ToInteger(ShaderVectorVariable^ floatArray)
	{
		return floatArray->GetX();
	}

};