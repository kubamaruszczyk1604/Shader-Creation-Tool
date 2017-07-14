#include "SceneManager.h"
#include "InputSystem.h"
#include "EntityManager.h"
#include "Renderer.h"
#include "BehaviourManager.h"
#include "GLRenderer.h"

namespace KLM_FRAMEWORK
{
	SceneUniquePtr SceneManager::m_upCurrentScene;

	void SceneManager::Initialize(const int width, const int height, const HWND handle)
	{
		InputSystem::SetKeyboardCallback(SceneManager::OnKeyPressed);
		InputSystem::SetMouseButtonCallback(SceneManager::OnMouseButtonUp, SceneManager::OnMouseButtonDown);
		InputSystem::SetMouseMoveCallback(SceneManager::OnMouseMove);
		Renderer::Initialize(width, height, handle,GfxAPI::D3D11);
	}

	void SceneManager::Load(Scene* scene)
	{

		if (m_upCurrentScene)
		{
			ListOfEntities* list = m_upCurrentScene->GetEntityManager()->GetListOfEntities();
			BehaviourManager::TerminateAllBehaviours(list);
			m_upCurrentScene.get()->OnExit();
		}
		m_upCurrentScene = std::unique_ptr<Scene>(scene);
		m_upCurrentScene->OnStart();

	}

	void SceneManager::Update(const float deltaTime, const float totalTime)
	{
	
		if (m_upCurrentScene)
		{
			
			m_upCurrentScene->Update(deltaTime, totalTime);
			m_upCurrentScene->GetEntityManager()->Update(deltaTime, totalTime);
			ListOfEntities* list = m_upCurrentScene->GetEntityManager()->GetListOfEntities();
			//TODO: SYSTEMS ACT ON ENTITIES HERE
			BehaviourManager::Update(list, deltaTime, totalTime);

			Renderer::ClearScreen(Colour(0.5, 0.2, 0, 1));
			Renderer::Render(list);
			Renderer::Update(deltaTime, totalTime);
			Renderer::SwapBuffers();
			m_upCurrentScene->PostUpdate();

		}
		
	}

	void SceneManager::ShutDown()
	{
		if (m_upCurrentScene)
		{
			ListOfEntities* list = m_upCurrentScene->GetEntityManager()->GetListOfEntities();
			BehaviourManager::TerminateAllBehaviours(list);
			m_upCurrentScene->OnExit();
		}
		Renderer::Terminate();
		ResourceManager::ReleaseAllResources();
	}

	void SceneManager::OnKeyPressed(const int key, const KeyState state)
	{
		if (m_upCurrentScene)
		{
			m_upCurrentScene->OnKeyPressed(key, state);
		}
	}

	void SceneManager::OnMouseMove(const int x, const int y)
	{
		if (m_upCurrentScene)
		{
			m_upCurrentScene->OnMouseMove(x, y);
		}
	}

	void SceneManager::OnMouseButtonUp(const MouseButton button)
	{
		if (m_upCurrentScene)
		{
			m_upCurrentScene->OnMouseButtonUp(button);
		}
	}

	void SceneManager::OnMouseButtonDown(const MouseButton button)
	{
		if (m_upCurrentScene)
		{
			m_upCurrentScene->OnMouseButtonDown(button);
		}
	}
}