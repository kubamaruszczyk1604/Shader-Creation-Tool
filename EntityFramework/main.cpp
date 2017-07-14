
#include "WindowsApp.h"
#include "Behaviours.h"

struct AtExit
{
	~AtExit()
	{
		_CrtDumpMemoryLeaks();
	}
} doAtExit;


int main()
{
	WindowsApp::Create(1280, 720, "DX WINDOW ");
	SceneManager::Load(new ExampleScene());
	const int appState = WindowsApp::Run();
	WaitForKeypress();
	return appState;
}

