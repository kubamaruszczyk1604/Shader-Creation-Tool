#pragma once
#include "Component.h"
namespace KLM_FRAMEWORK
{
	class BehaviourManager;

	class BehaviourComponent :public Component
	{
		friend BehaviourManager;
	private:
		bool m_StartCalled;

	public:
		BehaviourComponent() = delete;
		BehaviourComponent(const BehaviourComponent&) = delete;
		BehaviourComponent& operator=(const BehaviourComponent&) = delete;

		explicit BehaviourComponent(const std::string& ID);
		virtual ~BehaviourComponent();

		virtual void OnStart() = 0;
		virtual void Update(float deltaTime, float totalTime) = 0;;
		virtual void OnExit() = 0;
	};

}