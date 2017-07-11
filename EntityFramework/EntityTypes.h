#pragma once
#include <vector>
#include <memory>
namespace KLM_FRAMEWORK
{
	class Entity;
	using EntityUnique = std::unique_ptr<Entity>;
	using ListOfEntities = std::vector<EntityUnique>;
}