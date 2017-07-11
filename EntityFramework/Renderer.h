#pragma once
#include "StdIncludes.h"
#include "RendererModes.h"
#include "Camera.h"
#include "EntityTypes.h"

namespace KLM_FRAMEWORK
{

	class Renderer
	{
	private:
		static GfxAPI m_API;
	public:

		Renderer() = delete;
		Renderer(const Renderer&) = delete;
		Renderer& operator=(const Renderer&) = delete;



		static bool Initialize(const int width, const int height, const HWND handle, GfxAPI gfxApi);
		static void Render(ListOfEntities* listOfEntities);
		static void Terminate();
		static void Update(const float deltaTime, const float totalTime);
		static void ClearScreen(const Colour& colour);
		static void SwapBuffers();
		static void SetCullMode(const CullMode mode);
		static void SetFillMode(const FillMode mode);

		static void SetActiveCamera(Camera* camera);
	

		static GfxAPI GetAPI() { return m_API; }
	};

}