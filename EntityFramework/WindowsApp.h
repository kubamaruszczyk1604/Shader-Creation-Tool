#pragma once


#include <Windows.h>
#include "KLM_Framework.h"

class WindowsApp
{
public:
	WindowsApp() = delete;
	WindowsApp(const WindowsApp&) = delete;
	WindowsApp& operator=(const WindowsApp&) = delete;

private:
	static int s_ClientWidth;
	static int s_ClientHeight;
	static HWND s_Hwnd;

	static DWORD s_WindowStyle;
	static std::string s_AppTitle;
	static bool s_ExitFlag;
	
	static Stopwatch s_GlobalTimer;
	static Stopwatch s_FrameTimer;
	static float s_TimeScale;


private:
	static LRESULT CALLBACK MsgProc(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam);

public:
	static bool Create(int width, int height, const std::string& title);
	static int Run();

};

