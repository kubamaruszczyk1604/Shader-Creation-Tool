#pragma once
#include "DataWrappers.h"


public ref class Bridge
{
public:
	static float TESTUJE = 10;
	static int StartRenderer(int width, int height, System::IntPtr handle);
	static int Terminate();
	static void ReloadScene();
};



