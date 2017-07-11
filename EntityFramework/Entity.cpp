#include "Entity.h"


namespace KLM_FRAMEWORK
{

	Entity::Entity(const std::string& ID) :
		m_ID{ ID },
		m_pParent{ nullptr }
	{
	}


	Entity::~Entity()
	{
	}

	void Entity::Delete()
	{
		//If I have parent - unregister me from it.
		if (m_pParent)
		{
			m_pParent->RemoveFromChildreen(this);	
		}
		// Set me as "to be deleted"
		m_DeleteMeFlag = true;
		// Run this method on all my childreen
		DeleteAllChildreen();
	}

	void Entity::AddChild(Entity* child)
	{
		if (child->GetParent())
		{
			child->GetParent()->RemoveFromChildreen(child);
		}

		child->SetParent(this);
		m_pChildren.Add(child);
	}

	void Entity::RemoveFromChildreen(Entity* child)
	{
		if (m_pChildren.Contains(child))
		{
			child->SetParent(nullptr);
			m_pChildren.Remove(child);
		}
	}


	void Entity::DeleteAllChildreen()
	{
		for (int i = 0; i < m_pChildren.Count(); ++i)
		{
			//m_pChildren[i]->SetParent(nullptr);
			m_pChildren[i]->Delete();
		}
	}

	Entity* Entity::FindInChildreen(const std::string & name)
	{
		for (int i = 0; i < m_pChildren.Count(); ++i)
		{
			Entity* child = m_pChildren[i];
			if (child->ShouldDelete()) continue;

			if (child->GetName() == name) return (child);
			Entity* foundInChildreen = child->FindInChildreen(name);
			if (foundInChildreen) return foundInChildreen;
		}

		return nullptr;
	}


	void Entity::AddComponent(ComponentUnique component)
	{
		component.get()->SetParent(this);

		m_pComponents.StdVector().push_back(std::move(component));
	}

	Component* Entity::GetComponentFirst(ComponentType const type)
	{
		for (int i = 0; i < m_pComponents.Count(); ++i)
		{
			if (m_pComponents[i]->GetType() == type) return m_pComponents[i].get();
		}

		return nullptr;
	}

	void Entity::CalculateTransform()
	{
		//WORLD TRANSFORM CALCULATION
		m_Transform.SetWorld(m_Transform.GetParentTransformStack() *
			glm::translate(m_Identity, m_Transform.GetPosition()));

		const Vec3& rotation = m_Transform.GetRotation();
		m_Transform.SetWorld(glm::rotate(m_Transform.GetWorldMat(), rotation.x, glm::vec3(1, 0, 0)));
		m_Transform.SetWorld(glm::rotate(m_Transform.GetWorldMat(), rotation.y, glm::vec3(0, 1, 0)));
		m_Transform.SetWorld(glm::rotate(m_Transform.GetWorldMat(), rotation.z, glm::vec3(0, 0, 1)));
		m_Transform.SetWorld(glm::scale(m_Transform.GetWorldMat(), m_Transform.GetScale()));

		for (int i = 0; i < m_pChildren.Count(); ++i)
		{
			m_pChildren[i]->GetTransform()->SetParentTransformStack(m_Transform.GetWorldMat());
		}

	}
}