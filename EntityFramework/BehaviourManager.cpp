#include "BehaviourManager.h"
#include "Entity.h"

namespace KLM_FRAMEWORK
{


	void BehaviourManager::Update(ListOfEntities* entityList, const float deltaTime, const float totalTime)
	{
		for (int i = 0; i < entityList->size(); ++i)
		{
			Entity* e = (*entityList)[i].get();

			Component* c = e->GetComponentFirst(ComponentType::BEHAVIOUR_COMPONENT);
			if (c)
			{
				BehaviourComponent* bc = static_cast<BehaviourComponent*>(c);
				if (!bc->m_StartCalled) 
				{
					bc->OnStart();
					bc->m_StartCalled = true;
				}
				bc->Update(deltaTime, totalTime);
			}
		}
	}

	void BehaviourManager::TerminateAllBehaviours(ListOfEntities* entityList)
	{
		for (int i = 0; i < entityList->size(); ++i)
		{
			Entity* e = (*entityList)[i].get();

			Component* c = e->GetComponentFirst(ComponentType::BEHAVIOUR_COMPONENT);
			if (c)
			{
				BehaviourComponent* bc = static_cast<BehaviourComponent*>(c);
				bc->OnExit();
			}
		}
	}
}