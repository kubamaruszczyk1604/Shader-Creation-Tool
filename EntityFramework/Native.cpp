#include "ApplicationInterface.h"
#include "Behaviours.h"

int CppApplicationInterface::StartRenderer(int width, int height,int handle)
{

	/*WindowsApp::Create(1280, 720, "DX WINDOW ");
	SceneManager::Load(new ExampleScene());
	const int appState = WindowsApp::Run();
	return appState;*/

	ExternalControllClass::Start(width, height, handle);
	SceneManager::Load(new ExampleScene());
	return 0;
}

void CppApplicationInterface::Update()
{
	ExternalControllClass::UpdateApp();
}

int CppApplicationInterface::Terminate()
{
	ExternalControllClass::Terminate();
	return 0;
}

void CppApplicationInterface::ReloadScene()
{
	ExternalControllClass::ReloadScene();
}
