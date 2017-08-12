#include "ControlApp.h"
#include "InputSystem.h"
#include "SceneManager.h"
#include "Behaviours.h"
#include "GLRenderer.h"

using namespace KLM_FRAMEWORK;

bool ControlApp::s_ReloadRequest{ false };
bool ControlApp::s_Running{ false };
float ControlApp::s_TimeScale{ 1.0f };

Stopwatch ControlApp::s_GlobalTimer;
Stopwatch ControlApp::s_FrameTimer;

WndUpdateLoopCallback ControlApp::s_UpdateLoopCallback{ nullptr };
WndMessageLoopCallback ControlApp::s_MessageLoopCallback{ nullptr };


void ControlApp::Create(int width, int height, System::IntPtr handle)
{
	//PRINTL("HANDLE IS: " + ToString(handle));
	SceneManager::Initialize(width,height,static_cast<HWND>(handle.ToPointer()));
	s_FrameTimer.Start();
	s_GlobalTimer.Start();
	PRINTL("GLOBAL TIMER START..");
	s_Running = true;
}

void ControlApp::Run()
{
	MSG msg = { nullptr };
	while (s_Running)
	{
	  //Handle input from system
		if (PeekMessage(&msg, NULL, NULL, NULL, PM_REMOVE))
		{	
			TranslateMessage(&msg);
			DispatchMessage(&msg);
			if (s_MessageLoopCallback)
			{
				s_MessageLoopCallback(msg.message, msg.wParam, msg.lParam);
			}
		}
		else
		{
			
		}
		if (s_UpdateLoopCallback)
		{
			s_UpdateLoopCallback();
		}
		
		SceneManager::Update(s_FrameTimer.ElapsedTime()* s_TimeScale, s_GlobalTimer.ElapsedTime() * s_TimeScale);
		s_FrameTimer.Stop();
		if (s_ReloadRequest)
		{
			SceneManager::Load(new ExampleScene());
			s_ReloadRequest = false;
		}
		s_FrameTimer.Start();
	}

	s_ReloadRequest = false;
	SceneManager::ShutDown();
}

void ControlApp::Terminate()
{
	s_Running = false;
}

void ControlApp::ReloadScene()
{
	s_ReloadRequest = true;
}

void ControlApp::SetShaderVectorVariable(ShaderVectorVariable ^ variable)
{ 
	//Tu zmiana bedzie
	//GLRenderer::SetShaderVariable(variable);
	ShaderVariableContainer::AddVariable(variable);
}

void ControlApp::SetUpdateCallback(WndUpdateLoopCallback callback)
{
	s_UpdateLoopCallback = callback;
}

void ControlApp::SetMessageCallback(WndMessageLoopCallback callback)
{
	s_MessageLoopCallback = callback;
}

