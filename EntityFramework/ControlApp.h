#pragma once
#include "StdIncludes.h"
#include "KLM_Framework.h"
#include <Windows.h>
#include <msclr\marshal_cppstd.h>
#include "DataWrappers.h"

using WndUpdateLoopCallback = void(*)();
using WndMessageLoopCallback = void(*)(SUINT message, SUINT wParam, SUINT lParam);
using SceneReloadedCallback = void(*)();

class ControlApp
{
private:
	static bool s_ReloadRequest;
	static bool s_Running;
	static Stopwatch s_GlobalTimer;
	static Stopwatch s_FrameTimer;
	static float s_TimeScale;
	static WndUpdateLoopCallback s_UpdateLoopCallback;
	static WndMessageLoopCallback s_MessageLoopCallback;
	static SceneReloadedCallback s_SceneReloadedCallback;
	static Scene* s_CurrentScene;
	

public:
	ControlApp() = delete;
	ControlApp(const ControlApp&) = delete;
	ControlApp& operator=(const ControlApp&) = delete;
	static void Create(int width, int height, System::IntPtr handle);
	static void Run();
	static void Terminate();
	static void ReloadScene(Scene* scene);
	static void SetShaderVectorVariable( ShaderVectorVariable^ variable);
	static void SetShaderTextureVarable(ShaderTextureVariable^ variable);
	static void SetUpdateCallback(WndUpdateLoopCallback callback);
	static void SetMessageCallback(WndMessageLoopCallback callback);
	static void SetSceneReloadedCallback(SceneReloadedCallback callback);

	static void RotateObject(float x, float y, float z);
	static void Zoom(float amount);
	static void LoadModel(System::String^ modelName);
};

