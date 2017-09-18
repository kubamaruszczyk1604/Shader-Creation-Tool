#include "Behaviours.h"
#include "GeometryGenerator.h"

const std::string SCTScene::TYPE_QUAD = "QUAD";
const std::string SCTScene::TYPE_SPHERE = "SPHERE";
const std::string SCTScene::TYPE_BOX = "BOX";
const std::string SCTScene::TYPE_PROPELLER = "PROPELLER";

Entity* SCTScene::s_pMainObjectEntity{ nullptr };
Entity* SCTScene::s_CameraEntity{ nullptr };
float SCTScene::s_DeltaTime{ 0.0f };
std::string SCTScene::s_ModelType = "";

void SCTScene::CreateSphere(Material* dummyMat)
{
	GeometryGenerator::GenerateSphere(40.0f, 20, 20, m_pSphereMesh);
	ModelComponent* bkgSphereModel = new ModelComponent("SPHERE_MODEL", m_pSphereMesh, dummyMat);
	Entity* sphereEntity = new Entity("MAIN_ENTITY");
    sphereEntity->AddComponent(std::unique_ptr<ModelComponent>(bkgSphereModel));
	sphereEntity->GetTransform()->SetPosition(Vec3(0, 0, 0));
	sphereEntity->GetTransform()->SetScale(Vec3(1, 1, 1));
	AddEntity(sphereEntity);
	s_pMainObjectEntity = sphereEntity;
}

void SCTScene::CreateQuad(Material* dummyMat)
{
	GeometryGenerator::GenerateQuad(40.0f, m_pQuadMesh);
	ModelComponent* bkgQuadModel = new ModelComponent("QUAD_MODEL", m_pQuadMesh, dummyMat);
	Entity* quadEntity = new Entity("MAIN_ENTITY");
	quadEntity->AddComponent(std::unique_ptr<ModelComponent>(bkgQuadModel));
	quadEntity->GetTransform()->SetPosition(Vec3(0, 0, 0));
	quadEntity->GetTransform()->SetScale(Vec3(1, 1, 1));
	AddEntity(quadEntity);
	s_pMainObjectEntity = quadEntity;
}

void SCTScene::CreatePropeller(Material* dummyMat)
{
	GeometryGenerator::GenerateSphere(18.0f, 20, 20, m_pSphereMesh);
	GeometryGenerator::GenerateIrregBlock(37, 0, 20,6, 5, 5, m_pQuadMesh);
	ModelComponent* bkgSphereModel = new ModelComponent("SPHERE_MODEL", m_pSphereMesh, dummyMat);

	PropellerBehaviour* propBehA = new PropellerBehaviour(KLM_AXIS::Z_AXIS, -1);

	Entity* sphereMainEntity = new Entity("MAIN_ENTITY");
	sphereMainEntity->AddComponent(std::unique_ptr<ModelComponent>(bkgSphereModel));
	sphereMainEntity->AddComponent(std::unique_ptr<BehaviourComponent>(propBehA));
	sphereMainEntity->GetTransform()->SetPosition(Vec3(0, 0, 0));
	sphereMainEntity->GetTransform()->SetScale(Vec3(1, 1, 0.25f));
	AddEntity(sphereMainEntity);
	for (int i = 0; i < 4; ++i)
	{
		ModelComponent* bkgQuadModel = new ModelComponent("QUAD_MODEL"+ std::to_string(i), m_pQuadMesh, dummyMat);
		Entity* blade = new Entity("test2");
		blade->AddComponent(std::unique_ptr<ModelComponent>(bkgQuadModel));
		blade->GetTransform()->SetPosition(Vec3(0, 0, 0));
		blade->GetTransform()->SetRotation(Vec3(0, 0, ToRadians(i*90.0f)));
		AddEntity(blade);
		sphereMainEntity->AddChild(blade);
	}


	s_pMainObjectEntity = sphereMainEntity;
}

void SCTScene::CreateBox(Material * dummyMat)
{
	GeometryGenerator::GenerateIrregBlock(5, 5, 5, 5, 5, 5, m_pQuadMesh);
	ModelComponent* bkgQuadModel = new ModelComponent("QUAD_MODEL", m_pQuadMesh, dummyMat);
	Entity* quadEntity = new Entity("MAIN_ENTITY");
	quadEntity->AddComponent(std::unique_ptr<ModelComponent>(bkgQuadModel));
	quadEntity->GetTransform()->SetPosition(Vec3(0, 0, 0));
	quadEntity->GetTransform()->SetScale(Vec3(5, 5, 5));
	AddEntity(quadEntity);
	s_pMainObjectEntity = quadEntity;
}

void SCTScene::OnStart()
{
	PRINTL("OnStart()");

	////////////////////////////////////////////   MATERIALS //////////////////////////
	MaterialDescription desc;
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

	Material* dummyMat = ResourceManager::CreateMaterial(desc, "material1");
	if (!dummyMat)
	{
		PRINTL("SHADER FAILED TO COMPILE<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
	}

	if (s_ModelType == TYPE_QUAD)
	{	
		CreateQuad(dummyMat);
	}
	else if (s_ModelType == TYPE_SPHERE)
	{
		CreateSphere(dummyMat);
	}
	else if (s_ModelType == TYPE_BOX)
	{
		CreateBox(dummyMat);
	}
	else if (s_ModelType == TYPE_PROPELLER)
	{
		CreatePropeller(dummyMat);
	}
	else
	{
		CreateQuad(dummyMat);
	}
	/////////////////////////////////////////////  CAMERA ENTITY  ////////////////////////////////////////////

	Camera* cam = new Camera(ProjectionType::PERSPECTIVE, 60.0f, 0.1f, 1000.0f);

	Renderer::SetActiveCamera(cam);
	s_CameraEntity = new Entity("cameraEntity");
	s_CameraEntity->AddComponent(std::unique_ptr<Camera>(cam));
	s_CameraEntity->GetTransform()->SetPosition(Vec3(0, 0, -85));
	AddEntity(s_CameraEntity);
}

void SCTScene::Update(float deltaTime, float totalTime)

{
	//PRINTL("Update(" + ToString(deltaTime) + ", " + ToString(totalTime) + ")");
	s_DeltaTime = deltaTime;
}

void SCTScene::OnExit()

{
	PRINTL("OnExit()");
	delete m_pQuadMesh;
	delete m_pSphereMesh;
	delete m_pSpotLight;
	delete m_pDirectionalLight;

}

void SCTScene::PostUpdate()
{
}

void SCTScene::RotateObject(float x, float y, float z)
{
   s_pMainObjectEntity->GetTransform()->RotateXYZ(x, y, z);
}

void SCTScene::Zoom(float amount)
{
	s_CameraEntity->GetTransform()->UpdatePosition(0, 0, amount);
}



void SCTScene::OnKeyPressed(const int key, const KeyState state)

{
	PRINTL("Key Pressed: " + ToString(key));

	//if (state == KeyState::RELEASED)
	//{

	//}
	//else if (state == KeyState::PRESSED)
	//{
	//	switch (key)
	//	{
	//	case VK_LEFT:
	//	{
	//		parentQuad->GetTransform()->UpdatePosition(-1, 0, 0);
	//		//p_CameraEntity->GetTransform()->UpdatePosition(-1, 0, 0);
	//	}
	//	break;
	//	case VK_RIGHT:
	//	{
	//		parentQuad->GetTransform()->UpdatePosition(1, 0, 0);
	//		//p_CameraEntity->GetTransform()->UpdatePosition(1, 0, 0);
	//	}
	//	break;
	//	case VK_UP:
	//	{
	//		parentQuad->GetTransform()->UpdatePosition(0, 1, 0);
	//		//p_CameraEntity->GetTransform()->UpdatePosition(0, 1, 0);
	//	}
	//	break;
	//	case VK_DOWN:
	//	{
	//		parentQuad->GetTransform()->UpdatePosition(0, -1, 0);
	//		//p_CameraEntity->GetTransform()->UpdatePosition(0, -1, 0);
	//	}
	//	break;

	//	case 0x57: // W
	//	{
	//		m_pSpotLight->UpdatePosition(0, 1, 0);
	//	}
	//	break;
	//	case 0x53: // S
	//	{
	//		m_pSpotLight->UpdatePosition(0, -1, 0);
	//	}
	//	break;
	//	case 0x41: // A
	//	{
	//		m_pSpotLight->UpdatePosition(-1, 0, 0);
	//	}
	//	break;
	//	break;
	//	case 0x44: // D
	//	{
	//		m_pSpotLight->UpdatePosition(1, 0, 0);

	//	}
	//	break;

	//	case VK_ESCAPE: // esc
	//	{

	//	}
	//	break;

	//	default:
	//		break;
	//	}

	//}
}

void SCTScene::OnMouseMove(const int x, const int y)
{
	//PRINTL("Mouse Move: " + ToString(x) + ", " + ToString(y));
}

void SCTScene::OnMouseButtonUp(MouseButton const button)
{
	PRINTL("Mouse Button Up: " + ToString(static_cast<int>(button)));
}

void SCTScene::OnMouseButtonDown(MouseButton const button)
{
	PRINTL("Mouse Button Down: " + ToString(static_cast<int>(button)));
}
