#include "Behaviours.h"

void ExampleScene::OnStart()
{
	PRINTL("OnStart()");

	////////////////////////////////////////////   MATERIALS //////////////////////////
	MaterialDescription desc;
	//desc.AmbientCol = Colour(0.1, 0.1, 0.1, 1);
	//desc.DiffuseReflectivity = Colour(1, 1, 1, 1);
	//desc.SpecularReflectivity = Colour(1, 1,1, 1);
	//desc.DiffuseMap = PathList::TEXTURE_DIR + "logoP.png";
	//	desc.SpecularMap = PathList::TEXTURE_DIR + "logo.bmp";
	//desc.NormalMap = PathList::TEXTURE_DIR + "logo.bmp";

	if (Renderer::GetAPI() == GfxAPI::D3D11)
	{
		desc.DXShaderFile = PathList::SHADER_DIR + "Shader.hlsl";
		desc.VertexShader = "VShader";
		desc.FragmentShader = "fp_main";
	}
	else if (Renderer::GetAPI() == GfxAPI::GL)
	{
		desc.DXShaderFile = "";
		desc.VertexShader = PathList::SHADER_DIR + "glVert.txt";
		desc.FragmentShader = PathList::SHADER_DIR + "glFrag.txt";
	}
	desc.DiffuseWrapMode = TextureAddressMode::WRAP;

	Material* testMat1 = ResourceManager::CreateMaterial(desc, "material1");
	if (!testMat1)
	{
		PRINTL("SHADER FAILED TO COMPILE<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
	}

	///desc.DiffuseMap = PathList::TEXTURE_DIR + "bkg.bmp";
	//desc.AmbientCol = Colour(0.1, 0.1, 0.1, 1);
	//desc.SpecularReflectivity = Colour(0, 0, 0, 1);
	Material* testMat2 = ResourceManager::CreateMaterial(desc, "material2");


	//////////////////////////////////////////     MESH    ////////////////////////////
	m_pQuadMesh = new Mesh();

	float size{ 5.5f };
	float fbDist = 0.01f;

	//front
	m_pQuadMesh->AddVertex(Vertex(-size, -size, -fbDist, 0, 0, -1, 0, 0));
	m_pQuadMesh->AddVertex(Vertex(size, -size, -fbDist, 0, 0, -1, 1, 0));
	m_pQuadMesh->AddVertex(Vertex(size, size, -fbDist, 0, 0, -1, 1, 1));
	m_pQuadMesh->AddVertex(Vertex(-size, size, -fbDist, 0, 0, -1, 0, 1));

	std::vector<unsigned> indices;
	indices.push_back(0);
	indices.push_back(1);
	indices.push_back(2);
	indices.push_back(0);
	indices.push_back(2);
	indices.push_back(3);

	m_pQuadMesh->CreateVertexBuffer(indices);

	///////////////////////////////////////////////////////////   MODELS    ////////////////////////////////////////////

	//Meshes and materials can be reused by many models 
	ModelComponent* mc1 = new ModelComponent("testModel", m_pQuadMesh, testMat1);
	ModelComponent* mc2 = new ModelComponent("testModel2", m_pQuadMesh, testMat1);
	ModelComponent* bkgQuadModel = new ModelComponent("testModel3", m_pQuadMesh, testMat2);

	PropellerBehaviour* propBehA = new PropellerBehaviour(KLM_AXIS::Z_AXIS, -1);

	Entity* bkgQuad = new Entity("test3");
	bkgQuad->AddComponent(std::unique_ptr<ModelComponent>(bkgQuadModel));
	bkgQuad->GetTransform()->SetPosition(Vec3(0, 0, 50));
	bkgQuad->GetTransform()->SetScale(Vec3(20, 20, 1));
	AddEntity(bkgQuad);


	parentQuad = new Entity("test1");
	parentQuad->AddComponent(std::unique_ptr<ModelComponent>(mc1));
	parentQuad->AddComponent(std::unique_ptr<BehaviourComponent>(propBehA));
	parentQuad->GetTransform()->SetPosition(Vec3(-10, 0, 11));
	parentQuad->GetTransform()->SetRotation(Vec3(0, glm::radians(45.0f), 0));
	AddEntity(parentQuad);


	PropellerBehaviour* propBehB = new PropellerBehaviour(KLM_AXIS::Z_AXIS, 9);
	Entity* childQuad = new Entity("test2");
	childQuad->AddComponent(std::unique_ptr<ModelComponent>(mc2));
	childQuad->AddComponent(std::unique_ptr<BehaviourComponent>(propBehB));
	childQuad->GetTransform()->SetPosition(Vec3(0, 10, 0));
	childQuad->GetTransform()->SetRotation(Vec3(0, 0, 0));
	childQuad->GetTransform()->SetScale(Vec3(0.5, 0.5, 0.5));
	AddEntity(childQuad);
	parentQuad->AddChild(childQuad);




	/////////////////////////////////////////////  CAMERA ENTITY  ////////////////////////////////////////////

	Camera* cam = new Camera(ProjectionType::PERSPECTIVE, 60.0f, 0.1f, 1000.0f);

	Renderer::SetActiveCamera(cam);
	Entity* p_CameraEntity = new Entity("cameraEntity");
	p_CameraEntity->AddComponent(std::unique_ptr<Camera>(cam));
	p_CameraEntity->GetTransform()->SetPosition(Vec3(0, 0, -35));
	AddEntity(p_CameraEntity);


	//m_pDirectionalLight = new DirectionalLight(Vec3(10., 0, -10.f), Colour(0, 0, 0, 1), Colour(0.5, 0.5, 1, 1), Colour(0.1, 0.1, 0.1, 1));
	m_pSpotLight = new Spotlight(
		Vec3(0, 0, -20.0f),
		Colour(0, 0, 0, 1),
		Colour(0.7, 0.9, 1, 1),
		Colour(0, 0, 1, 1),
		Vec3(0, 0, 1),
		45.0f
		);

}

void ExampleScene::Update(float deltaTime, float totalTime)

{
	//PRINTL("Update(" + ToString(deltaTime) + ", " + ToString(totalTime) + ")");
}

void ExampleScene::OnExit()

{
	PRINTL("OnExit()");
	delete m_pQuadMesh;
	delete m_pSpotLight;
	delete m_pDirectionalLight;

}

void ExampleScene::PostUpdate()
{
}

void ExampleScene::OnKeyPressed(const int key, const KeyState state)

{
	PRINTL("Key Pressed: " + ToString(key));

	if (state == KeyState::RELEASED)
	{

	}
	else if (state == KeyState::PRESSED)
	{
		switch (key)
		{
		case VK_LEFT:
		{
			parentQuad->GetTransform()->UpdatePosition(-1, 0, 0);
			//p_CameraEntity->GetTransform()->UpdatePosition(-1, 0, 0);
		}
		break;
		case VK_RIGHT:
		{
			parentQuad->GetTransform()->UpdatePosition(1, 0, 0);
			//p_CameraEntity->GetTransform()->UpdatePosition(1, 0, 0);
		}
		break;
		case VK_UP:
		{
			parentQuad->GetTransform()->UpdatePosition(0, 1, 0);
			//p_CameraEntity->GetTransform()->UpdatePosition(0, 1, 0);
		}
		break;
		case VK_DOWN:
		{
			parentQuad->GetTransform()->UpdatePosition(0, -1, 0);
			//p_CameraEntity->GetTransform()->UpdatePosition(0, -1, 0);
		}
		break;

		case 0x57: // W
		{
			m_pSpotLight->UpdatePosition(0, 1, 0);
		}
		break;
		case 0x53: // S
		{
			m_pSpotLight->UpdatePosition(0, -1, 0);
		}
		break;
		case 0x41: // A
		{
			m_pSpotLight->UpdatePosition(-1, 0, 0);
		}
		break;
		break;
		case 0x44: // D
		{
			m_pSpotLight->UpdatePosition(1, 0, 0);

		}
		break;

		case VK_ESCAPE: // esc
		{

		}
		break;

		default:
			break;
		}

	}
}

void ExampleScene::OnMouseMove(const int x, const int y)
{
	//PRINTL("Mouse Move: " + ToString(x) + ", " + ToString(y));
}

void ExampleScene::OnMouseButtonUp(MouseButton const button)
{
	PRINTL("Mouse Button Up: " + ToString(static_cast<int>(button)));
}

void ExampleScene::OnMouseButtonDown(MouseButton const button)
{
	PRINTL("Mouse Button Down: " + ToString(static_cast<int>(button)));
}
