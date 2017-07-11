#include "WindowsApp.h"
#include "InputSystem.h"
#include "SceneManager.h"


using namespace KLM_FRAMEWORK;

int WindowsApp::s_ClientWidth{ 800 };
int WindowsApp::s_ClientHeight{ 600 };
DWORD WindowsApp::s_WindowStyle{ WS_OVERLAPPEDWINDOW };
std::string WindowsApp::s_AppTitle{ "no title" };
HWND WindowsApp::s_Hwnd{ nullptr };
bool WindowsApp::s_ExitFlag{ false };
float WindowsApp::s_TimeScale{ 1.0f };

Stopwatch WindowsApp::s_GlobalTimer;
Stopwatch WindowsApp::s_FrameTimer;

bool WindowsApp::Create(int const width, int const height, const std::string& title)
{
	s_ClientWidth = width;
	s_ClientHeight = height;
	s_AppTitle = title;

	WNDCLASSEX wc;
	ZeroMemory(&wc, sizeof(WNDCLASSEX));

	wc.cbSize = sizeof(WNDCLASSEX);
	wc.style = CS_HREDRAW | CS_VREDRAW;
	wc.lpfnWndProc = MsgProc;
	wc.hIcon = LoadIcon(NULL, IDI_APPLICATION);
	wc.hCursor = LoadCursor(NULL, IDC_ARROW);

	wc.lpszClassName = "windowX";

	if (!RegisterClassEx(&wc))
	{
		::MessageBox(NULL, "Failed to register window", "Registration fail", MB_ICONEXCLAMATION | MB_OK);
		return false;
	}
	RECT rect;
	rect = { 0, 0, static_cast<LONG>(s_ClientWidth), static_cast<LONG>(s_ClientHeight) };
	AdjustWindowRect(&rect, s_WindowStyle, FALSE);

	const UINT wdth = rect.right - rect.left;
	const UINT hght = rect.bottom - rect.top;
	const UINT x = GetSystemMetrics(SM_CXSCREEN) / 2 - wdth / 2;
	const UINT y = GetSystemMetrics(SM_CYSCREEN) / 2 - hght / 2;

	s_Hwnd = CreateWindowEx(
		static_cast<DWORD>(0),
		wc.lpszClassName,
		s_AppTitle.c_str(),
		s_WindowStyle,
		x,
		y,
		width,
		height,
		nullptr,
		nullptr,
		::GetModuleHandle(nullptr),
		nullptr
		);

	if (!s_Hwnd)
	{
		::MessageBox(nullptr, "Failed to create Window", "Error", MB_OK);
		return false;
	}
	ShowWindow(s_Hwnd, SW_SHOW);

	PRINTL("WINDOW CREATED..");
	SceneManager::Initialize(width,height, s_Hwnd);
	return true;
}

int WindowsApp::Run()
{
	MSG msg = { nullptr };
	// Begin timers
	s_GlobalTimer.Start();
	PRINTL("GLOBAL TIMER START..");
	s_FrameTimer.Start();
	while (!s_ExitFlag && msg.message != WM_QUIT)
	{


		//Handle input from system
		if (PeekMessage(&msg, NULL, NULL, NULL, PM_REMOVE))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
		else
		{

			

			SceneManager::Update(s_FrameTimer.ElapsedTime()* s_TimeScale, s_GlobalTimer.ElapsedTime() * s_TimeScale);
			s_FrameTimer.Stop();
			s_FrameTimer.Start();
			//DXRenderer::SwapBuffers();
		 
		}

	}

	if (s_ExitFlag)
	{
		msg.wParam = 0;
	}

	// Shut down the scene manager
	SceneManager::ShutDown();

	return static_cast<int>(msg.wParam);
}

LRESULT WindowsApp::MsgProc(HWND const hWnd, UINT const msg, WPARAM const wParam, LPARAM const lParam)
{
	// Check for anttweakbar events
	/*if (TwEventWin(hWnd, msg, wParam, lParam))
	{
	return 0;
	}*/

	switch (msg)
	{

	case WM_DESTROY:
		PostQuitMessage(0);
		return 0;
	case WM_KEYDOWN:
		InputSystem::PropagateOnKeyDown(wParam);
		break;
	case WM_KEYUP:
		InputSystem::PropagateOnKeyUp(wParam);
		break;
	case WM_MOUSEMOVE:
		InputSystem::PropagateOnMouseMove(LOWORD(lParam), HIWORD(lParam));
		break;
	case WM_LBUTTONDOWN:
		InputSystem::PropagateOnMouseDown(0);
		break;
	case WM_LBUTTONUP:
		InputSystem::PropagateOnMouseUp(0);
		break;
	case WM_RBUTTONDOWN:
		InputSystem::PropagateOnMouseDown(1);
		break;
	case WM_RBUTTONUP:
		InputSystem::PropagateOnMouseUp(1);
		break;
	case WM_ENTERSIZEMOVE:
		break;
	case WM_EXITSIZEMOVE:
		break;
	case WM_SIZE:
		break;

	default:
		return DefWindowProc(hWnd, msg, wParam, lParam);
	}

	return LRESULT();
}
