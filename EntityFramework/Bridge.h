#pragma once
#include "DataWrappers.h"



public ref class Bridge
{
private:
	static OnWndProcUpdate^ s_OnProcUpdateDelegate;
	static OnWndProcMessage^ s_OnMessageDelegate;
public:
	static float TESTUJE = 10;
	static int StartRenderer(int width, int height, System::IntPtr handle);
	static int Terminate();
	static void ReloadScene();
	static void SetVariable(ShaderVectorVariable^ variable);
	static void SetVariable(ShaderTextureVariable^ variable);
	static void AddWndProcUpdateCallback(OnWndProcUpdate^ dlg);
	static void AddWndProcMessageCallback(OnWndProcMessage^ dlg);
	static System::String^ GetLastCompilerMessage();
	static void ClearLastCompilerMessage();



	// used only on C++ side
	static void CallUpdateDelegate();
	static void CallMessageDelegate(SUINT message, SUINT wParam, SUINT lParam);
	
};



