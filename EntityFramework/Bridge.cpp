#include "Bridge.h"
#include "Behaviours.h"



int Bridge::StartRenderer(int width, int height, System::IntPtr handle)
{

	/*WindowsApp::Create(1280, 720, "DX WINDOW ");
	SceneManager::Load(new ExampleScene());
	const int appState = WindowsApp::Run();
	return appState;*/

	ControlApp::Create(width, height, handle);
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
	ControlApp::SetShaderVectorVariable("Cos", variable);
}
