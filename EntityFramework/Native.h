#pragma once

public ref class CppApplicationInterface
{
public:
	static int StartRenderer(int width, int height, int handle);
	static void Update();
	static int Terminate();
	static void ReloadScene();
};



