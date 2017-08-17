#pragma once

#include "Material.h"
#include "Mesh.h"
#include "D3DTexture.h"
#include "Shader.h"
#include "StdIncludes.h"
#include <unordered_map>

namespace KLM_FRAMEWORK
{
	class ResourceManager
	{
	
	public:
		static Mesh* LoadMesh(const std::string& path);
		static Texture* LoadTexture(const std::string& path, TextureAddressMode wrapMode);

		static void ReleaseAllResources();

		static Material* CreateMaterial(const MaterialDescription& desc, const std::string& name);
		static Material* FindMaterial(const std::string& name);

		static Shader* CreateShader(const ShaderType& type, const std::string& file, const std::string& functionName);
		static std::string GetLastCompileMessage() { return s_LastCompileLogMessage; }
		static void ClearCompilerMessages() { s_LastCompileLogMessage = ""; }

	private:
		static std::unordered_map<std::string, Shader*> m_ShadersMap;
		static std::unordered_map<std::string, Texture*> m_TexturesMap;
		static std::unordered_map<std::string, Mesh*> m_MeshesMap;
		static std::unordered_map<std::string, Material*> m_MaterialsMap;
		static std::string s_LastCompileLogMessage;
	};


}