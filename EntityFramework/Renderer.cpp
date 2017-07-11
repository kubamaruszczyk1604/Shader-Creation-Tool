#include "Renderer.h"
#include  "D3DRenderer.h"
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
		return result; 

	}
	void Renderer::Render(ListOfEntities* listOfEntities)
	{
		for (int i = 0; i < listOfEntities->size(); ++i)
		{
			Entity* e = (*listOfEntities)[i].get();

			DXRenderer::Render(e);
			//PRINTL("Render ENTITY: " + e->GetName() + " is at position: " + ToString(e->GetTransform()->GetWorldPosition()));

		}

	}

	void Renderer::Update(const float deltaTime, const float totalTime)
	{
		DXRenderer::Update(deltaTime, totalTime);
	}

	void Renderer::ClearScreen(const Colour & colour)
	{
		DXRenderer::ClearScreen(colour);
	}

	void Renderer::SwapBuffers()
	{
		DXRenderer::SwapBuffers();
	}

	void Renderer::SetCullMode(const CullMode mode)
	{

	}
	void Renderer::SetFillMode(const FillMode mode)
	{

	}
	void Renderer::SetActiveCamera(Camera* camera)
	{
		DXRenderer::SetActiveCamera(camera);
	}
}
