#pragma once
#include "StdIncludes.h"
#include "Camera.h"
#include "RendererModes.h"
#include "LightBase.h"

#include "DataWrappers.h"

namespace KLM_FRAMEWORK
{
	using namespace System;
	using namespace System::Collections::Generic;

	public ref class ShaderVariableContainer
	{
	private:
		static List<ShaderVectorVariable^> s_ShaderVaraiablesList;

	public:
		static int GetSize() { return s_ShaderVaraiablesList.Count; }
		static void AddVariable(ShaderVectorVariable^ variable);
		static ShaderVectorVariable^ GetShaderVectorVariable(int index);
	
	};
}


namespace KLM_FRAMEWORK
{

    class GLRenderer
	{
	private:
		static bool	s_IsRunning;
		static HWND	s_hWnd;
		static HGLRC s_hGLRC;
		static HDC s_hDevCtx;

		static Camera* s_CurrentCamera;

		static int s_ScreenWidth;
		static int s_ScreenHeight;
		static bool s_MakeCurrentCalled;
		static bool KLMSetPixelFormat(HDC hdc);

		static Vec4 VectorVariableTest;
	

	public:
		GLRenderer() = delete;
		GLRenderer(const GLRenderer&) = delete;
		GLRenderer& operator=(const GLRenderer&) = delete;

		static bool Initialize(const int width, const int height, const HWND handle);
		static void Render(Entity* entity);

		static void RenderSCTool(Entity* entity);
		static void Update(const float deltaTime, const float totalTime);
		static void Terminate();
		static void ClearScreen(const Colour& colour);
		static void SwapBuffers();
		static void SetCullMode(const CullMode mode);
		static void SetFillMode(const FillMode mode);
		static void SetActiveCamera(Camera* camera);
	};

}