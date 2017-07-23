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
	ShaderVectorVariable(ShaderVectorVariable^ data, System::String^ name)
	{
		m_Name = name;
		m_Type = data->GetType();  
		m_Size = data->GetSize();
		m_Data = gcnew array<float>(m_Size);

		for (int i = 0; i < m_Size; ++i)
		{
			m_Data[i] = data->GetElement(i);
		}
	}

	ShaderVectorVariable(ShaderVariableType type,System::String^ name)
	{
		m_Name = name;
		if (type == ShaderVariableType::Vector4)
		{ 
			m_Size = 4;
			m_Type = ShaderVariableType::Vector4;
		}
		else if (type == ShaderVariableType::Vector3)
		{
			m_Size = 3;
			m_Type = ShaderVariableType::Vector3;
		}
		else if (type == ShaderVariableType::Vector2)
		{
			m_Size = 2;
			m_Type = ShaderVariableType::Vector2;
		}
		else if (type == ShaderVariableType::Single)
		{
			m_Type = ShaderVariableType::Single;
			m_Size = 1;
		}

		m_Data = gcnew array<float>(m_Size);
		
	}


	ShaderVectorVariable(float x, float y, float z, float w, System::String^ name)
	{
		m_Name = name;
		m_Type = ShaderVariableType::Vector4;
		m_Size = 4;
		m_Data = gcnew array<float>(m_Size);
		m_Data[0] = x; m_Data[1] = y; m_Data[2] = z; m_Data[3] = w;
	}

	ShaderVectorVariable(float x, float y, float z, System::String^ name)
	{
		m_Name = name;
		m_Type = ShaderVariableType::Vector3;
		m_Size = 3;
		m_Data = gcnew array<float>(m_Size);
		m_Data[0] = x; m_Data[1] = y; m_Data[2] = z;	
	}

	ShaderVectorVariable(float x, float y, System::String^ name)
	{
		m_Name = name;
		m_Type = ShaderVariableType::Vector2;
		m_Size = 2;
		m_Data = gcnew array<float>(m_Size);
		m_Data[0] = x; m_Data[1] = y;
	}

	ShaderVectorVariable(float x, System::String^ name)
	{
		m_Name = name;
		m_Type = ShaderVariableType::Single;
		m_Size = 1;
		m_Data = gcnew array<float>(m_Size);
		m_Data[0] = x;
	}

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