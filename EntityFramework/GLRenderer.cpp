#include "GLRenderer.h"
#include "ModelComponent.h"
#include "Entity.h"
#include "GLTexture.h"
#include <msclr\marshal_cppstd.h>
#include "ShaderVariableContainer.h"
#include "VariableStrings.cs"

namespace KLM_FRAMEWORK
{
	using namespace msclr::interop;

	bool GLRenderer::s_IsRunning{ false };
	HWND GLRenderer::s_hWnd{ 0 };
	HGLRC GLRenderer::s_hGLRC{ nullptr };
	HDC GLRenderer::s_hDevCtx{ nullptr };
	Camera* GLRenderer::s_CurrentCamera{ nullptr };

	int GLRenderer::s_ScreenWidth{ 800 };
	int GLRenderer::s_ScreenHeight{ 600 };

	bool GLRenderer::s_MakeCurrentCalled{ false };


	Vec4 GLRenderer::VectorVariableTest{ Vec4(1.0f) };

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
		glEnable(GL_BLEND);
		glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
		glEnable(GL_MULTISAMPLE);
		glViewport(0, 0, width, height);
	
		s_IsRunning = true;
		glewInit();
		//LightBase::InitializeLightSystem();
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
		GLuint ambientID = glGetUniformLocation(shaderProgID, "ambient");
		GLuint diffuseID = glGetUniformLocation(shaderProgID, "diffuse");
		GLuint specularID = glGetUniformLocation(shaderProgID, "specular");


		glUniform4fv(ambientID, 1, &material->GetAmbientColPtr()->r);
		glUniform4fv(diffuseID, 1, &material->GetDiffuseColPtr()->r);
		glUniform4fv(specularID, 1, &material->GetSpecularColPtr()->r);


		//TExtures
		const Texture* diffuseMapGen = material->GetDiffuseMap();
		const Texture* specularMapGen = material->GetSpecularMap();
		const Texture* normalMapGen = material->GetNormalMap();


		if (diffuseMapGen)
		{
			const void* diffuse = material->GetDiffuseMap()->GetApiSpecificTexture();


			GLTexture* diffuseMap = const_cast<GLTexture*>(static_cast<const GLTexture*>(diffuse));

			GLuint samplerID = glGetUniformLocation(shaderProgID, "Texture0");
			glUniform1i(samplerID, 0);
			glActiveTexture(GL_TEXTURE0);
			glBindTexture(GL_TEXTURE_2D, diffuseMap->GetID());
		}
		if (specularMapGen)
		{
		
		}
		if (normalMapGen)
		{
		
		}


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
	

		/*String^ stringon = "MVP";
		std::string standardString = marshal_as<std::string>(stringon);*/

		//Matrices
		GLuint MVP_ID = glGetUniformLocation(shaderProgID, "MVP");
		glUniformMatrix4fv(MVP_ID, 1, GL_FALSE, &MVP[0][0]);

		GLuint WORLD_ID = glGetUniformLocation(shaderProgID, "WORLD");
		glm::mat4 world = entity->GetTransform()->GetWorldMat();
		glUniformMatrix4fv(WORLD_ID, 1, GL_FALSE, &world[0][0]);


		
		GLuint WORLD_INVERSED_ID = glGetUniformLocation(shaderProgID, "WORLD_INVERSE");
		glm::mat4 worldInverse = glm::transpose(glm::inverse(entity->GetTransform()->GetWorldMat()));
		glUniformMatrix4fv(WORLD_INVERSED_ID, 1, GL_FALSE, &worldInverse[0][0]);

		
        //Lighting
		if (LightBase::IsRequestingUpdate())
		{

			for (int lIndex = 0; lIndex < LightBase::MAX_LIGHTS; ++lIndex)
			{
				const ShaderLightInfoStruct& l = LightBase::GetLightInfo(lIndex);

				const std::string baseStr = "lights[" + ToString(lIndex) + "].";
				//Enabled
				GLuint loc = glGetUniformLocation(shaderProgID, (baseStr + "enabled").c_str());
				glUniform1i(loc, l.Enabled);

				//Position
				loc = glGetUniformLocation(shaderProgID, (baseStr + "position").c_str());
				glUniform4fv(loc,1, &l.Position.x);

				//Ambient
				loc = glGetUniformLocation(shaderProgID, (baseStr + "ambient").c_str());
				glUniform4fv(loc, 1, &l.Ambient.r);

				//Diffuse
				loc = glGetUniformLocation(shaderProgID, (baseStr + "diffuse").c_str());
				glUniform4fv(loc, 1, &l.Diffuse.r);

				//Specular
				loc = glGetUniformLocation(shaderProgID, (baseStr + "specular").c_str());
				Vec4 tmpSpc(0, 0, 0, 0);
				glUniform4fv(loc, 1, &tmpSpc.r);

				//Spot cutoff
				loc = glGetUniformLocation(shaderProgID, (baseStr + "spot_cutoff").c_str());
				glUniform1f(loc,l.SpotCutoff);

				//Spot Direction
				loc = glGetUniformLocation(shaderProgID, (baseStr + "spot_direction").c_str());
				glUniform3fv(loc, 1, &l.SpotDirection.x);

				//Spot exponent
				loc = glGetUniformLocation(shaderProgID, (baseStr + "spot_exponent").c_str());
				glUniform1fv(loc, 1, &l.SpotDecay);

				//Spot Attenuation
				loc = glGetUniformLocation(shaderProgID, (baseStr + "attenuation").c_str());
				glUniform3fv(loc, 1, &l.Attenuation.x);

				
			}

			
		}

		mc->GetMesh()->GetVBO()->Draw(PrimitiveType::TRIANGLES);
	
	}

	void GLRenderer::RenderSCTool(Entity * entity)
	{
	
		Component* c = entity->GetComponentFirst(ComponentType::MODEL_COMPONENT);
		if (!c) return;

		ModelComponent* mc = static_cast<ModelComponent*>(c);
		Material* material = const_cast<Material*>(mc->GetMaterial());
		if (!material) return;

		GLuint shaderProgID = material->GetShaderProgID();
		material->SetCurrentShaders();


		for (int i = 0; i < ShaderVariableContainer::GetSize_VectorVariables(); ++i)
		{
			ShaderVectorVariable^ variable =  ShaderVariableContainer::GetShaderVectorVariable(i);

		    String^ managedstr = variable->GetName();
			std::string name = marshal_as<std::string>(managedstr);

			if (variable->GetType() == ShaderVariableType::Vector4)
			{
				GLuint id = glGetUniformLocation(shaderProgID, name.c_str());
				Vec4 vect = DataConverter::ToVec4(variable);
				glUniform4fv(id, 1, &vect.x);
			}
		}


		for (int i = 0; i < ShaderVariableContainer::GetSize_TextureVariables(); ++i)
		{
			const std::string name = marshal_as<std::string>(ShaderVariableContainer::GetShaderTextureVariable(i)->GetName());
			
			Texture* t = DataConverter::ToTexture(ShaderVariableContainer::GetShaderTextureVariable(i));
			const void* vpoint = t->GetApiSpecificTexture();
			GLTexture* texture = const_cast<GLTexture*>(static_cast<const GLTexture*>(vpoint));

			GLuint samplerID = glGetUniformLocation(shaderProgID,name.c_str());

			//DEBUG("Disp Name: " + name + "  Location" + ToString((int)samplerID));
			if (samplerID != -1 && texture)
			{
				glUniform1i(samplerID, i);
				glActiveTexture(GL_TEXTURE0 + i);
				glBindTexture(GL_TEXTURE_2D, texture->GetID());
			}
		}

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

		GLuint WORLD_INVERSED_ID = glGetUniformLocation(shaderProgID, "WORLD_INVERSE");
		glm::mat4 worldInverse = glm::transpose(glm::inverse(entity->GetTransform()->GetWorldMat()));
		glUniformMatrix4fv(WORLD_INVERSED_ID, 1, GL_FALSE, &worldInverse[0][0]);


		//Lighting
		

		mc->GetMesh()->GetVBO()->Draw(PrimitiveType::TRIANGLES);
	}

	void GLRenderer::Update(const float deltaTime, const float totalTime)
	{
		LightBase::UpdateFinished();
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