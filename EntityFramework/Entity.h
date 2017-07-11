#pragma once
#include "StdIncludes.h"
#include "Component.h"
#include "Transform.h"

namespace KLM_FRAMEWORK
{
	class Entity
	{

	private:
		KLMList<Entity*> m_pChildren;
		KLMList<ComponentUnique> m_pComponents;
		std::string m_ID;
		Entity* m_pParent;
		Transform m_Transform;

		Entity() = default;
		Entity(const Entity&) = delete;
		Entity& operator=(const Entity&) = delete;
		Mat4 m_Identity;
		bool m_DeleteMeFlag{ false };

	public:
		explicit Entity(const std::string& ID);
		virtual ~Entity();

		void     Delete();
		bool     ShouldDelete() { return m_DeleteMeFlag; }
		void     AddChild(Entity* child);
		void     RemoveFromChildreen(Entity* child);
		void     DeleteAllChildreen();
		Entity*  FindInChildreen(const std::string& name);
		void     AddComponent(ComponentUnique component);
		void     SetParent(Entity* const entity) { m_pParent = entity; }
		Entity*  GetParent() const { return m_pParent; }
		const std::string& GetName() const { return m_ID; }

		Component* GetComponentFirst(ComponentType type);
	    int        GetComponentCount() const { return m_pComponents.Count(); }
		Component* GetComponent(int const index) { m_pComponents[index]; }
		Transform* GetTransform() { return &m_Transform; }
		void       CalculateTransform();

	};


}
