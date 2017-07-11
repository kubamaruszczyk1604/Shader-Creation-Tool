#include "InputSystem.h"

namespace KLM_FRAMEWORK
{
	Vec2i InputSystem::s_MousePos{ Vec2i{ 0,0 } };
	Vec2i InputSystem::s_PreviousMousePos{ Vec2i{ 0,0 } };

	KeyboardCallback InputSystem::s_KeyboardCallback{ nullptr };
	MouseMoveCallback InputSystem::s_MouseMoveCallback{ nullptr };;
	MouseButtonUpCallback InputSystem::s_MouseButtonUpCallback{ nullptr };
	MouseButtonDownCallback InputSystem::s_MouseButtonDownCallback{ nullptr };



	void InputSystem::PropagateOnKeyDown(const int key)
	{
		if (s_KeyboardCallback) s_KeyboardCallback(key, KeyState::PRESSED);
	}

	void InputSystem::PropagateOnKeyUp(const int key)
	{
		if (s_KeyboardCallback) s_KeyboardCallback(key, KeyState::RELEASED);
	}

	void InputSystem::PropagateOnMouseMove(const int mouseX, const int mouseY)
	{
		s_MousePos = Vec2i(mouseX, mouseY);
		if (s_MouseMoveCallback) s_MouseMoveCallback(mouseX, mouseY);
	}

	void InputSystem::PropagateOnMouseUp(const int button)
	{
		if (s_MouseButtonUpCallback) s_MouseButtonUpCallback(static_cast<MouseButton>(button));
	}

	void InputSystem::PropagateOnMouseDown(const int button)
	{
		if (s_MouseButtonDownCallback) s_MouseButtonDownCallback(static_cast<MouseButton>(button));
	}
}