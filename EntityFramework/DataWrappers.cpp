#include "DataWrappers.h"
#include "ResourceManagerWrapper.h"

ShaderVectorVariable::ShaderVectorVariable(ShaderVectorVariable ^ data, System::String ^ name)
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

ShaderVectorVariable::ShaderVectorVariable(ShaderVariableType type, System::String ^ name)

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

ShaderVectorVariable::ShaderVectorVariable(float x, float y, float z, float w, System::String ^ name)

{
	m_Name = name;
	m_Type = ShaderVariableType::Vector4;
	m_Size = 4;
	m_Data = gcnew array<float>(m_Size);
	m_Data[0] = x; m_Data[1] = y; m_Data[2] = z; m_Data[3] = w;
}

ShaderVectorVariable::ShaderVectorVariable(float x, float y, float z, System::String ^ name)
{
	m_Name = name;
	m_Type = ShaderVariableType::Vector3;
	m_Size = 3;
	m_Data = gcnew array<float>(m_Size);
	m_Data[0] = x; m_Data[1] = y; m_Data[2] = z;
}

ShaderVectorVariable::ShaderVectorVariable(float x, float y, System::String ^ name)
{
	m_Name = name;
	m_Type = ShaderVariableType::Vector2;
	m_Size = 2;
	m_Data = gcnew array<float>(m_Size);
	m_Data[0] = x; m_Data[1] = y;
}

ShaderVectorVariable::ShaderVectorVariable(float x, System::String ^ name)
{
	m_Name = name;
	m_Type = ShaderVariableType::Single;
	m_Size = 1;
	m_Data = gcnew array<float>(m_Size);
	m_Data[0] = x;
}

void ShaderVectorVariable::Set(float x, float y, float z, float w)
{
	 m_Data[0] = x; 
	 m_Data[1] = y; 
	 m_Data[2] = z; 
	 m_Data[3] = w; 
}

void ShaderVectorVariable::Set(float x, float y, float z)
{
	m_Data[0] = x;
	m_Data[1] = y;
	m_Data[2] = z;
}

void ShaderVectorVariable::Set(float x, float y)
{
	m_Data[0] = x;
	m_Data[1] = y;
}

void ShaderVectorVariable::Set(float x)
{
	m_Data[0] = x;
}




/////////////////////////////////////////  SHADER TEXXTURE VARIABLE  //////////////////////////////////////////
ShaderTextureVariable::ShaderTextureVariable(System::String ^ path, System::String ^ name) :
	m_Type{ ShaderVariableType::Texture2D },
	m_Path{ path },
	m_Name{ name }
{
	m_pTexture = ResourceManagerWrapper::LoadTexture(path);
}

void ShaderTextureVariable::SetPath(System::String ^ path)
{
	m_Path = path;
	m_pTexture = ResourceManagerWrapper::LoadTexture(path);
}
