#include "Bridge.h"
#include "Behaviours.h"


void _UpdateCallbackMethod()
{ 
	Bridge::CallUpdateDelegate();
}

void _MessageCallbackMethod(SUINT message, SUINT wParam, SUINT lParam)
{
	Bridge::CallMessageDelegate(message, wParam, lParam);
}


int Bridge::StartRenderer(int width, int height, System::IntPtr handle)
{

	/*WindowsApp::Create(1280, 720, "DX WINDOW ");
	SceneManager::Load(new ExampleScene());
	const int appState = WindowsApp::Run();
	return appState;*/

	ControlApp::Create(width, height, handle);
	ControlApp::SetUpdateCallback(_UpdateCallbackMethod);
	ControlApp::SetMessageCallback(_MessageCallbackMethod);
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

void Bridge::SetVariable(ShaderTextureVariable ^ variable)
{
	throw gcnew System::NotImplementedException();
}

void Bridge::AddWndProcUpdateCallback(OnWndProcUpdate ^ dlg)
{
	s_OnProcUpdateDelegate += dlg;
}

void Bridge::AddWndProcMessageCallback(OnWndProcMessage ^ dlg)
{
	s_OnMessageDelegate += dlg;
}

void Bridge::CallUpdateDelegate()
{
	if(s_OnProcUpdateDelegate)
	s_OnProcUpdateDelegate();
}

void Bridge::CallMessageDelegate(SUINT message, SUINT wParam, SUINT lParam)
{
	if(s_OnMessageDelegate)
	s_OnMessageDelegate(message, wParam, lParam);
}
