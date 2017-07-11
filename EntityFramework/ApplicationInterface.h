#pragma once

public ref class CppApplicationInterface
{
public:
	static int StartRenderer(int width, int height, System::IntPtr handle);
	static void Update();
	static int Terminate();
	static void ReloadScene();
};



