#pragma once
#include "Scene.h"

namespace KLM_FRAMEWORK
{

	class SceneManager
	{

	public:
		SceneManager() = delete;
		SceneManager(const SceneManager&) = delete;
		SceneManager& operator=(const SceneManager&) = delete;

	private:
		static SceneUniquePtr m_upCurrentScene;

	public:
		//InputCallbacks
		static void OnKeyPressed(const int key, const KeyState state);
		static void OnMouseMove(const int x, const int y);
		static void OnMouseButtonUp(const MouseButton button);
		static void OnMouseButtonDown(const MouseButton button);
		static void Update(const float deltaTime, const float totalTime = 0);

	public:
		static void Initialize(const int width, const int height, const HWND handle);
		static void Load(Scene* scene);
		static void ShutDown();

	};
}