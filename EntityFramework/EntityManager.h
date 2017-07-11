#pragma once
#include "Entity.h"
#include "EntityTypes.h"

namespace KLM_FRAMEWORK
{
	
	class EntityManager
	{

	private:
		std::vector<EntityUnique> m_pEntities;

	public:
		EntityManager();
		EntityManager(const EntityManager&) = delete;
		EntityManager& operator=(const EntityManager&) = delete;
		~EntityManager();

		void      AddEntity(EntityUnique entity);
		void      RemoveEntity(const std::string& id);
		Entity*   FindEntity(const std::string& id);
		void      Update(float deltaTime, float totalTime = 0);
		void      RemoveAllEntities();
		ListOfEntities* GetListOfEntities() { return &m_pEntities; }


	};
}
