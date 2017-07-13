#include "GLRenderer.h"
#include "ModelComponent.h"
#include "Entity.h"
namespace KLM_FRAMEWORK
{
	bool GLRenderer::s_IsRunning{ false };
	HWND GLRenderer::s_hWnd{ 0 };
	HGLRC GLRenderer::s_hGLRC{ nullptr };
	HDC GLRenderer::s_hDevCtx{ nullptr };
	Camera* GLRenderer::s_CurrentCamera{ nullptr };

	int GLRenderer::s_ScreenWidth{ 800 };
	int GLRenderer::s_ScreenHeight{ 600 };

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
		s_ScreenWidth = width;
		s_ScreenHeight = height;
		s_hWnd = handle;
		if ((s_hDevCtx = GetDC(s_hWnd)) == NULL) return false;
	
		// set the pixel format
		if (!KLMSetPixelFormat(s_hDevCtx)) return false;
		
		// Create context 
		if ((s_hGLRC = wglCreateContext(s_hDevCtx)) == NULL) return false;
		
        
		wglMakeCurrent(s_hDevCtx, s_hGLRC);
		
		// set defaults
		glEnable(GL_DEPTH_TEST);
		glDepthFunc(GL_LEQUAL);
		glCullFace(GL_FRONT_AND_BACK);
		glViewport(0, 0, width, height);
		s_IsRunning = true;
		glewInit();
		return true;
	}

	void GLRenderer::Render(Entity * entity)
	{
		//static float i = 0;
		//i += 0.01f;
		//ClearScreen(Colour(sin(i), 1, cos(i), 1));
		Component* c = entity->GetComponentFirst(ComponentType::MODEL_COMPONENT);
		if (!c) return;

		ModelComponent* mc = static_cast<ModelComponent*>(c);
		Material* material = const_cast<Material*>(mc->GetMaterial());
		if (!material) return;
		//PRINTL("Render ENTITY: " + entity->GetName() + " is at position: " + ToString(entity->GetTransform()->GetWorldPosition()));
	

		GLuint shaderProgID = material->GetShaderProgID();
		material->SetCurrentShaders();
		//Colours
		/*GLuint ambientID = glGetUniformLocation(shaderProgID, "ambient");
		GLuint diffuseID = glGetUniformLocation(shaderProgID, "diffuse");
		GLuint specularID = glGetUniformLocation(shaderProgID, "specular");


		glUniform4fv(ambientID, 1, &material->GetAmbientColPtr()->r);
		glUniform4fv(diffuseID, 1, &material->GetDiffuseColPtr()->r);
		glUniform4fv(specularID, 1, &material->GetSpecularColPtr()->r);*/


		if (!s_CurrentCamera) return;
		if (!s_CurrentCamera->isActive()) return;


		Mat4 worldView;
		if (s_CurrentCamera->GetParent() == nullptr)
		{
			worldView = s_CurrentCamera->GetTransformMatrix() * entity->GetTransform()->GetWorldMat();
		}
		else
		{
			Entity* parent = s_CurrentCamera->GetParent();
			worldView =
				s_CurrentCamera->SetTransformMatrix(parent->GetTransform()->GetWorldMat())
				* entity->GetTransform()->GetWorldMat();

			//ubs.CameraPosition = (parent->GetTransform()->GetWorldMat()*glm::vec4(0, 0, 0, 1));
		}
		Mat4 MVP = s_CurrentCamera->GetProjectionMatrix(s_ScreenWidth, s_ScreenHeight) * worldView;
	

		
		//Matrices
		GLuint MVP_ID = glGetUniformLocation(shaderProgID, "MVP");
		glUniformMatrix4fv(MVP_ID, 1, GL_FALSE, &MVP[0][0]);

		GLuint WORLD_ID = glGetUniformLocation(shaderProgID, "WORLD");
		glm::mat4 world = entity->GetTransform()->GetWorldMat();
		glUniformMatrix4fv(WORLD_ID, 1, GL_FALSE, &world[0][0]);
		
		mc->GetMesh()->GetVBO()->Draw(PrimitiveType::TRIANGLES);
	
	}

	void GLRenderer::Update(const float deltaTime, const float totalTime)
	{
		
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
		s_CurrentCamera = camera;
	}
}