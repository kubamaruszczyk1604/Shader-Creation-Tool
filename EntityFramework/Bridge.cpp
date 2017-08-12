#include "Bridge.h"
#include "Behaviours.h"


void _UpdateCallbackMethod()
{ 
	Bridge::CallDelegate();
}


int Bridge::StartRenderer(int width, int height, System::IntPtr handle)
{

	/*WindowsApp::Create(1280, 720, "DX WINDOW ");
	SceneManager::Load(new ExampleScene());
	const int appState = WindowsApp::Run();
	return appState;*/

	ControlApp::Create(width, height, handle);
	ControlApp::SetLoopCallback(_UpdateCallbackMethod);
	SceneManager::Load(new ExampleScene());
	ControlApp::Run();

	return 0;
}



int Bridge::Terminate()
{
	ControlApp::Terminate();
	return 0;
}

void Bridge::ReloadScene()
{
	ControlApp::ReloadScene();
}

void Bridge::SetVariable(ShaderVectorVariable ^ variable)
{
	ControlApp::SetShaderVectorVariable(variable);
}

void Bridge::AddWndProcCallback(OnWndProcUpdate ^ dlg)
{
	s_dlg += dlg;
}

void Bridge::CallDelegate()
{
	s_dlg();
}
