#include "StdIncludes.h"

namespace KLM_FRAMEWORK
{

#ifdef _KLM_CSHARP
	const std::string PathList::SHADER_DIR = "../../../Data/Shaders/";
	const std::string PathList::TEXTURE_DIR = "../../../Assets/Textures/";
	const std::string PathList::ASSETS_DIR = "../../../Assets/";

#else
	const std::string PathList::SHADER_DIR = "../Data/Shaders/";
	const std::string PathList::TEXTURE_DIR = "../Assets/Textures/";
	const std::string PathList::ASSETS_DIR = "../Assets/";
#endif



}
