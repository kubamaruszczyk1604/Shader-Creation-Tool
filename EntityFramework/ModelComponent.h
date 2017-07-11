#pragma once
#include "Component.h"
#include "Mesh.h"
#include "Material.h"

namespace KLM_FRAMEWORK
{
	class ModelComponent : public Component
	{
	private:
		Mesh* p_Mesh;
		Material* p_Material;

	public:
		explicit ModelComponent(const std::string& ID, Mesh* const mesh, Material*const material);
		~ModelComponent();

		ModelComponent(const ModelComponent&) = default;

		void SetMesh(Mesh* const mesh) { p_Mesh = mesh; };
		void SetMaterial(Material* const material) { p_Material = material; };
		void SetMeshAndMaterial(Mesh* const mesh, Material*const material) { p_Mesh = mesh; p_Material = material; }

		Mesh * GetMesh() { return p_Mesh; };
		Material* GetMaterial() { return p_Material; };

	};

}