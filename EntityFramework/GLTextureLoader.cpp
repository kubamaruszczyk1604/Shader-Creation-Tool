#include "GLTextureLoader.h"



namespace KLM_FRAMEWORK
{
	GLTextureLoader::~GLTextureLoader()
	{
	}


	GLuint GLTextureLoader::LoadTexture(const std::string & path)
	{
		FREE_IMAGE_FORMAT texFormat = FreeImage_GetFileType(path.c_str(), 0);
		FIBITMAP* imagen = FreeImage_Load(texFormat, path.c_str());
		if (!imagen) return -1;


		FIBITMAP* temp = imagen;
		imagen = FreeImage_ConvertTo32Bits(imagen);
		FreeImage_Unload(temp);

		int w = FreeImage_GetWidth(imagen);
		int h = FreeImage_GetHeight(imagen);
		DEBUG("The size of the image " + path + " is: " + ToString(w) + "*"+ ToString(h)); 

		GLubyte* textura = new GLubyte[4 * w*h];
		char* pixeles = (char*)FreeImage_GetBits(imagen);
		//FreeImage loads in BGR format, so you need to swap some bytes(Or use GL_BGR).

		for (int j = 0; j<w*h; j++) 
		{
			textura[j * 4 + 0] = pixeles[j * 4 + 2];
			textura[j * 4 + 1] = pixeles[j * 4 + 1];
			textura[j * 4 + 2] = pixeles[j * 4 + 0];
			textura[j * 4 + 3] = pixeles[j * 4 + 3];
			//cout<<j<<": "<<textura[j*4+0]<<"**"<<textura[j*4+1]<<"**"<<textura[j*4+2]<<"**"<<textura[j*4+3]<<endl;
		}

		//Now generate the OpenGL texture object 
		GLuint textureID;
		glGenTextures(1, &textureID);
		glBindTexture(GL_TEXTURE_2D, textureID);
		glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, w, h, 0, GL_RGBA, GL_UNSIGNED_BYTE, (GLvoid*)textura);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR_MIPMAP_LINEAR);
		return textureID;
	}
}
