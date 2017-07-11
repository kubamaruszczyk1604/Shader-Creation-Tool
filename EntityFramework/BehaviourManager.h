#pragma once
#include "BehaviourComponent.h"
#include "EntityTypes.h"

namespace KLM_FRAMEWORK
{
	class BehaviourManager
	{
	public:
		BehaviourManager() = delete;
		BehaviourManager(const BehaviourManager&) = delete;
		BehaviourManager& operator=(const BehaviourManager&) = delete;

		static void Update(ListOfEntities* entityList, const float deltaTime, const float totalTime);
		static void TerminateAllBehaviours(ListOfEntities* entityList);
	};
}
