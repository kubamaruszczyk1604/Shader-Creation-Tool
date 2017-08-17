#include "ResourceManagerWrapper.h"
#include <msclr\marshal_cppstd.h>


namespace KLM_FRAMEWORK
{
	using namespace msclr::interop;
	ResourceManagerWrapper::~ResourceManagerWrapper()
	{
	}

	System::IntPtr ResourceManagerWrapper::LoadMesh(System::String ^ path)
	{
		
		Mesh* p = ResourceManager::LoadMesh(marshal_as<std::string>(path));
		return static_cast<System::IntPtr>(p);
	}

	System::IntPtr ResourceManagerWrapper::LoadTexture(System::String ^ path)
	{
		Texture * p = ResourceManager::LoadTexture(marshal_as<std::string>(path),TextureAddressMode::WRAP);
		return static_cast<System::IntPtr>(p);
	}

	void ResourceManagerWrapper::ReleaseAllResources()
	{
		ResourceManager::ReleaseAllResources();
	}

	System::IntPtr ResourceManagerWrapper::FindMaterial(System::String ^ path)
	{
		Material* p = ResourceManager::FindMaterial(marshal_as<std::string>(path));
		return static_cast<System::IntPtr>(p);
	}

	System::IntPtr ResourceManagerWrapper::CreateShader(const ShaderType & type, System::String ^ file, System::String ^ functionName)
	{
		Shader* p = ResourceManager::CreateShader(type, marshal_as<std::string>(file), marshal_as<std::string>(functionName));
		return static_cast<System::IntPtr>(p);
	}

	System::String ^ ResourceManagerWrapper::GetLastCompileMessage()
	{
		return gcnew System::String(ResourceManager::GetLastCompileMessage().c_str());
	}

	void ResourceManagerWrapper::ClearCompilerMessages()
	{
		ResourceManager::ClearCompilerMessages();
	}

}