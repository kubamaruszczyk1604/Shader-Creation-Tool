#pragma once
#include "WindowsApp.h"
#include "ControlApp.h"

using namespace KLM_FRAMEWORK;

///////////////////   TEST DEMO BEHAVIOURS //////////////////////////////////////////

class PropellerBehaviour : public BehaviourComponent
{
private:
	KLM_AXIS m_Axis;
	float m_Speed;

public:
	PropellerBehaviour(KLM_AXIS rotationAxis, float speed)
		:BehaviourComponent("testName"), m_Axis{ rotationAxis }, m_Speed{ speed } {}

	~PropellerBehaviour() {}

	void OnStart()
	{
		PRINTL("PROPELLER BEHAVIOUR on start");
	}
	void Update(float deltaTime, float totalTime)
	{
		//this->GetParent()->GetTransform()->SetRotationY(totalTime * 3);
		this->GetParent()->GetTransform()->SetRotation(m_Axis, totalTime * m_Speed);
	}
	void OnExit()
	{
		PRINTL("PROPELLER BEHAVIOUR on exit");
	}
};



class SCTScene :public Scene
{

private:
	static const std::string TYPE_QUAD;
	static const std::string TYPE_SPHERE;
	static const std::string TYPE_BOX;
	static const std::string TYPE_PROPELLER;

private:
	Mesh* m_pQuadMesh{ nullptr };
	Mesh* m_pSphereMesh{ nullptr };
	Entity* parentQuad{ nullptr };
	Spotlight* m_pSpotLight{ nullptr };
	DirectionalLight* m_pDirectionalLight{ nullptr };
	static Entity* s_pMainObjectEntity;
	static Entity* s_CameraEntity;
	static float s_DeltaTime;
	static std::string s_ModelType;

public:
	~SCTScene() {}


	void OnStart();
	void Update(float deltaTime, float totalTime = 0);
	void OnExit();
	void PostUpdate();
	void RotateObject(float x, float y, float z);
	void Zoom(float amount);

	//InputCallbacks
	void OnKeyPressed(const int key, const KeyState state);
	void OnMouseMove(const int x, const int y);
	void OnMouseButtonUp(MouseButton const button);
	void OnMouseButtonDown(MouseButton const button);

	//static

	static void SetModelType(const std::string& model) { s_ModelType = model; }

};


