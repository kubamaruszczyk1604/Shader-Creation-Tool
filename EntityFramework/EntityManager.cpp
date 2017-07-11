#include "EntityManager.h"


namespace KLM_FRAMEWORK
{
	//#include "BehaviourManager.h"



	EntityManager::EntityManager()
	{
	}

	EntityManager::~EntityManager()
	{
	}

	void EntityManager::AddEntity(EntityUnique entity)
	{
		m_pEntities.push_back(std::move(entity));
	}

	void EntityManager::RemoveEntity(const std::string& name)
	{
		for (int i = 0; i < m_pEntities.size(); ++i)
		{
			if (m_pEntities[i]->GetName() == name)
			{
				Entity* e = m_pEntities[i].get();
				e->Delete();
			}
		}
	}

	Entity * EntityManager::FindEntity(const std::string & name)
	{
		for (int i = 0; i < m_pEntities.size(); ++i)
		{
			if (m_pEntities[i]->ShouldDelete())continue;
			if (m_pEntities[i]->GetName() == name)
			{
				
				return m_pEntities[i].get();
			}
		}
		return nullptr;
	}

	void EntityManager::Update(float const deltaTime, float const totalTime)
	{

		for (int i = 0; i < m_pEntities.size(); ++i)
		{
			m_pEntities[i]->CalculateTransform();
		}
		

		for (int i = 0; i < m_pEntities.size(); ++i)
		{
		
			Entity* e = m_pEntities[i].get();
			if (e->ShouldDelete())
			{
				m_pEntities[i].reset(nullptr);
				RemoveFromVecByValue(m_pEntities, m_pEntities[i]);
			}
		}


		//PRINTL("UPDATING X FORMS..");

	}

	void EntityManager::RemoveAllEntities()
	{
		m_pEntities.clear();
	}
}