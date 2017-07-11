#pragma once

#include "StdIncludes.h"
namespace KLM_FRAMEWORK
{
	enum class KeyState { PRESSED, RELEASED };
	enum class MouseButton { LEFT, RIGHT };

	using KeyboardCallback = void(*)(const int key, const KeyState state);	//typedef void(*KeyboardCallback)(const int key, const KeyState state);
	using MouseMoveCallback = void(*)(const int x, const int y);			//typedef void(*MouseMoveCallback)(const int x, const int y);
	using MouseButtonUpCallback = void(*)(const MouseButton button);		//typedef void(*MouseButtonUpCallback)(const MouseButton button);
	using MouseButtonDownCallback = void(*)(const MouseButton button);		//typedef void(*MouseButtonDownCallback)(const MouseButton button);

	class InputSystem
	{
	public:
		InputSystem() = delete;
		InputSystem(const InputSystem&) = delete;
		InputSystem& operator=(const InputSystem&) = delete;

	private:
		static Vec2i s_MousePos;
		static Vec2i s_PreviousMousePos;
		static KeyboardCallback s_KeyboardCallback;
		static MouseMoveCallback s_MouseMoveCallback;
		static MouseButtonUpCallback s_MouseButtonUpCallback;
		static MouseButtonDownCallback s_MouseButtonDownCallback;

	public: // Called by window's msgproc
		static void PropagateOnKeyDown(const int key);
		static void PropagateOnKeyUp(const int key);
		static void PropagateOnMouseMove(const int mouseX, const int mouseY);
		static void PropagateOnMouseUp(const int button);
		static void PropagateOnMouseDown(const int button);

	public: //Called by client
		static void SetKeyboardCallback(KeyboardCallback callback) { s_KeyboardCallback = callback; }
		static void SetMouseMoveCallback(MouseMoveCallback callback) { s_MouseMoveCallback = callback; }
		static void SetMouseButtonCallback(MouseButtonUpCallback callbackUp, MouseButtonDownCallback callbackDown) { s_MouseButtonUpCallback = callbackUp; s_MouseButtonDownCallback = callbackDown; }
		static const Vec2i& GetMousePosition() { return s_MousePos; };
	};

}