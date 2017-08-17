#pragma once
#include "StdIncludes.h"
#include "ResourceManager.h"




namespace KLM_FRAMEWORK
{

	class ResourceManagerWrapper
	{
	public:
		ResourceManagerWrapper() = delete;
		~ResourceManagerWrapper();

		static System::IntPtr LoadMesh(System::String^ path);
		static System::IntPtr LoadTexture(System::String^ path);

		static void ReleaseAllResources();

		static System::IntPtr FindMaterial(System::String^ path);

		static System::IntPtr CreateShader(const ShaderType& type, System::String^ file, System::String^ functionName);
		static System::String^ GetLastCompileMessage();
		static void ClearCompilerMessages();

	};

}