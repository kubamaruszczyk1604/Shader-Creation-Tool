#pragma once
#include "StdIncludes.h"
#include "KLM_Framework.h"
#include <Windows.h>

#include "DataWrappers.h"

class ControlApp
{
private:
	static bool s_ReloadRequest;
	static bool s_Running;
	static Stopwatch s_GlobalTimer;
	static Stopwatch s_FrameTimer;
	static float s_TimeScale;
	

public:
	ControlApp() = delete;
	ControlApp(const ControlApp&) = delete;
	ControlApp& operator=(const ControlApp&) = delete;
	static void Create(int width, int height, System::IntPtr handle);
	static void Run();
	static void Terminate();
	static void ReloadScene();
	static void SetShaderVectorVariable(System::String^ name, ShaderVectorVariable^ variable);
};

