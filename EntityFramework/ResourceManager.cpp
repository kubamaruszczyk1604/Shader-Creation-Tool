#include "ResourceManager.h"
#include "Renderer.h"
#include "D3D11VertexShader.h"
#include "D3DFragmentShader.h"

namespace KLM_FRAMEWORK
{
	std::unordered_map<std::string, Texture*> ResourceManager::m_TexturesMap;
	std::unordered_map<std::string, Mesh*> ResourceManager::m_MeshesMap;
	std::unordered_map<std::string, Material*> ResourceManager::m_MaterialsMap;
	std::unordered_map<std::string, Shader*> ResourceManager::m_ShadersMap;




	Mesh * ResourceManager::LoadMesh(const std::string & path)
	{
		return nullptr;
	}

	Texture * ResourceManager::LoadTexture(const std::string & path, TextureAddressMode wrapMode)
	{
		if (path == "")
		{
			PRINTL("TEXTURE PATH EMPTY.. -> IGNORING");
			return nullptr;
		}

		Texture* output = nullptr;
		if (!QueryMap<std::string, Texture*>(path, output, m_TexturesMap))
		{
			output = new Texture(path, wrapMode);
			if (output)
			{
				if (!output->IsCreated())
				{
					std::cout << "FAILED TO CREATE TEXTURE: " << path << std::endl;
				}
				else
				{
					std::cout << "CREATED NEW TEXTURE: " << path << std::endl;
				}
				m_TexturesMap.insert(std::pair<std::string, Texture*>(path, output));
				
			}
			else
			{
				std::cout << "FAILED TO INSTANTIATE TEXTURE: " << path << std::endl;
			}
		}
		return output;
	}

	void ResourceManager::ReleaseAllResources()
	{
		for (auto it = m_MeshesMap.begin(); it != m_MeshesMap.end(); ++it)
		{
			if (it->second != nullptr)
				delete it->second;
		}
		m_MeshesMap.clear();
		for (auto it = m_TexturesMap.begin(); it != m_TexturesMap.end(); ++it)
		{
			if (it->second != nullptr)
				delete it->second;
		}
		m_TexturesMap.clear();
		for (auto it = m_MaterialsMap.begin(); it != m_MaterialsMap.end(); ++it)
		{
			if (it->second != nullptr)
				delete it->second;
		}
		m_MaterialsMap.clear();
		for (auto it = m_ShadersMap.begin(); it != m_ShadersMap.end(); ++it)
		{
			if (it->second != nullptr)
				delete it->second;
		}
		m_ShadersMap.clear();
		std::cout << "Resource Manager -> RELEASED RESOURCESS" << std::endl;
	}


	Material * ResourceManager::CreateMaterial(const MaterialDescription & desc, const std::string& name)
	{
		Material* output = nullptr;
		//cout << "ASKING FOR MATERIAL " << name << " Shaderfile " << desc.ShaderFile << endl;
		if (!QueryMap<std::string, Material*>(name, output, m_MaterialsMap))
		{
			std::cout << "REQUESTED MATERIAL NOT FOUND ->  CREATING NEW MATERIAL : " << name << std::endl;

			Texture* diffTex = ResourceManager::LoadTexture(desc.DiffuseMap, desc.DiffuseWrapMode);
			Texture* specTex = ResourceManager::LoadTexture(desc.SpecularMap, desc.SpecularWrapMode);
			Texture* normTex = ResourceManager::LoadTexture(desc.NormalMap, desc.NormalWrapMode);


			Shader* m_VS_TempPtr = 
				ResourceManager::CreateShader(ShaderType::VERTEX, desc.DXShaderFile, desc.VertexShader);
			Shader* m_PS_TempPtr =  
				ResourceManager::CreateShader(ShaderType::FRAGMENT, desc.DXShaderFile, desc.FragmentShader);

			//if (!m_VS_TempPtr->Compile()) return nullptr;
			//if (!m_PS_TempPtr->Compile()) return nullptr;

			if (m_VS_TempPtr == nullptr) return nullptr;
			if (m_PS_TempPtr == nullptr) return nullptr;

			output = new Material(diffTex, specTex, normTex,
				desc.DiffuseReflectivity, desc.SpecularReflectivity, desc.AmbientCol,
				m_VS_TempPtr, m_PS_TempPtr);

			m_MaterialsMap.insert(std::pair<std::string, Material*>(name, output));
		}
		return output;
	}

	Material * ResourceManager::FindMaterial(const std::string & name)
	{
		Material* output = nullptr;
		if (QueryMap<std::string, Material*>(name, output, m_MaterialsMap));
		return output;

		return nullptr;
	}

	Shader * ResourceManager::CreateShader(const ShaderType & type, const std::string & file, const std::string & functionName)
	{
		Shader* output = nullptr;
		if (!QueryMap<std::string, Shader*>(file + "|" + functionName, output, m_ShadersMap))
		{
			std::cout << "REQUESTER SHADER NOT FOUND -> CREATING NEW SHADER  file:" << file << " function name: " << functionName << std::endl;
			
			if (Renderer::GetAPI() == GfxAPI::D3D11)
			{
				if (type == ShaderType::VERTEX)
				{
					D3D11Shader* shader = new D3D11VertexShader(file, functionName);
					if (shader == nullptr) return nullptr;
					if (!shader->Compile())
					{
						delete shader;
						return nullptr;
					}
					output = shader;						
				}

				else if (type == ShaderType::FRAGMENT)
				{
					D3D11Shader* shader = new D3D11FragmentShader(file, functionName);
					if (shader == nullptr) return nullptr;
					if (!shader->Compile())
					{
						delete shader;
						return nullptr;
					}
					output = shader;
				}
				m_ShadersMap.insert(std::pair<std::string, Shader*>(file + "|" + functionName, output));
			}
		}
		return output;
	}

}