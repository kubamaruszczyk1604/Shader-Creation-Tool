#pragma once
#include "StdIncludes.h"
#include "InputSystem.h"
#include "EntityManager.h"
#include "ResourceManager.h"

namespace KLM_FRAMEWORK
{
	class Scene
	{

	private:
		EntityManager* m_pEntityManager;

	protected:
		void AddEntity(Entity* entity);
		void RemoveEntity(const std::string& ID);
		Entity* FindEntity(const std::string& ID);

	public:
		Scene();
		Scene(const Scene&) = delete;
		Scene& operator=(const Scene&) = delete;

		virtual ~Scene();
		virtual void OnStart() = 0;
		virtual void Update(float deltaTime, float totalTime = 0) = 0;
		virtual void OnExit() = 0;

		virtual void PostUpdate() {}

		//InputCallbacks
		virtual void OnKeyPressed(const int key, const KeyState state);
		virtual void OnMouseMove(const int x, const int y);
		virtual void OnMouseButtonUp(MouseButton const button);
		virtual void OnMouseButtonDown(MouseButton const button);

		EntityManager* GetEntityManager() { return m_pEntityManager; }
	};

	using SceneUniquePtr = std::unique_ptr<Scene>;
}