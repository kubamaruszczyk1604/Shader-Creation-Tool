#include "GLRenderer.h"

namespace KLM_FRAMEWORK
{
	bool GLRenderer::s_IsRunning{ false };
	HWND GLRenderer::s_hWnd{ 0 };
	HGLRC GLRenderer::s_hGLRC{ nullptr };
	HDC GLRenderer::s_hDevCtx{ nullptr };

	bool GLRenderer::s_MakeCurrentCalled{ false };


	bool GLRenderer::KLMSetPixelFormat(HDC hdc)
	{
		PIXELFORMATDESCRIPTOR pfd;
	
		pfd.nSize = sizeof(PIXELFORMATDESCRIPTOR);
		pfd.nVersion = 1;
		pfd.dwFlags = PFD_DRAW_TO_WINDOW |
			          PFD_SUPPORT_OPENGL | 
			          PFD_DOUBLEBUFFER |
			          PFD_SUPPORT_COMPOSITION;
		pfd.iPixelType = PFD_TYPE_RGBA;
		pfd.cColorBits = 32;
		pfd.cRedBits = pfd.cRedShift = pfd.cGreenBits = pfd.cGreenShift =
			pfd.cBlueBits = pfd.cBlueShift = 0;
		pfd.cAlphaBits = pfd.cAlphaShift = 0;
		pfd.cAccumBits = pfd.cAccumRedBits = pfd.cAccumGreenBits =
			pfd.cAccumBlueBits = pfd.cAccumAlphaBits = 0;
		pfd.cDepthBits = 24;
		pfd.cStencilBits = pfd.cAuxBuffers = 0;
		pfd.iLayerType = PFD_MAIN_PLANE;
		pfd.bReserved = 0;
		pfd.dwLayerMask = pfd.dwVisibleMask = pfd.dwDamageMask = 0;

		// choose pixel format returns the number most similar pixel format available
		int n = ChoosePixelFormat(hdc, &pfd);
		// set pixel format returns whether it sucessfully set the pixel format
		return SetPixelFormat(hdc, n, &pfd);
	}

	bool GLRenderer::Initialize(const int width, const int height, const HWND handle)
	{
		s_hWnd = handle;
		if ((s_hDevCtx = GetDC(s_hWnd)) == NULL) return false;
	
		// set the pixel format
		if (!KLMSetPixelFormat(s_hDevCtx)) return false;
		
		// Create context 
		if ((s_hGLRC = wglCreateContext(s_hDevCtx)) == NULL) return false;
		
		// set defaults
		glEnable(GL_DEPTH_TEST);
		glDepthFunc(GL_LEQUAL);
		glViewport(0, 0, width, height);
		s_IsRunning = true;
		return true;
	}

	void GLRenderer::Render(Entity * entity)
	{
		static float i = 0;
		i += 0.01f;
		ClearScreen(Colour(sin(i), 1, cos(i), 1));
	}

	void GLRenderer::Update(const float deltaTime, const float totalTime)
	{
		if (!s_MakeCurrentCalled)
		{
			wglMakeCurrent(s_hDevCtx, s_hGLRC);
			s_MakeCurrentCalled = true;
		}
	}

	void GLRenderer::Terminate()
	{
		// release device context
		ReleaseDC(s_hWnd, s_hDevCtx);
		// default to no context
		wglMakeCurrent(0, 0);
		// delete GL context0
		wglDeleteContext(s_hGLRC);
		// delete device context
		DeleteDC(s_hDevCtx);
	}

	void GLRenderer::ClearScreen(const Colour & colour)
	{
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
		glClearColor(colour.r, colour.g, colour.b, colour.a);
	}

	void GLRenderer::SwapBuffers()
	{
		::SwapBuffers(s_hDevCtx);
		//wglSwapLayerBuffers(s_hDevCtx,1);
	}

	void GLRenderer::SetCullMode(const CullMode mode)
	{

	}

	void GLRenderer::SetFillMode(const FillMode mode)
	{

	}

	void GLRenderer::SetActiveCamera(Camera * camera)
	{

	}
}