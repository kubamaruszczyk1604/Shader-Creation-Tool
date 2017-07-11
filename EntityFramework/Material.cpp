#include "Material.h"
#include "Renderer.h"
#include "D3D11Shader.h"

namespace KLM_FRAMEWORK
{
	Material::Material(const Texture * const diffuseMap,
		const Texture * const specularMap,
		const Texture * const normalMap,
		const Colour & diffuseReflectivity,
		const Colour & specularReflectivity,
		const Colour & ambientReflectivity,
		Shader * const vertexShader,
		Shader * const fragmentShader) :

		p_DiffMap(diffuseMap),
		p_SpecMap(specularMap),
		p_NormMap(normalMap),
		m_DiffuseRefl(diffuseReflectivity),
		m_SpecularRefl(specularReflectivity),
		m_AmbientRefl(ambientReflectivity),
		m_pVertexShader(std::move(vertexShader)),
		m_PFragmentShader(std::move(fragmentShader)),
		m_MaterialOk(true)
	{
	}

	Material::~Material()
	{
	}

	void Material::SetCurrentShaders()
	{
		if (Renderer::GetAPI() == GfxAPI::D3D11)
		{

			if (m_PFragmentShader && m_pVertexShader)
			{
				
				D3D11Shader* vertexShader = static_cast<D3D11Shader*>(m_pVertexShader);
				D3D11Shader* fragmentShader = static_cast<D3D11Shader*>(m_PFragmentShader);
			   
				vertexShader->SetAsCurrent();
			    fragmentShader->SetAsCurrent();
			}
		}

		
	}


}