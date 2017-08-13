#pragma once
#include "StdIncludes.h"
#include "Shader.h"
#include "Texture.h"
#include "GLShaderProgram.h"


namespace KLM_FRAMEWORK
{

	struct MaterialDescription
	{
		std::string DiffuseMap = "";
		std::string SpecularMap = "";
		std::string NormalMap = "";
		std::string HeightMap = "";

		Colour AmbientCol;
		Colour DiffuseReflectivity;
		Colour SpecularReflectivity;

		TextureAddressMode DiffuseWrapMode = TextureAddressMode::WRAP;
		TextureAddressMode SpecularWrapMode = TextureAddressMode::WRAP;
		TextureAddressMode NormalWrapMode = TextureAddressMode::WRAP;

		//if left empty - GL Shader will be assumed.
		std::string DXShaderFile = "";
		std::string VertexShader = "";
		std::string FragmentShader = "";

	};




	//class ResourceManager;
	class Material
	{
	private:
		const Texture* p_DiffMap;
		const Texture* p_SpecMap;
		const Texture* p_NormMap;

		Colour m_DiffuseRefl;
		Colour m_SpecularRefl;
		Colour m_AmbientRefl;


		Shader* m_pVertexShader;
		Shader* m_PFragmentShader;

		Shader* m_pAdditionalPassShaderVS;
		Shader* m_pAdditionalPassShaderPS;

		GLShaderProgram* m_pGLShaderProgram{ nullptr };
		bool m_MaterialOk;

		
	public:
		Material(const Texture* const diffuseMap,
			const Texture* const specularMap,
			const Texture* const normalMap,
			const Colour& diffuseReflectivity,
			const Colour& specularReflectivity,
			const Colour& ambientReflectivity,
			Shader* const vertexShader,
			Shader* const fragmentShader);
		virtual ~Material();

		//TO prevent usage of assigment opp. and copy constructor
		Material& operator=(const Material& other) = delete;
		Material(const Material& other) = delete;




	public:
		const Texture* GetDiffuseMap() const { return p_DiffMap; }
		const Texture* GetSpecularMap()const { return p_SpecMap; }
		const Texture* GetNormalMap()  const { return p_NormMap; }
		const  Colour* GetDiffuseColPtr() const { return &m_DiffuseRefl; }
		const  Colour* GetSpecularColPtr() const { return &m_SpecularRefl; }
		const  Colour* GetAmbientColPtr() const { return &m_AmbientRefl; }


		void SetDiffuseReflectivity(const Colour& col) { m_DiffuseRefl = col; }
		void SetSpecularReflectivity(const Colour& col) { m_SpecularRefl = col; }
		void SetAmbientReflectivity(const Colour& col) { m_AmbientRefl = col; }

		void SetDiffuseReflectivity(const float& r, const float& g, const float& b, const float& a) { m_DiffuseRefl = Vec4(r, g, b, a); }
		void SetSpecularReflectivity(const float& r, const float& g, const float& b, const float& a) { m_SpecularRefl = Vec4(r, g, b, a); }
		void SetAmbientReflectivity(const float& r, const float& g, const float& b, const float& a) { m_AmbientRefl = Vec4(r, g, b, a); }

		void SetDiffuseReflectivity(const float& r, const float& g, const float& b) { m_DiffuseRefl = Vec4(r, g, b, 1); }
		void SetSpecularReflectivity(const float& r, const float& g, const float& b) { m_SpecularRefl = Vec4(r, g, b, 1); }
		void SetAmbientReflectivity(const float& r, const float& g, const float& b) { m_AmbientRefl = Vec4(r, g, b, 1); }

		bool MaterialOk()const { return m_MaterialOk; }

		void SetCurrentShaders();

		GLuint GetShaderProgID() { return m_pGLShaderProgram->GetID(); }

	};


}
