#include "Scene.h"


namespace KLM_FRAMEWORK
{

	Scene::Scene() :m_pEntityManager{ new EntityManager() }
	{
	}

	Scene::~Scene()
	{
		m_pEntityManager->RemoveAllEntities();//not needed really
		delete m_pEntityManager;
		m_pEntityManager = nullptr;
	}



	void Scene::AddEntity(Entity* entity)
	{
		m_pEntityManager->AddEntity(std::unique_ptr<Entity>(entity));
	}

	void Scene::RemoveEntity(const std::string & name)
	{
		m_pEntityManager->RemoveEntity(name);
	}

	Entity * Scene::FindEntity(const std::string & name)
	{
		return m_pEntityManager->FindEntity(name);
	}

	void Scene::OnKeyPressed(const int key, const KeyState state)
	{
	}

	void Scene::OnMouseMove(const int x, const int y)
	{
	}

	void Scene::OnMouseButtonUp(MouseButton const button)
	{
	}

	void Scene::OnMouseButtonDown(MouseButton const button)
	{
	}
}