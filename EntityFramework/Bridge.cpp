#include "Bridge.h"
#include "Behaviours.h"
#include "ResourceManagerWrapper.h" 

void _UpdateCallbackMethod()
{ 
	Bridge::CallUpdateDelegate();
}

void _MessageCallbackMethod(SUINT message, SUINT wParam, SUINT lParam)
{
	Bridge::CallMessageDelegate(message, wParam, lParam);
}

void _SceneReloadCallback()
{
	Bridge::CallOnSceneReloadDelegate();
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
	ControlApp::SetSceneReloadedCallback(_SceneReloadCallback);
	SceneManager::Load(new SCTScene());
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
	ControlApp::ReloadScene(new SCTScene());
}

void Bridge::SetVariable(ShaderVectorVariable ^ variable)
{
	ControlApp::SetShaderVectorVariable(variable);
}

void Bridge::SetVariable(ShaderTextureVariable ^ variable)
{
	ControlApp::SetShaderTextureVarable(variable);
}

void Bridge::AddWndProcUpdateCallback(OnWndProcUpdate ^ dlg)
{
	s_OnProcUpdateDelegate += dlg;
}

void Bridge::AddWndProcMessageCallback(OnWndProcMessage ^ dlg)
{
	s_OnMessageDelegate += dlg;
}

void Bridge::AddOnSceneReloadCallback(OnSceneReload ^ dlg)
{
	s_OnSceneReloadDelegate += dlg;
}

System::String ^ Bridge::GetLastCompilerMessage()
{
	return ResourceManagerWrapper::GetLastCompileMessage();
}

void Bridge::ClearLastCompilerMessage()
{
	ResourceManagerWrapper::ClearCompilerMessages();
}

void Bridge::RotateObject(float x, float y, float z)
{
	ControlApp::RotateObject(x, y, z);
}

void Bridge::Zoom(float amount)
{
	ControlApp::Zoom(amount);
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

void Bridge::CallOnSceneReloadDelegate()
{
	if (s_OnSceneReloadDelegate)
		s_OnSceneReloadDelegate();
}
