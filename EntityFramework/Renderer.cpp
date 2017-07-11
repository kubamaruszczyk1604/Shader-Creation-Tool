#include "Renderer.h"
#include  "D3DRenderer.h"
#include "GLRenderer.h"
#include "Entity.h"


namespace KLM_FRAMEWORK
{

	GfxAPI Renderer::m_API;

	bool Renderer::Initialize(const int width, const int height, const HWND handle, GfxAPI gfxApi)
	{
		m_API = gfxApi;
		PRINTL("Renderer is starting..");
		bool result{ false };
		if (gfxApi == GfxAPI::D3D11)
		{
			result = DXRenderer::Initialize(width, height, handle);
		}
		else if (gfxApi == GfxAPI::GL)
		{
			result = GLRenderer::Initialize(width, height, handle);
		}
		return result; 

	}
	void Renderer::Render(ListOfEntities* listOfEntities)
	{
		GLRenderer::Render(nullptr);
		for (int i = 0; i < listOfEntities->size(); ++i)
		{
			Entity* e = (*listOfEntities)[i].get();
		
			if (m_API == GfxAPI::D3D11)
			{
				DXRenderer::Render(e);
			}
			else if (m_API == GfxAPI::GL)
			{
				GLRenderer::Render(e);
			}

		}

	}

	void Renderer::Terminate()
	{
		if (m_API == GfxAPI::D3D11)
		{
			
		}
		else if (m_API == GfxAPI::GL)
		{
			GLRenderer::Terminate();
		}
	}

	void Renderer::Update(const float deltaTime, const float totalTime)
	{
		if (m_API == GfxAPI::D3D11)
		{
			DXRenderer::Update(deltaTime, totalTime);
		}
		else if (m_API == GfxAPI::GL)
		{
			GLRenderer::Update(deltaTime, totalTime);
		}
		
	}

	void Renderer::ClearScreen(const Colour & colour)
	{

		if (m_API == GfxAPI::D3D11)
		{
			DXRenderer::ClearScreen(colour);
		}
		else if (m_API == GfxAPI::GL)
		{
			GLRenderer::ClearScreen(colour);
		}
		
	}

	void Renderer::SwapBuffers()
	{
		if (m_API == GfxAPI::D3D11)
		{
			DXRenderer::SwapBuffers();
		}
		else if (m_API == GfxAPI::GL)
		{
			GLRenderer::SwapBuffers();
		}
		
	}

	void Renderer::SetCullMode(const CullMode mode)
	{

	}
	void Renderer::SetFillMode(const FillMode mode)
	{

	}
	void Renderer::SetActiveCamera(Camera* camera)
	{
		if (m_API == GfxAPI::D3D11)
		{
			DXRenderer::SetActiveCamera(camera);
		}
		else if (m_API == GfxAPI::GL)
		{
			GLRenderer::SetActiveCamera(camera);
		}

		
	}
}
