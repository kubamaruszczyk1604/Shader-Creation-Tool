#pragma once
#include "StdIncludes.h"
class ExternalControllClass
{
private:
	static bool s_ReloadRequest;
	static Stopwatch s_GlobalTimer;
	static Stopwatch s_FrameTimer;
	static float s_TimeScale;
	

public:
	ExternalControllClass() = delete;
	ExternalControllClass(const ExternalControllClass&) = delete;
	ExternalControllClass& operator=(const ExternalControllClass&) = delete;
	static void Start(int width, int height, int handle);
	static void UpdateApp();
	static void Terminate();
	static void ReloadScene();
};

