#pragma once

public ref class Bridge
{
public:
	static int StartRenderer(int width, int height, System::IntPtr handle);
	static int Terminate();
	static void ReloadScene();
};



