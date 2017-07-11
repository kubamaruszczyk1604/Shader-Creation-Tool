#include "ExternalControllClass.h"
#include "InputSystem.h"
#include "SceneManager.h"
#include "Behaviours.h"
#include <Windows.h>
using namespace KLM_FRAMEWORK;

bool ExternalControllClass::s_ReloadRequest{ false };

float ExternalControllClass::s_TimeScale{ 1.0f };

Stopwatch ExternalControllClass::s_GlobalTimer;
Stopwatch ExternalControllClass::s_FrameTimer;


void ExternalControllClass::Start(int width, int height, System::IntPtr handle)
{
	//PRINTL("HANDLE IS: " + ToString(handle));
	SceneManager::Initialize(width,height,static_cast<HWND>(handle.ToPointer()));
	s_FrameTimer.Start();
	s_GlobalTimer.Start();
	PRINTL("GLOBAL TIMER START..");
}

void ExternalControllClass::Terminate()
{
	s_ReloadRequest = false;
	SceneManager::ShutDown();
}

void ExternalControllClass::ReloadScene()
{
	s_ReloadRequest = true;
}


void ExternalControllClass::UpdateApp()
{
	SceneManager::Update(s_FrameTimer.ElapsedTime()* s_TimeScale, s_GlobalTimer.ElapsedTime() * s_TimeScale);
	s_FrameTimer.Stop();
	if (s_ReloadRequest)
	{
		SceneManager::Load(new ExampleScene());
		s_ReloadRequest = false;
	}
	s_FrameTimer.Start();
}