#pragma once
#include "DataWrappers.h"



public ref class Bridge
{
private:
	static OnWndProcUpdate^ s_dlg;
public:
	static float TESTUJE = 10;
	static int StartRenderer(int width, int height, System::IntPtr handle);
	static int Terminate();
	static void ReloadScene();
	static void SetVariable(ShaderVectorVariable^ variable);
	static void AddWndProcCallback(OnWndProcUpdate^ dlg);

	static void CallDelegate();
	
};



