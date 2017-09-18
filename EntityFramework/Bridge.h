#pragma once
#include "DataWrappers.h"



public ref class Bridge
{
private:
	static OnWndProcUpdate^ s_OnProcUpdateDelegate;
	static OnWndProcMessage^ s_OnMessageDelegate;
	static OnSceneReload^ s_OnSceneReloadDelegate;
public:
	static float TESTUJE = 10;
	static int StartRenderer(int width, int height, System::IntPtr handle);
	static int Terminate();
	static void ReloadScene();
	static void SetVariable(ShaderVectorVariable^ variable);
	static void SetVariable(ShaderTextureVariable^ variable);
	static void AddWndProcUpdateCallback(OnWndProcUpdate^ dlg);
	static void AddWndProcMessageCallback(OnWndProcMessage^ dlg);
	static void AddOnSceneReloadCallback(OnSceneReload^ dlg);
	static System::String^ GetLastCompilerMessage();
	static void ClearLastCompilerMessage();
	static void RotateObject(float x, float y, float z);
	static void Zoom(float amount);
	static void LoadModel(System::String^ modelName);


	// used only on C++ side
	static void CallUpdateDelegate();
	static void CallMessageDelegate(SUINT message, SUINT wParam, SUINT lParam);
	static void CallOnSceneReloadDelegate();
	
};



